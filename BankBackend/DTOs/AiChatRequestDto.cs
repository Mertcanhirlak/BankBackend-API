namespace BankBackend.DTOs
{
    public class AiChatRequestDto
    {
        public int MusteriId { get; set; }
        public string UserMessage { get; set; } = string.Empty;
        public string Language { get; set; } = "tr";
    }
}