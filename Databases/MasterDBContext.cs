using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CeramicShopMasterApi.Databases
{
    public partial class MasterDBContext : DbContext
    {
        public MasterDBContext()
        {
        }

        public MasterDBContext(DbContextOptions<MasterDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-592USLC\\SQLEXPRESS;Database=cernamic_shop;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("accounts");

                entity.HasIndex(e => e.Username, "IX_accounts_username")
                    .IsUnique();

                entity.HasIndex(e => e.Uuid, "UQ__accounts__7F4279316C17559F")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .HasColumnName("full_name");

                entity.Property(e => e.IsEnable)
                    .IsRequired()
                    .HasColumnName("is_enable")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.Uuid)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("uuid")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsEnable)
                    .HasColumnName("is_enable")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasIndex(e => e.Uuid, "UQ__orders__7F427931C54DF2D1")
                    .IsUnique();

                entity.HasIndex(e => e.CreatedAt, "idx_created_at");

                entity.HasIndex(e => e.PhoneNumber, "idx_phone");

                entity.HasIndex(e => new { e.State, e.IsEnable }, "idx_state_isenable");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("created_by")
                    .IsFixedLength();

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("full_name");

                entity.Property(e => e.IsEnable)
                    .IsRequired()
                    .HasColumnName("is_enable")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("phone_number");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasDefaultValueSql("((1))")
                    .HasComment("1 chờ thanh toán, 2 đã thanh toán");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("total_amount");

                entity.Property(e => e.Uuid)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("uuid")
                    .IsFixedLength();

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Orders)
                    .HasPrincipalKey(p => p.Uuid)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__orders__created___2739D489");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("order_detail");

                entity.HasIndex(e => e.CreatedAt, "idx_created_at");

                entity.HasIndex(e => e.Uuid, "idx_uq_uuid")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsEnable)
                    .IsRequired()
                    .HasColumnName("is_enable")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.OrderUuid)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("order_uuid")
                    .IsFixedLength();

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.SlugProduct)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("slug_product");

                entity.Property(e => e.Uuid)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("uuid")
                    .IsFixedLength();

                entity.HasOne(d => d.OrderUu)
                    .WithMany(p => p.OrderDetails)
                    .HasPrincipalKey(p => p.Uuid)
                    .HasForeignKey(d => d.OrderUuid)
                    .HasConstraintName("fk_order_detail_order");

                entity.HasOne(d => d.SlugProductNavigation)
                    .WithMany(p => p.OrderDetails)
                    .HasPrincipalKey(p => p.Slug)
                    .HasForeignKey(d => d.SlugProduct)
                    .HasConstraintName("fk_order_detail_products");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.HasIndex(e => e.Slug, "IX_Products_Slug")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoriesId).HasColumnName("categories_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsEnable)
                    .IsRequired()
                    .HasColumnName("is_enable")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(12, 2)")
                    .HasColumnName("price");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Slug)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("slug");

                entity.HasOne(d => d.Categories)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoriesId)
                    .HasConstraintName("FK_Products_Categories");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
