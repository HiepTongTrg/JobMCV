using Microsoft.EntityFrameworkCore;

namespace QLViecLam.Models
{
    public class QLViecLamDBContext : DbContext
    {
        public QLViecLamDBContext(DbContextOptions<QLViecLamDBContext> options) : base(options) { }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<NhaTuyenDung> NhaTuyenDungs { get; set;}
        public DbSet<UngVien> UngViens { get; set; }
        public DbSet<CongViec> CongViecs { get; set; }
        public DbSet<XinViec> XinViecs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaiKhoan>().ToTable("TaiKhoan");
            modelBuilder.Entity<NhaTuyenDung>().ToTable("NhaTuyenDung");
            modelBuilder.Entity<UngVien>().ToTable("UngVien");
            modelBuilder.Entity<CongViec>().ToTable("CongViec");
            modelBuilder.Entity<XinViec>().ToTable("XinViec");
        }
    }
}
