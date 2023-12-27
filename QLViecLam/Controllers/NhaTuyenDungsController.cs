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
    public class NhaTuyenDungsController : BaseController
    {
        private readonly QLViecLamDBContext _context;

        public NhaTuyenDungsController(QLViecLamDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Admin(int Id)
        {
            var CongViec = _context.CongViecs.Include(m => m.NhaTuyenDung).Where(m => m.NhaTuyenDungID == Id).ToList();
            var User = _context.TaiKhoans.Include(m => m.nhaTuyenDung).FirstOrDefault(m => m.Username == CurrentUser);
            ViewData["user"] = User;
            return View(CongViec);
        }
        [HttpGet]
        public IActionResult Information(int Id)
        {
            var NTD = _context.NhaTuyenDungs.FirstOrDefault(m => m.ID == Id);
            return View(NTD);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Information([Bind("ID","TaiKhoanID","TenCongTy","DiaChi","DienThoai","Email","Avatar")] NhaTuyenDung model,IFormFile? file)
        {
            if(file == null)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Admin", "NhaTuyenDungs", new { Id = model.ID });
            }
            if(file != null)
            {
                DeleteAvatar(model.ID);
                model.Avatar = UploadNTD(model.ID, file);
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Admin", "NhaTuyenDungs", new { Id = model.ID });
            }
            return View();
        }
        public async Task<IActionResult> TimKiem(string Search)
        {
            if (Search == null)
            {
                var user = _context.TaiKhoans.Include(m => m.nhaTuyenDung).FirstOrDefault(m => m.Username == CurrentUser);
                ViewData["user"] = user;
                return RedirectToAction("Admin", "NhaTuyenDungs", new { Id = user.nhaTuyenDung.ID });
            }
            var CongViec = _context.CongViecs.Include(m => m.NhaTuyenDung).Where(m => m.TenCongViec.Contains(Search) || m.NhaTuyenDung.TenCongTy.Contains(Search)).ToList();
            var user1 = _context.TaiKhoans.Include(m => m.nhaTuyenDung).FirstOrDefault(m => m.Username == CurrentUser);
            ViewData["user"] = user1;
            return View("Admin", CongViec);
        }
        public async Task<IActionResult> ViewJob(int Id)
        {
            var XinViec = _context.XinViecs.Include(m => m.UngVien).Where(m => m.CongViecID == Id && m.TrangThai == 0).ToList();
            return View(XinViec);
        }
        public async Task<IActionResult> Accept(int Id)
        {
            var XinViec = _context.XinViecs.FirstOrDefault(m => m.ID == Id);
            XinViec.TrangThai = 1;
            _context.Update(XinViec);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewJob","NhaTuyenDungs",new {Id = XinViec.CongViecID});
        }
        public async Task<IActionResult> Reject(int Id)
        {
            var XinViec = _context.XinViecs.FirstOrDefault(m => m.ID == Id);
            XinViec.TrangThai = 2;
            _context.Update(XinViec);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewJob", "NhaTuyenDungs", new { Id = XinViec.CongViecID });
        }
        [NonAction]
        public string UploadNTD(int id, IFormFile file)
        {
            if (file != null)
            {
                var folderPath = Path.GetFullPath("./wwwroot/AnhNTD");
                folderPath = string.Format(@"{0}\{1}", folderPath, id);
                Directory.CreateDirectory(folderPath);
                var filePath = string.Format(@"{0}\{1}", folderPath, file.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                return string.Format("{0}/{1}", id, file.FileName);
            }
            return String.Empty;
        }
        private bool DeleteAvatar(int Id)
        {
            try
            {
                string path = Path.GetFullPath("./wwwroot/AnhNTD/" + Id);
                Directory.Delete(path, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
