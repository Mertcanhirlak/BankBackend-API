using BankBackend.DTOs;
using BankBackend.Interfaces;
using BankBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BankBackend.Services
{
    public class AiAdvisoryService : IAiAdvisoryService
    {
        private readonly BankDbContext _context;
        private readonly Kernel? _kernel;
        private readonly bool _isAiEnabled;

        public AiAdvisoryService(BankDbContext context, IConfiguration configuration)
        {
            _context = context;
            
            // API Key check
            var apiKey = configuration["AiSettings:OpenAiKey"];
            var modelId = configuration["AiSettings:ModelId"] ?? "gpt-3.5-turbo";

            if (!string.IsNullOrEmpty(apiKey))
            {
                var builder = Kernel.CreateBuilder();
                builder.AddOpenAIChatCompletion(modelId, apiKey);
                _kernel = builder.Build();
                _isAiEnabled = true;
            }
            else
            {
                _isAiEnabled = false; // Switch to Mock mode
            }
        }

        public async Task<AiAdviceResponseDto> AnalyzeFinancialStatusAsync(int musteriId, string language = "tr")
        {
            // 1. Get the User's Last 10 Transactions
            var islemler = await _context.IslemHareketleris
                .Where(i => i.GonderenHesap.MusteriId == musteriId || i.AliciHesap.MusteriId == musteriId)
                .OrderByDescending(i => i.IslemTarihi)
                .Take(10)
                .Select(i => new { i.Aciklama, i.Miktar, Tarih = i.IslemTarihi.HasValue ? i.IslemTarihi.Value.ToShortDateString() : "Date Unknown" })
                .ToListAsync();

            var bakiye = await _context.Hesaplars
                .Where(h => h.MusteriId == musteriId)
                .SumAsync(h => h.Bakiye) ?? 0;

            if (!_isAiEnabled)
            {
                return GenerateMockAdvice(bakiye, language);
            }

            // 2. Prepare AI Prompt (Based on Language)
            var sb = new StringBuilder();
            
            if (language == "en")
            {
                sb.AppendLine($"User Total Balance: {bakiye} TRY.");
                sb.AppendLine("Recent Transactions:");
                foreach (var islem in islemler)
                {
                    sb.AppendLine($"- {islem.Tarih}: {islem.Miktar} TRY ({islem.Aciklama})");
                }
                sb.AppendLine("\nYou are an expert financial advisor. Based on this data, provide 3 savings tips and 1 short general comment. Respond in English.");
            }
            else
            {
                sb.AppendLine($"Kullanıcının toplam bakiyesi: {bakiye} TL.");
                sb.AppendLine("Son Harcamalar:");
                foreach (var islem in islemler)
                {
                    sb.AppendLine($"- {islem.Tarih}: {islem.Miktar} TL ({islem.Aciklama})");
                }
                sb.AppendLine("\nSen bir finans uzmanısın. Bu verilere göre kullanıcıya 3 maddelik tasarruf tavsiyesi ve 1 kısa genel yorum yap. Cevabı Türkçe ver.");
            }

            // 3. Ask AI
            try
            {
                if (_kernel == null) return GenerateMockAdvice(bakiye, language);

                var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
                var history = new ChatHistory();
                
                string systemMsg = language == "en" 
                    ? "You are a helpful banking assistant. Your answers must be polite and financially literate."
                    : "You are a helpful banking assistant. Your answers must be polite and financially literate (Respond in Turkish).";
                
                history.AddSystemMessage(systemMsg);
                history.AddUserMessage(sb.ToString());

                var result = await chatCompletionService.GetChatMessageContentAsync(history);
                
                var content = result?.Content ?? (language == "en" ? "Analysis complete." : "Analiz tamamlandı.");
                
                // Basic parsing: Extract lines starting with -, *, or a digit as recommendations
                var recommendations = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(line => line.Trim().StartsWith("-") || line.Trim().StartsWith("*") || (line.Trim().Length > 0 && char.IsDigit(line.Trim()[0])))
                    .Select(line => line.Trim())
                    .ToList();

                return new AiAdviceResponseDto
                {
                    Baslik = language == "en" ? "AI Financial Report" : "Yapay Zeka Finans Raporu",
                    GenelYorum = content,
                    Tavsiyeler = recommendations.Any() ? recommendations : new List<string>()
                };
            }
            catch (Exception)
            {
                return GenerateMockAdvice(bakiye, language);
            }
        }

        public async Task<string> ChatWithAdvisorAsync(int musteriId, string userMessage, string language = "tr")
        {
            // 1. Context Gathering (Last 20 transactions for better context)
            var islemler = await _context.IslemHareketleris
                .Where(i => i.GonderenHesap.MusteriId == musteriId || i.AliciHesap.MusteriId == musteriId)
                .OrderByDescending(i => i.IslemTarihi)
                .Take(20)
                .Select(i => new { i.Aciklama, i.Miktar, Tarih = i.IslemTarihi.HasValue ? i.IslemTarihi.Value.ToShortDateString() : "Unknown" })
                .ToListAsync();

            var bakiye = await _context.Hesaplars
                .Where(h => h.MusteriId == musteriId)
                .SumAsync(h => h.Bakiye) ?? 0;

            if (!_isAiEnabled)
            {
                return language == "en" 
                    ? "I am currently in demo mode. Please configure OpenAI API Key to chat with me about your finances." 
                    : "Şu an demo modundayım. Finansal durumunuz hakkında sohbet etmek için lütfen OpenAI API anahtarını yapılandırın.";
            }

            // 2. Build Prompt
            var sb = new StringBuilder();
            if (language == "en")
            {
                sb.AppendLine("Context Data:");
                sb.AppendLine($"Total Balance: {bakiye} TRY.");
                sb.AppendLine("Recent Transactions:");
                foreach (var islem in islemler) sb.AppendLine($"- {islem.Tarih}: {islem.Miktar} TRY ({islem.Aciklama})");
                sb.AppendLine("\nInstruction: You are a helpful financial assistant. Answer the user's question based on the data above. Be concise and friendly.");
            }
            else
            {
                sb.AppendLine("Bağlam Verisi:");
                sb.AppendLine($"Toplam Bakiye: {bakiye} TL.");
                sb.AppendLine("Son İşlemler:");
                foreach (var islem in islemler) sb.AppendLine($"- {islem.Tarih}: {islem.Miktar} TL ({islem.Aciklama})");
                sb.AppendLine("\nTalimat: Sen yardımsever bir finans asistanısın. Yukarıdaki verilere dayanarak kullanıcının sorusunu cevapla. Kısa ve samimi ol.");
            }

            // 3. Ask AI
            try
            {
                if (_kernel == null) return "AI Kernel Error";

                var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
                var history = new ChatHistory();
                
                // System Persona
                history.AddSystemMessage(language == "en" ? "You are a smart banking assistant." : "Sen akıllı bir banka asistanısın.");
                
                // User Query with Context
                history.AddUserMessage(sb.ToString() + $"\n\nUser Question: {userMessage}");

                var result = await chatCompletionService.GetChatMessageContentAsync(history);
                return result?.Content ?? (language == "en" ? "I couldn't generate a response." : "Bir cevap oluşturamadım.");
            }
            catch (Exception ex)
            {
                return language == "en" ? $"Error: {ex.Message}" : $"Hata: {ex.Message}";
            }
        }

        private AiAdviceResponseDto GenerateMockAdvice(decimal bakiye, string language)
        {
            if (language == "en")
            {
                return new AiAdviceResponseDto
                {
                    Baslik = "Financial Summary (Demo Mode)",
                    GenelYorum = bakiye > 5000 
                        ? "Your financial situation looks great, consider investing."
                        : "I suggest you pay more attention to your expenses.",
                    Tavsiyeler = new List<string> 
                    { 
                        "Cancel unnecessary subscriptions.",
                        "Save 10% of your income every month.",
                        "Create an emergency fund."
                    }
                };
            }
            else
            {
                return new AiAdviceResponseDto
                {
                    Baslik = "Finansal Durum Özeti (Demo Mod)",
                    GenelYorum = bakiye > 5000 
                        ? "Mali durumunuz gayet iyi görünüyor, yatırım yapmayı düşünebilirsiniz."
                        : "Harcamalarınıza biraz daha dikkat etmenizi öneririm.",
                    Tavsiyeler = new List<string> 
                    { 
                        "Gereksiz aboneliklerinizi iptal edin.",
                        "Her ay gelirinizin %10'unu biriktirin.",
                        "Acil durum fonu oluşturun."
                    }
                };
            }
        }
    }
}