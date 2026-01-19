using System;
using System.Collections.Generic;
using BankBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace BankBackend.Data;

public partial class BankDbContext : DbContext
{
    public BankDbContext()
    {
    }

    public BankDbContext(DbContextOptions<BankDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Hesaplar> Hesaplars { get; set; }

    public virtual DbSet<IslemHareketleri> IslemHareketleris { get; set; }

    public virtual DbSet<Musteriler> Musterilers { get; set; }

    public virtual DbSet<RiskMasasi> RiskMasasis { get; set; }

    // Stored Procedure Sonucu İçin
    public virtual DbSet<GenelIstatistik> GenelIstatistikler { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // SP Sonucu (Keyless)
        modelBuilder.Entity<GenelIstatistik>().HasNoKey();

        modelBuilder.Entity<Hesaplar>(entity =>
        {
            entity.HasKey(e => e.HesapId).HasName("hesaplar_pkey");

            entity.ToTable("hesaplar");

            entity.HasIndex(e => e.HesapNo, "hesaplar_hesap_no_key").IsUnique();

            entity.Property(e => e.HesapId).HasColumnName("hesap_id");
            entity.Property(e => e.AktifMi)
                .HasDefaultValue(true)
                .HasColumnName("aktif_mi");
            entity.Property(e => e.Bakiye)
                .HasPrecision(15, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("bakiye");
            entity.Property(e => e.HesapNo)
                .HasMaxLength(20)
                .HasColumnName("hesap_no");

            entity.Property(e => e.HesapTuru)
                .HasMaxLength(20)
                .HasDefaultValue("VADESIZ")
                .HasColumnName("hesap_turu");

            entity.Property(e => e.MusteriId).HasColumnName("musteri_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("olusturma_tarihi");

            entity.HasOne(d => d.Musteri).WithMany(p => p.Hesaplars)
                .HasForeignKey(d => d.MusteriId)
                .HasConstraintName("fk_musteri");
        });

        modelBuilder.Entity<IslemHareketleri>(entity =>
        {
            entity.HasKey(e => e.IslemId).HasName("islem_hareketleri_pkey");

            entity.ToTable("islem_hareketleri");

            entity.Property(e => e.IslemId).HasColumnName("islem_id");
            entity.Property(e => e.Aciklama)
                .HasMaxLength(100)
                .HasColumnName("aciklama");
            entity.Property(e => e.AliciHesapId).HasColumnName("alici_hesap_id");
            entity.Property(e => e.GonderenHesapId).HasColumnName("gonderen_hesap_id");
            entity.Property(e => e.IslemTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("islem_tarihi");
            entity.Property(e => e.Miktar)
                .HasPrecision(15, 2)
                .HasColumnName("miktar");

            entity.HasOne(d => d.AliciHesap).WithMany(p => p.IslemHareketleriAliciHesaps)
                .HasForeignKey(d => d.AliciHesapId)
                .HasConstraintName("fk_alici");

            entity.HasOne(d => d.GonderenHesap).WithMany(p => p.IslemHareketleriGonderenHesaps)
                .HasForeignKey(d => d.GonderenHesapId)
                .HasConstraintName("fk_gonderen");
        });

        modelBuilder.Entity<Musteriler>(entity =>
        {
            entity.HasKey(e => e.MusteriId).HasName("musteriler_pkey");

            entity.ToTable("musteriler");

            entity.HasIndex(e => e.TcKimlikNo, "musteriler_tc_kimlik_no_key").IsUnique();

            entity.Property(e => e.MusteriId).HasColumnName("musteri_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(50)
                .HasColumnName("ad");
            entity.Property(e => e.KayitTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("kayit_tarihi");
            entity.Property(e => e.Rol)
                .HasMaxLength(20)
                .HasDefaultValueSql("'MUSTERI'::character varying")
                .HasColumnName("rol");
            entity.Property(e => e.Sifre)
                .HasMaxLength(50)
                .HasColumnName("sifre");
            entity.Property(e => e.Soyad)
                .HasMaxLength(50)
                .HasColumnName("soyad");
            entity.Property(e => e.TcKimlikNo)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("tc_kimlik_no");
        });

        modelBuilder.Entity<RiskMasasi>(entity =>
        {
            entity.HasKey(e => e.RiskId).HasName("risk_masasi_pkey");

            entity.ToTable("risk_masasi");

            entity.Property(e => e.RiskId).HasColumnName("risk_id");
            entity.Property(e => e.HesapId).HasColumnName("hesap_id");
            entity.Property(e => e.OlayTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("olay_tarihi");
            entity.Property(e => e.SupheliOlay)
                .HasMaxLength(200)
                .HasColumnName("supheli_olay");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
