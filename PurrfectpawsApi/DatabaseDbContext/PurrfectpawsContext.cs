﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PurrfectpawsApi.Models;

namespace PurrfectpawsApi.DatabaseDbContext;

public partial class PurrfectpawsContext : DbContext
{
    public PurrfectpawsContext()
    {
    }

    public PurrfectpawsContext(DbContextOptions<PurrfectpawsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MCategory> MCategories { get; set; }

    public virtual DbSet<MOrderMaster> MOrderMasters { get; set; }

    public virtual DbSet<MOrderStatus> MOrderStatuses { get; set; }

    public virtual DbSet<MPaymentStatus> MPaymentStatuses { get; set; }

    public virtual DbSet<MRole> MRoles { get; set; }

    public virtual DbSet<MSize> MSizes { get; set; }

    public virtual DbSet<TBillingAddress> TBillingAddresses { get; set; }

    public virtual DbSet<TCart> TCarts { get; set; }

    public virtual DbSet<TLeadLength> TLeadLengths { get; set; }

    public virtual DbSet<TOrder> TOrders { get; set; }

    public virtual DbSet<TProduct> TProducts { get; set; }

    public virtual DbSet<TProductBlobImage> TProductBlobImages { get; set; }

    public virtual DbSet<TProductDetail> TProductDetails { get; set; }

    public virtual DbSet<TProductImage> TProductImages { get; set; }

    public virtual DbSet<TShippingAddress> TShippingAddresses { get; set; }

    public virtual DbSet<TTransaction> TTransactions { get; set; }

    public virtual DbSet<TUser> TUsers { get; set; }

    public virtual DbSet<TVariation> TVariations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        .AddInterceptors(new SoftDeleteInterceptor());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.ToTable("M_Category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Category)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("category");
        });

        modelBuilder.Entity<MOrderMaster>(entity =>
        {
            entity.HasKey(e => e.OrderMasterId);

            entity.ToTable("M_Order_Master");

            entity.Property(e => e.OrderMasterId).HasColumnName("order_master_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.MOrderMasters)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_M_Order_Master_T_User");
        });

        modelBuilder.Entity<MOrderStatus>(entity =>
        {
            entity.HasKey(e => e.OrderStatusId);

            entity.ToTable("M_Order_Status");

            entity.Property(e => e.OrderStatusId).HasColumnName("order_status_id");
            entity.Property(e => e.Status)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("status");
        });

        modelBuilder.Entity<MPaymentStatus>(entity =>
        {
            entity.HasKey(e => e.PaymentStatusId);

            entity.ToTable("M_Payment_Status");

            entity.Property(e => e.PaymentStatusId).HasColumnName("payment_status_id");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("payment_status");
        });

        modelBuilder.Entity<MRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("M_Role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<MSize>(entity =>
        {
            entity.HasKey(e => e.SizeId);

            entity.ToTable("M_Size");

            entity.Property(e => e.SizeId).HasColumnName("size_id");
            entity.Property(e => e.SizeLabel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("size_label");
        });

        modelBuilder.Entity<TBillingAddress>(entity =>
        {
            entity.HasKey(e => e.BillingAddressId);

            entity.ToTable("T_Billing_Address");

            entity.Property(e => e.BillingAddressId).HasColumnName("billing_address_id");
            entity.Property(e => e.City)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("country");
            entity.Property(e => e.Postcode).HasColumnName("postcode");
            entity.Property(e => e.State)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.Street1)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("street_1");
            entity.Property(e => e.Street2)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("street_2");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.TBillingAddresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_Billing_Address_T_User");
        });

        modelBuilder.Entity<TCart>(entity =>
        {
            entity.HasKey(e => e.CartId);

            entity.ToTable("T_Cart");

            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.TCarts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_Cart_T_Product");

            entity.HasOne(d => d.User).WithMany(p => p.TCarts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_Cart_T_User");
        });

        modelBuilder.Entity<TLeadLength>(entity =>
        {
            entity.HasKey(e => e.LeadLengthId);

            entity.ToTable("T_Lead_Length");

            entity.Property(e => e.LeadLengthId).HasColumnName("lead_length_id");
            entity.Property(e => e.LeadLength)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("lead_length");
        });

        modelBuilder.Entity<TOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.ToTable("T_Order");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.BillingAddressId).HasColumnName("billing_address_id");
            entity.Property(e => e.OrderMasterId).HasColumnName("order_master_id");
            entity.Property(e => e.OrderStatusId).HasColumnName("order_status_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ShippingAddressId).HasColumnName("shipping_address_id");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_price");

            entity.HasOne(d => d.BillingAddress).WithMany(p => p.TOrders)
                .HasForeignKey(d => d.BillingAddressId)
                .HasConstraintName("FK_T_Order_T_Billing_Address");

            entity.HasOne(d => d.OrderMaster).WithMany(p => p.TOrders)
                .HasForeignKey(d => d.OrderMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_Order_M_Order_Master");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.TOrders)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_Order_M_Order_Status");

            entity.HasOne(d => d.Product).WithMany(p => p.TOrders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_Order_T_Product");

            entity.HasOne(d => d.ShippingAddress).WithMany(p => p.TOrders)
                .HasForeignKey(d => d.ShippingAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_Order_T_Shipping_Address");
        });

