using System.ComponentModel.DataAnnotations;

namespace QLViecLam.Models
{
    public class CongViec
    {
        [Key]
        public int ID { get; set; }
        public int NhaTuyenDungID { get; set; }
        public DateTime NgayDang { get; set; }
        public string TenCongViec { get; set; }
        public string DiaChi { get; set; }
        public double Luong { get; set; }
        public string MoTa { get; set; }
        public string LoaiCV { get; set; }
        public NhaTuyenDung? NhaTuyenDung { get; set; }
        public ICollection<XinViec>? xinViecs { get; set; }
    }
}
