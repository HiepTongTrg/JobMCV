using System.ComponentModel.DataAnnotations;

namespace QLViecLam.Models
{
    public class TaiKhoan
    {
        [Key]
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public NhaTuyenDung? nhaTuyenDung { get; set; }
        public UngVien? ungVien { get; set; }
    }
}
