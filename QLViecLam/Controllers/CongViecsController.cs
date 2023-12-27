using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLViecLam.Models;

namespace QLViecLam.Controllers
{
    public class CongViecsController : BaseController
    {
        private readonly QLViecLamDBContext _context;

        public CongViecsController(QLViecLamDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = _context.TaiKhoans.Include(m=>m.nhaTuyenDung).Include(m=>m.ungVien).FirstOrDefault(m=>m.Username==CurrentUser);
            var congviec = _context.CongViecs.Include(m => m.NhaTuyenDung).ToList();
            ViewData["user"]=user;
            return View(congviec);
        }
        public IActionResult PostJob()
        {
            var user = _context.TaiKhoans.Include(m => m.nhaTuyenDung).Include(m => m.ungVien).FirstOrDefault(m => m.Username == CurrentUser);
            ViewData["user"] = user;
            return View();
        }
       
        public async Task<IActionResult> PostJobCheck(string TenCV,string Address,double Salary,string LoaiCV,string MoTa, string type = "Normal")
        {
            var LoginUser = _context.TaiKhoans.Include(m => m.nhaTuyenDung).FirstOrDefault(m => m.Username == CurrentUser);
            CongViec cv = new CongViec();
            cv.NgayDang = DateTime.Now;
            cv.NhaTuyenDungID = LoginUser.nhaTuyenDung.ID;
            cv.TenCongViec = TenCV;
            cv.DiaChi = Address;
            cv.MoTa = MoTa;
            cv.Luong = Salary;
            cv.LoaiCV = LoaiCV;
            _context.Add(cv);
            await _context.SaveChangesAsync();
            if(type == "ajax")
            {
                return Json(cv.ID);
            }    
            return RedirectToAction("Index", "CongViecs");
        }
        public async Task<IActionResult> Search(string keyword,string place,string time)
        {
            if (keyword==null&&place==null&&time==null)
            {
                return RedirectToAction("Index");
            }    
            if (keyword!=null&&place==null&&time==null)
            {
                var congviecs = _context.CongViecs.Include(m=>m.NhaTuyenDung).Where(m => m.TenCongViec.Contains(keyword)||m.NhaTuyenDung.TenCongTy.Contains(keyword)).ToList();
                return View("Index",congviecs);
            }
            if (keyword == null && place != null && time == null)
            {
                var congviecs = _context.CongViecs.Include(m => m.NhaTuyenDung).Where(m => m.DiaChi == place).ToList();
                return View("Index", congviecs);
            }
            if (keyword == null && place == null && time != null)
            {
                var congviecs = _context.CongViecs.Include(m => m.NhaTuyenDung).Where(m => m.LoaiCV == time).ToList();
                return View("Index", congviecs);
            }
            if (keyword != null && place != null && time == null)
            {
                var congviecs = _context.CongViecs.Include(m => m.NhaTuyenDung).Where(m => (m.TenCongViec.Contains(keyword) || m.NhaTuyenDung.TenCongTy.Contains(keyword)) && m.DiaChi==place).ToList();
                return View("Index", congviecs);
            }
            if (keyword == null && place != null && time != null)
            {
                var congviecs = _context.CongViecs.Include(m => m.NhaTuyenDung).Where(m => m.DiaChi == place&&m.LoaiCV==time).ToList();
                return View("Index", congviecs);
            }
            if (keyword != null && place == null && time != null)
            {
                var congviecs = _context.CongViecs.Include(m => m.NhaTuyenDung).Where(m => (m.TenCongViec.Contains(keyword) || m.NhaTuyenDung.TenCongTy.Contains(keyword)) && m.LoaiCV==time).ToList();
                return View("Index", congviecs);
            }
            if (keyword != null && place != null && time != null)
            {
                var congviecs = _context.CongViecs.Include(m => m.NhaTuyenDung).Where(m => (m.TenCongViec.Contains(keyword) || m.NhaTuyenDung.TenCongTy.Contains(keyword)) && m.LoaiCV == time&&m.DiaChi==place).ToList();
                return View("Index", congviecs);
            }    
            return View("Index");
        }
        public async Task<IActionResult> Detail(int Id)
        {
            var user = _context.TaiKhoans.Include(m => m.nhaTuyenDung).Include(m => m.ungVien).FirstOrDefault(m => m.Username == CurrentUser);
            var congviecs = _context.CongViecs.Include(m => m.NhaTuyenDung).FirstOrDefault(m => m.ID == Id);
            ViewData["user"] = user;
            return View(congviecs);
        }
    }
}
