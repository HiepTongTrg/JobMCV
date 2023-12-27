using System.ComponentModel.DataAnnotations;

namespace QLViecLam.Models
{
    public class NhaTuyenDung
    {
        [Key]
        public int ID { get; set; }
        public int TaiKhoanID { get; set; }
        public string TenCongTy { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public TaiKhoan? TaiKhoan { get; set; }
        public ICollection<CongViec>? congViecs { get; set; }
    }
}