        modelBuilder.Entity<TProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId);

            entity.ToTable("T_Product");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.LeadLengthId).HasColumnName("lead_length_id");
            entity.Property(e => e.ProductDetailsId).HasColumnName("product_details_id");
            entity.Property(e => e.ProductQuantity).HasColumnName("product_quantity");
            entity.Property(e => e.QuantitySold).HasColumnName("quantity_sold");
            entity.Property(e => e.SizeId).HasColumnName("size_id");
            entity.Property(e => e.VariationId).HasColumnName("variation_id");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

            entity.HasOne(d => d.LeadLength).WithMany(p => p.TProducts)
                .HasForeignKey(d => d.LeadLengthId)
                .HasConstraintName("FK_T_Product_T_Lead_Length");

            entity.HasOne(d => d.ProductDetails).WithMany(p => p.TProducts)
                .HasForeignKey(d => d.ProductDetailsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_Product_T_Product_Details");

            entity.HasOne(d => d.Size).WithMany(p => p.TProducts)
                .HasForeignKey(d => d.SizeId)
                .HasConstraintName("FK_T_Product_M_Size");

            entity.HasOne(d => d.Variation).WithMany(p => p.TProducts)
                .HasForeignKey(d => d.VariationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_Product_T_Variation");

            entity.HasQueryFilter(x => x.IsDeleted == false);
        });

        modelBuilder.Entity<TProductBlobImage>(entity =>
        {
            entity.HasKey(e => e.ProductImageId);

            entity.ToTable("T_Product_Blob_Images");

            entity.Property(e => e.ProductImageId).HasColumnName("product_image_id");
            entity.Property(e => e.BlobStorageId).HasColumnName("blob_storage_id");
            entity.Property(e => e.ProductDetailsId).HasColumnName("product_details_id");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");


            entity.HasOne(d => d.ProductDetails).WithMany(p => p.TProductBlobImages)
                .HasForeignKey(d => d.ProductDetailsId)
                .OnDelete(DeleteBehavior.SetNull)
                //.OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("FK_T_Product_Blob_Images_T_Product_Details");

            entity.HasQueryFilter(x => x.IsDeleted == false);

        });

        modelBuilder.Entity<TProductDetail>(entity =>
        {
            entity.HasKey(e => e.ProductDetailsId);

            entity.ToTable("T_Product_Details");

            entity.Property(e => e.ProductDetailsId).HasColumnName("product_details_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.ProductCost)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("product_cost");
            entity.Property(e => e.ProductDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("product_description");
            entity.Property(e => e.ProductName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("product_name");
            entity.Property(e => e.ProductPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("product_price");
            entity.Property(e => e.ProductProfit)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("product_profit");
            entity.Property(e => e.ProductRevenue)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("product_revenue");
            entity.Property(e => e.QuantitySold).HasColumnName("quantity_sold");

            entity.HasOne(d => d.Category).WithMany(p => p.TProductDetails)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_Product_Details_M_Category");

            entity.HasQueryFilter(x => x.IsDeleted == false);
        });

        modelBuilder.Entity<TProductImage>(entity =>
        {
            entity.HasKey(e => e.ImageId);

            entity.ToTable("T_Product_Image");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.BlobStorageId).HasColumnName("blob_storage_id");
            entity.Property(e => e.ProductDetailsId).HasColumnName("product_details_id");
        });

        modelBuilder.Entity<TShippingAddress>(entity =>
        {
            entity.HasKey(e => e.ShippingAddressId);

            entity.ToTable("T_Shipping_Address");

            entity.Property(e => e.ShippingAddressId).HasColumnName("shipping_address_id");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("country");
            entity.Property(e => e.Postcode).HasColumnName("postcode");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.Street1)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("street_1");
            entity.Property(e => e.Street2)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("street_2");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.TShippingAddresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_Shipping_Address_T_User");
        });

        modelBuilder.Entity<TTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId);

            entity.ToTable("T_Transaction");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.OrderMasterId).HasColumnName("order_master_id");
            entity.Property(e => e.PaymentStatusId).HasColumnName("payment_status_id");
            entity.Property(e => e.TransactionAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("transaction_amount");
            entity.Property(e => e.TransactionDate)
                .HasColumnType("datetime")
                .HasColumnName("transaction_date");

            entity.HasOne(d => d.OrderMaster).WithMany(p => p.TTransactions)
                .HasForeignKey(d => d.OrderMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_Transaction_M_Order_Master");

            entity.HasOne(d => d.PaymentStatus).WithMany(p => p.TTransactions)
                .HasForeignKey(d => d.PaymentStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_Transaction_M_Payment_Status");
        });

        modelBuilder.Entity<TUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("T_User");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.AccessToken).HasColumnName("access_token");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.TUsers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_T_User_M_Role");
        });

        modelBuilder.Entity<TVariation>(entity =>
        {
            entity.HasKey(e => e.VariationId);

            entity.ToTable("T_Variation");

            entity.Property(e => e.VariationId).HasColumnName("variation_id");
            entity.Property(e => e.VariationName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("variation_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
