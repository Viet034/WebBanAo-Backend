using Microsoft.EntityFrameworkCore;
using WebBanAoo.Models;
using System.Data;
using System.Globalization;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Data
{
    public class ApplicationDbContext : DbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Cart_ProductDetail> Cart_ProductDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Customer_Voucher> Customer_Vouchers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Employee_Role> Employee_Roles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetail { get; set; }
        public DbSet<ProductDetail_Sale> ProductDetail_Sales { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // Anh xa Brand voi Bang Brands trong MySQL
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brands"); //ten bang trong mysql
                entity.HasKey(b => b.Id); //khoa chinh
                entity.Property(b => b.Id).ValueGeneratedOnAdd();
                entity.Property(b => b.Code).IsRequired().HasMaxLength(100);
                entity.HasIndex(b => b.Code).IsUnique();
                entity.Property(b => b.BrandName).IsRequired().HasMaxLength(100);

                entity.HasMany(p => p.Products)
                .WithOne(b => b.Brand)
                .HasForeignKey(b => b.BrandId);
            });

            // Anh xa Category voi Bang Categories trong MySQL
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(cat => cat.Id);
                entity.Property(cat => cat.Id).ValueGeneratedOnAdd();
                entity.Property(cat => cat.Code).IsRequired().HasMaxLength(100);
                entity.HasIndex(cat => cat.Code).IsUnique();
                entity.Property(cat => cat.CategoryName).IsRequired().HasMaxLength(100);
                entity.Property(cat => cat.Description).IsRequired().HasMaxLength(100);
                entity.Property(cat => cat.CreateDate).IsRequired().HasMaxLength(100);
                entity.Property(cat => cat.UpdateDate).HasMaxLength(100);
                entity.Property(cat => cat.CreatedBy).IsRequired().HasMaxLength(100);
                
                entity.Property(cat => cat.UpdateBy);
                entity.Property(cat => cat.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (CategoryStatus)value);

                entity.HasMany(p => p.Products)
                .WithOne(cat => cat.Category)
                .HasForeignKey(cat => cat.CategoryId);
            });
            // Anh xa Product voi Bang Products trong MySQL
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.Code).IsRequired().HasMaxLength(100);
                entity.HasIndex(p => p.Code).IsUnique();
                entity.Property(p => p.ProductName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).HasMaxLength(100);
                entity.Property(p => p.CreateDate).IsRequired().HasMaxLength(100);
                entity.Property(p => p.UpdateDate).HasMaxLength(100);
                entity.Property(p => p.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(p => p.UpdateBy).HasMaxLength(100);
                entity.Property(p => p.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (ProductStatus)value);  // Chuyển số nguyên thành enum khi đọc dữ liệu

                entity.HasOne(cat => cat.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(cat => cat.CategoryId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.Brand)
                .WithMany(p => p.Products)
                .HasForeignKey(b => b.BrandId).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(prd => prd.ProductDetails)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.Cascade);
            });
            // Anh xa Color voi Bang Colors trong MySQL
            modelBuilder.Entity<Color>(entity =>
            {
                entity.ToTable("Colors");
                entity.HasKey(co => co.Id);
                entity.Property(co => co.Id).ValueGeneratedOnAdd();
                entity.Property(co => co.Code).IsRequired().HasMaxLength(100);
                entity.HasIndex(co => co.Code).IsUnique();
                entity.Property(co => co.ColorName).IsRequired().HasMaxLength(100);
                entity.Property(co => co.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (ColorStatus)value);

                entity.Property(co => co.CreateDate).IsRequired().HasMaxLength(100);
                entity.Property(co => co.UpdateDate).HasMaxLength(100);
                entity.Property(co => co.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(co => co.UpdateBy).HasMaxLength(100);

                entity.HasMany(prd => prd.ProductDetails)
                .WithOne(co => co.Color)
                .HasForeignKey(co => co.ColorId).OnDelete(DeleteBehavior.Cascade);

            });
            // Anh xa Size voi Bang Sizes trong MySQL
            modelBuilder.Entity<Size>(entity =>
            {
                entity.ToTable("Sizes");
                entity.HasKey(si => si.Id);
                entity.Property(si => si.Id).ValueGeneratedOnAdd();
                entity.Property(si => si.Code).IsRequired().HasMaxLength(100);
                entity.HasIndex(si => si.Code).IsUnique();
                entity.Property(si => si.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (SizeStatus)value);

                entity.Property(si => si.SizeCode).IsRequired().HasMaxLength(100)
                .HasConversion(sizeCode => (int)sizeCode,  // Lưu số nguyên vào database
                value => (SizeCode)value);

                entity.Property(si => si.CreateDate).IsRequired().HasMaxLength(100);
                entity.Property(si => si.UpdateDate).HasMaxLength(100);
                entity.Property(si => si.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(si => si.UpdateBy).HasMaxLength(100);

                entity.HasMany(prd => prd.ProductDetails)
                .WithOne(si => si.Size)
                .HasForeignKey(si => si.SizeId).OnDelete(DeleteBehavior.Cascade);

            });
            //Anh xa ProductImage voi bang ProductImages trong mySql
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.ToTable("ProductImages");
                entity.HasKey(pi => pi.Id);
                entity.Property(pi => pi.Id).ValueGeneratedOnAdd();
                entity.Property(pi => pi.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (ProductImageStatus)value);

                entity.Property(pi => pi.Url);

                entity.HasOne(prd => prd.ProductDetail)
                .WithMany(pi => pi.ProductImages)
                .HasForeignKey(prd => prd.ProductDetailId).OnDelete(DeleteBehavior.Cascade);

            });
            //Anh xa ProductDetail voi bang ProductDetails trong mySql
            modelBuilder.Entity<ProductDetail>(entity =>
            {
                entity.ToTable("ProductDetails");
                entity.HasKey(prd => prd.Id);
                entity.Property(prd => prd.Id).ValueGeneratedOnAdd();
                entity.Property(prd => prd.Name).IsRequired().HasMaxLength(100);
                entity.Property(prd => prd.Code).IsRequired().HasMaxLength(100);
                entity.HasIndex(prd => prd.Code).IsUnique();
                entity.Property(prd => prd.Price).IsRequired().HasMaxLength(100);
                entity.Property(prd => prd.Quantity).IsRequired().HasMaxLength(100);
                entity.Property(prd => prd.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (ProductDetailStatus)value);

                entity.Property(prd => prd.CreateDate).IsRequired().HasMaxLength(100);
                entity.Property(prd => prd.UpdateDate).HasMaxLength(100);
                entity.Property(prd => prd.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(prd => prd.UpdateBy).HasMaxLength(100);

                entity.HasOne(p => p.Product)
                .WithMany(prd => prd.ProductDetails)
                .HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(co => co.Color)
                .WithMany(prd => prd.ProductDetails)
                .HasForeignKey(co => co.ColorId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(si => si.Size)
                .WithMany(prd => prd.ProductDetails)
                .HasForeignKey(si => si.SizeId).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(prds => prds.ProductDetail_Sales)
                .WithOne(prd => prd.ProductDetail)
                .HasForeignKey(prd => prd.ProductDetailId).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(pi => pi.ProductImages)
                .WithOne(prd => prd.ProductDetail)
                .HasForeignKey(prd => prd.ProductDetailId).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(cprd => cprd.Cart_ProductDetails)
                .WithOne(prd => prd.ProductDetail)
                .HasForeignKey(prd => prd.ProductDetailId).OnDelete(DeleteBehavior.Cascade);
            });

            // Anh xa Sale voi bang Sales trong mySql
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.ToTable("Sales");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).ValueGeneratedOnAdd();
                entity.Property(s => s.Code).IsRequired().HasMaxLength(100);
                entity.HasIndex(s => s.Code).IsUnique();
                entity.Property(s => s.SaleName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (SaleStatus)value);

                entity.Property(s => s.StartDate).IsRequired().HasMaxLength(100);
                entity.Property(s => s.EndDate).IsRequired().HasMaxLength(100);
                entity.Property(s => s.CreateDate).IsRequired().HasMaxLength(100);
                entity.Property(s => s.UpdateDate).HasMaxLength(100);
                entity.Property(s => s.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(s => s.UpdateBy).HasMaxLength(100);

                entity.HasMany(prds => prds.ProductDetail_Sales)
                .WithOne(s => s.Sale)
                .HasForeignKey(s => s.SaleId).OnDelete(DeleteBehavior.Cascade);
            });

            // Anh xa ProductDetail_Sale voi bang ProductDetail_Sales trong mySql
            modelBuilder.Entity<ProductDetail_Sale>(entity =>
            {
                entity.ToTable("ProductDetail_Sales");
                entity.HasKey(prds => prds.Id);
                entity.Property(prds => prds.Id).ValueGeneratedOnAdd();
                entity.Property(prds => prds.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (ProductDetailSaleStatus)value);

                entity.HasOne(prd => prd.ProductDetail)
                .WithMany(prds => prds.ProductDetail_Sales)
                .HasForeignKey(prd => prd.ProductDetailId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.Sale)
                .WithMany(prds => prds.ProductDetail_Sales)
                .HasForeignKey(s => s.SaleId).OnDelete(DeleteBehavior.Cascade);
            });

            // Anh xa Cart_ProductDetail voi bang Cart_ProductDetails trong mySql
            modelBuilder.Entity<Cart_ProductDetail>(entity =>
            {
                entity.ToTable("Cart_ProductDetails");
                entity.HasKey(cprd => cprd.Id);
                entity.Property(cprd => cprd.Id).ValueGeneratedOnAdd();
                entity.Property(cprd => cprd.Quantity).IsRequired().HasMaxLength(100);
                entity.Property(cprd => cprd.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (CartProductDetailStatus)value);


                entity.HasOne(prd => prd.ProductDetail)
                .WithMany(cprd => cprd.Cart_ProductDetails)
                .HasForeignKey(prd => prd.ProductDetailId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Cart)
                .WithMany(cprd => cprd.Cart_ProductDetails)
                .HasForeignKey(c => c.CartId).OnDelete(DeleteBehavior.Cascade);
            });

            // Anh xa Cart voi bang Carts trong mySql
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Carts");
                entity.HasKey(c => c.CartId);
                
                entity.Property(c => c.SessionId).IsRequired().HasMaxLength(100);

                entity.HasOne(cus => cus.Customer)
                .WithOne(c => c.Cart)
                .HasForeignKey<Customer>(cus => cus.Id).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(cprd => cprd.Cart_ProductDetails)
                .WithOne(c => c.Cart)
                .HasForeignKey(prd => prd.CartId).OnDelete(DeleteBehavior.Cascade);
            });

            // Anh xa Customer voi bang Customers trong mySql
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(cus => cus.Id);
                entity.Property(cus => cus.Id).ValueGeneratedOnAdd();
                entity.Property(cus => cus.Code).IsRequired().HasMaxLength(100);
                entity.HasIndex(cus => cus.Code).IsUnique();
                entity.Property(cus => cus.FullName).IsRequired().HasMaxLength(100);
                entity.Property(cus => cus.Dob).IsRequired().HasMaxLength(100);
                entity.Property(cus => cus.Gender).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (Gender)value);

                entity.Property(cus => cus.Email).IsRequired().HasMaxLength(100);
                entity.Property(cus => cus.Password).IsRequired().HasMaxLength(100);
                entity.Property(cus => cus.Phone).IsRequired().HasMaxLength(100);
                entity.Property(cus => cus.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (CustomerStatus)value);

                entity.Property(cus => cus.Address).IsRequired().HasMaxLength(100);
                entity.Property(cus => cus.City).IsRequired().HasMaxLength(100);
                entity.Property(cus => cus.Image);
                entity.Property(cus => cus.CreateDate).IsRequired().HasMaxLength(100);
                entity.Property(cus => cus.UpdateDate).HasMaxLength(100);
                entity.Property(cus => cus.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(cus => cus.UpdateBy).HasMaxLength(100);
                entity.Property(cus => cus.RefreshToken).HasMaxLength(100);
                entity.Property(cus => cus.RefreshTokenExpiryTime);

                entity.HasOne(c => c.Cart)
                .WithOne(cus => cus.Customer)
                .HasForeignKey<Cart>(cus => cus.CustomerId).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(cv => cv.Customer_Vouchers)
                .WithOne(cus => cus.Customer)
                .HasForeignKey(cus => cus.CustomerId).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.Orders)
                .WithOne(cus => cus.Customer)
                .HasForeignKey(cus => cus.CustomerId).OnDelete(DeleteBehavior.Cascade);
            });

            // Anh xa Voucher voi bang Vouchers trong mySql
            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.ToTable("Vouchers");
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Id).ValueGeneratedOnAdd();

                entity.Property(v => v.Code).IsRequired().HasMaxLength(100);
                entity.HasIndex(v => v.Code).IsUnique();

                entity.Property(v => v.Name).IsRequired().HasMaxLength(100);
                entity.Property(v => v.Description).HasMaxLength(100);
                entity.Property(v => v.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (VoucherStatus)value);

                entity.Property(v => v.StartDate).IsRequired().HasMaxLength(100);
                entity.Property(v => v.EndDate).IsRequired().HasMaxLength(100);
                entity.Property(v => v.CreateDate).IsRequired().HasMaxLength(100);
                entity.Property(v => v.UpdateDate).HasMaxLength(100);
                entity.Property(v => v.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(v => v.UpdateBy).HasMaxLength(100);
                entity.Property(v => v.Quantity).IsRequired().HasMaxLength(100);
                entity.Property(v => v.DiscountValue).IsRequired().HasMaxLength(100);
                entity.Property(v => v.MinimumOrderValue).IsRequired().HasMaxLength(100);
                entity.Property(v => v.MaxDiscount).IsRequired().HasMaxLength(100);

                entity.HasMany(cv => cv.Customer_Vouchers)
                .WithOne(v => v.Voucher)
                .HasForeignKey(v => v.VoucherId).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.Orders)
                .WithOne(v => v.Voucher)
                .HasForeignKey(v => v.VoucherId).OnDelete(DeleteBehavior.Cascade);
            });

            // Anh xa Customer_Voucher voi bang Customer_Vouchers trong mySql
            modelBuilder.Entity<Customer_Voucher>(entity =>
            {
                entity.ToTable("Customer_Vouchers");
                entity.HasKey(cv => cv.Id);
                entity.Property(cv => cv.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (CustomerVoucherStatus)value);

                entity.HasOne(cus => cus.Customer)
                .WithMany(cv => cv.Customer_Vouchers)
                .HasForeignKey(cus => cus.CustomerId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(v => v.Voucher)
                .WithMany(cv => cv.Customer_Vouchers)
                .HasForeignKey(v => v.VoucherId).OnDelete(DeleteBehavior.Cascade);
            });

            // Anh xa Employee voi bang Employees trong mySql
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees");
                entity.HasKey(emp => emp.Id);
                entity.Property(emp => emp.Id).ValueGeneratedOnAdd();
                entity.Property(emp => emp.Code).IsRequired().HasMaxLength(100);
                entity.HasIndex(emp => emp.Code).IsUnique();
                entity.Property(emp => emp.FullName).IsRequired().HasMaxLength(100);
                entity.Property(emp => emp.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (EmployeeStatus)value);

                entity.Property(emp => emp.Dob).IsRequired().HasMaxLength(100);
                entity.Property(emp => emp.Gender).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (Gender)value);

                entity.Property(emp => emp.Image).IsRequired().HasMaxLength(100);
                entity.Property(emp => emp.Email).IsRequired().HasMaxLength(100);
                entity.Property(emp => emp.Password).IsRequired().HasMaxLength(100);
                entity.Property(emp => emp.Phone).IsRequired().HasMaxLength(100);
                entity.Property(emp => emp.Address).IsRequired().HasMaxLength(100);
                entity.Property(emp => emp.City).IsRequired().HasMaxLength(100);
                entity.Property(emp => emp.StartDate).IsRequired().HasMaxLength(100);
                entity.Property(emp => emp.EndDate);
                entity.Property(emp => emp.CreateDate).IsRequired().HasMaxLength(100);
                entity.Property(emp => emp.UpdateDate).HasMaxLength(100);
                entity.Property(emp => emp.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(emp => emp.UpdateBy).HasMaxLength(100);
                entity.Property(emp => emp.RefreshToken).HasMaxLength(100);
                entity.Property(emp => emp.RefreshTokenExpiryTime);

                entity.HasMany(o => o.Orders)
                .WithOne(emp => emp.Employee)
                .HasForeignKey(emp => emp.EmployeeId).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(empr => empr.Employee_Roles)
                .WithOne(emp => emp.Employee)
                .HasForeignKey(emp => emp.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            });

            // Anh xa Order voi bang Orders trong mySql
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id).ValueGeneratedOnAdd();
                entity.Property(o => o.Code).IsRequired().HasMaxLength(100);
                entity.HasIndex(o => o.Code).IsUnique();
                entity.Property(o => o.InitialTotalAmount).IsRequired().HasMaxLength(100);
                entity.Property(o => o.TotalAmount).IsRequired().HasMaxLength(100);
                entity.Property(o => o.Note).HasMaxLength(100);
                entity.Property(o => o.OrderDate).IsRequired().HasMaxLength(100);
                entity.Property(o => o.CreateDate).IsRequired().HasMaxLength(100);
                entity.Property(o => o.UpdateDate).HasMaxLength(100);
                entity.Property(o => o.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(o => o.UpdateBy).HasMaxLength(100);
                entity.Property(o => o.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (OrderStatus)value);


                entity.HasOne(cus => cus.Customer)
                .WithMany(o => o.Orders)
                .HasForeignKey(cus => cus.CustomerId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(v => v.Voucher)
                .WithMany(o => o.Orders)
                .HasForeignKey(v => v.VoucherId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(emp => emp.Employee)
                .WithMany(o => o.Orders)
                .HasForeignKey(emp => emp.EmployeeId).OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.OrderDetails)
                .WithOne(o => o.Order)
                .HasForeignKey(o => o.OrderId).OnDelete(DeleteBehavior.Cascade);
            });

            // Anh xa OrderDetail voi bang OrderDetails trong mySql
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetails");
                entity.HasKey(ord => ord.Id);
                entity.Property(ord => ord.Id).ValueGeneratedOnAdd();

                entity.Property(ord => ord.Code).IsRequired().HasMaxLength(100);
                entity.HasIndex(ord => ord.Code).IsUnique();

                entity.Property(ord => ord.Quantity).IsRequired().HasMaxLength(100);
                entity.Property(ord => ord.UnitPrice).IsRequired().HasMaxLength(100);
                entity.Property(ord => ord.Discount).IsRequired().HasMaxLength(100);
                entity.Property(ord => ord.TotalAmount).IsRequired().HasMaxLength(100);
                entity.Property(ord => ord.Note).HasMaxLength(100);
                entity.Property(ord => ord.CreateDate).IsRequired().HasMaxLength(100);
                entity.Property(ord => ord.UpdateDate).HasMaxLength(100);
                entity.Property(ord => ord.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(ord => ord.UpdateBy).HasMaxLength(100);
                entity.Property(ord => ord.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (OrderDetailStatus)value);

                entity.HasOne(o => o.Order)
                .WithMany(ord => ord.OrderDetails)
                .HasForeignKey(o => o.OrderId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(prd => prd.ProductDetail)
                .WithMany(ord => ord.OrderDetails)
                .HasForeignKey(prd => prd.ProductDetailId).OnDelete(DeleteBehavior.Cascade);
            });

            // Anh xa Role voi bang Roles trong mySql
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedOnAdd();

                entity.Property(r => r.Code).IsRequired().HasMaxLength(100);
                entity.HasIndex(r => r.Code).IsUnique();

                entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
                entity.Property(r => r.Description).HasMaxLength(100);
                entity.Property(r => r.CreateDate).IsRequired().HasMaxLength(100);
                entity.Property(r => r.UpdateDate).HasMaxLength(100);
                entity.Property(r => r.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(r => r.UpdateBy).HasMaxLength(100);
                entity.Property(r => r.Status).IsRequired().HasMaxLength(100)
                .HasConversion(status => (int)status,  // Lưu số nguyên vào database
                value => (RoleStatus)value);

                entity.HasMany(empr => empr.Employee_Roles)
                .WithOne(r => r.Role)
                .HasForeignKey(r => r.RoleId).OnDelete(DeleteBehavior.Cascade);
            });

            // Anh xa Employee_Role voi bang Employee_Roles trong mySql
            modelBuilder.Entity<Employee_Role>(entity =>
            {
                entity.ToTable("Employee_Roles");

                entity.HasOne(emp => emp.Employee)
                .WithMany(empr => empr.Employee_Roles)
                .HasForeignKey(emp => emp.EmployeeId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Role)
                .WithMany(empr => empr.Employee_Roles)
                .HasForeignKey(r => r.RoleId).OnDelete(DeleteBehavior.Cascade);
            });
        }


    }
}
