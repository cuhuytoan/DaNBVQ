﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HNM.DataNC.ModelsPayment
{
    public partial class PaymentContext : DbContext
    {
        public PaymentContext()
        {
        }

        public PaymentContext(DbContextOptions<PaymentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DiscountConfig> DiscountConfig { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<PaymentBanking> PaymentBanking { get; set; }
        public virtual DbSet<PaymentExpVat> PaymentExpVat { get; set; }
        public virtual DbSet<PaymentLog> PaymentLog { get; set; }
        public virtual DbSet<PaymentSetting> PaymentSetting { get; set; }
        public virtual DbSet<PaymentStatus> PaymentStatus { get; set; }
        public virtual DbSet<PaymentType> PaymentType { get; set; }
        public virtual DbSet<Services> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiscountConfig>(entity =>
            {
                entity.HasKey(e => e.Discount_ID);

                entity.Property(e => e.Percent_Discount).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Order_ID);

                entity.Property(e => e.Order_ID).HasMaxLength(128);

                entity.Property(e => e.ApprovedBy).HasMaxLength(128);

                entity.Property(e => e.ApprovedDate).HasColumnType("datetime");

                entity.Property(e => e.CreateBy).HasMaxLength(128);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerAddress).HasMaxLength(500);

                entity.Property(e => e.CustomerEmail).HasMaxLength(50);

                entity.Property(e => e.CustomerName).HasMaxLength(500);

                entity.Property(e => e.CustomerPhone).HasMaxLength(50);

                entity.Property(e => e.DisCount).HasColumnType("money");

                entity.Property(e => e.LastEditBy).HasMaxLength(128);

                entity.Property(e => e.LastEditDate).HasColumnType("datetime");

                entity.Property(e => e.OrderCode).HasMaxLength(100);

                entity.Property(e => e.ProductBrandName).HasMaxLength(500);

                entity.Property(e => e.SalesName).HasMaxLength(500);

                entity.Property(e => e.SalesUserName).HasMaxLength(128);

                entity.Property(e => e.Sum).HasColumnType("money");

                entity.Property(e => e.Total).HasColumnType("money");

                entity.Property(e => e.VAT).HasColumnType("money");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetail_ID)
                    .HasName("PK_ProductOrderDetail");

                entity.Property(e => e.OrderDetail_ID).HasMaxLength(128);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.DisCount).HasColumnType("money");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.OrderCode).HasMaxLength(100);

                entity.Property(e => e.Order_ID).HasMaxLength(128);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.ProductBrandName).HasMaxLength(500);

                entity.Property(e => e.ProductName).HasMaxLength(200);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Sum).HasColumnType("money");

                entity.Property(e => e.Total).HasColumnType("money");

                entity.Property(e => e.Unit).HasMaxLength(50);

                entity.Property(e => e.VAT).HasColumnType("money");
            });

            modelBuilder.Entity<PaymentBanking>(entity =>
            {
                entity.Property(e => e.BankCode).HasMaxLength(200);

                entity.Property(e => e.BankInfo).HasMaxLength(200);

                entity.Property(e => e.BankLogo).HasMaxLength(200);

                entity.Property(e => e.BankName).HasMaxLength(200);

                entity.Property(e => e.BankNumber).HasMaxLength(50);

                entity.Property(e => e.BankReceive).HasMaxLength(200);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.LastEditDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PaymentExpVat>(entity =>
            {
                entity.Property(e => e.BuyerName).HasMaxLength(100);

                entity.Property(e => e.CompanyAddress).HasMaxLength(200);

                entity.Property(e => e.CompanyName).HasMaxLength(200);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.LastEditDate).HasColumnType("datetime");

                entity.Property(e => e.OrderCode).HasMaxLength(50);

                entity.Property(e => e.ReceiveBillAddress).HasMaxLength(200);

                entity.Property(e => e.TaxCode).HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentLog>(entity =>
            {
                entity.HasKey(e => e.PaymentLog_ID);

                entity.Property(e => e.Amount).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.BankCode).HasMaxLength(50);

                entity.Property(e => e.BankTranNo).HasMaxLength(50);

                entity.Property(e => e.CardType).HasMaxLength(50);

                entity.Property(e => e.CreateBy).HasMaxLength(128);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.OrderInfo).HasMaxLength(500);

                entity.Property(e => e.Order_ID).HasMaxLength(128);

                entity.Property(e => e.ProductBrandName).HasMaxLength(500);

                entity.Property(e => e.ResponseCode).HasMaxLength(50);

                entity.Property(e => e.TransactionNo).HasMaxLength(50);

                entity.Property(e => e.TxnRef).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<PaymentSetting>(entity =>
            {
                entity.Property(e => e.vnp_HashSecret).HasMaxLength(200);

                entity.Property(e => e.vnp_Returnurl).HasMaxLength(200);

                entity.Property(e => e.vnp_TmnCode).HasMaxLength(200);

                entity.Property(e => e.vnp_Url).HasMaxLength(200);

                entity.Property(e => e.vnpay_api_url).HasMaxLength(200);
            });

            modelBuilder.Entity<PaymentStatus>(entity =>
            {
                entity.HasKey(e => e.PaymentStatus_ID)
                    .HasName("PK_ProductOrderStatus");

                entity.Property(e => e.PaymentStatus_ID).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.HasKey(e => e.PaymentType_ID)
                    .HasName("PK_ProductOrderType");

                entity.Property(e => e.PaymentType_ID).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Services>(entity =>
            {
                entity.HasKey(e => e.ServiceId)
                    .HasName("PK__Services__C51BB00AC978097E");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.LastEditDate).HasColumnType("datetime");

                entity.Property(e => e.ServiceName).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}