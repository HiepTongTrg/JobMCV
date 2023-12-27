using System.ComponentModel.DataAnnotations;

namespace QLViecLam.Models
{
    public class XinViec
    {
        [Key]
        public int ID { get; set; }
        public int UngVienID { get; set; }
        public int CongViecID { get; set; }
        public string CV { get; set; }
        public int TrangThai { get; set; }
        public string MoTa { get; set; }
        public UngVien? UngVien { get; set; }
        public CongViec? CongViec { get; set; }
        
    }
}
