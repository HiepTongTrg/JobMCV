using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace QLViecLam.Models
{
    public class UngVien
    {
        [Key]
        public int ID { get; set; }
        public int TaiKhoanID { get; set; }
        public string TenUngVien { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public TaiKhoan? TaiKhoan { get; set; }
        public ICollection<XinViec>? xinViecs { get; set; }
    }
}
