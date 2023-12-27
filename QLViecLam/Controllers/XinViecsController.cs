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
    public class XinViecsController : BaseController
    {
        private readonly QLViecLamDBContext _context;

        public XinViecsController(QLViecLamDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int Id)
        {
            if(!IsLogin)
            {
                return RedirectToAction("Login", "TaiKhoans");
            }
            var user = _context.TaiKhoans.Include(m => m.nhaTuyenDung).Include(m => m.ungVien).FirstOrDefault(m => m.Username == CurrentUser);
            var XinViec = _context.XinViecs.Where(m=>m.UngVienID==user.ungVien.ID).ToList();
            var CongViec = _context.CongViecs.Include(m => m.NhaTuyenDung).FirstOrDefault(m => m.ID == Id);
            foreach(XinViec xinViecs in XinViec)
            {
                if (xinViecs.CongViecID == CongViec.ID)
                {
                    return RedirectToAction("Index", "CongViecs");
                } 
                    
            }    
            var cv = _context.CongViecs.Include(m => m.NhaTuyenDung).FirstOrDefault(m => m.ID == Id);
            ViewData["user"] = user;
            return View(cv);
        }
        public IActionResult Information(int Id)
        {
            var UV = _context.UngViens.FirstOrDefault(m => m.ID == Id);
            return View(UV);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Information([Bind("ID", "TaiKhoanID", "TenUngVien", "DiaChi", "DienThoai", "Email", "Avatar")] UngVien model, IFormFile? file)
        {
            if (file == null)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("ListJob", "XinViecs", new { Id = model.ID });
            }
            if (file != null)
            {
                DeleteAvatar(model.ID);
                model.Avatar = UploadUV(model.ID, file);
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("ListJob", "XinViecs", new { Id = model.ID });
            }
            return View();
        }
        public async Task<IActionResult> ListJob(int Id)
        {
            var XinViec = _context.XinViecs.Include(m => m.CongViec).Include(m=>m.CongViec.NhaTuyenDung).Where(m => m.UngVienID == Id).ToList();
            var user = _context.TaiKhoans.Include(m => m.ungVien).FirstOrDefault(m => m.Username == CurrentUser);
            ViewData["user"] = user;
            return View(XinViec);
        }
        public async Task<IActionResult> DeleteJob(int Id)
        {
            var XinViec = _context.XinViecs.FirstOrDefault(m => m.ID == Id);
            _context.Remove(XinViec);
            await _context.SaveChangesAsync();
            var User = _context.TaiKhoans.Include(m=>m.ungVien).FirstOrDefault(m => m.Username == CurrentUser);
            return RedirectToAction("ListJob", "XinViecs", new {Id=User.ungVien.ID});
        }
        public async Task<IActionResult> TimKiem(string Search)
        {
            if(Search == null)
            {
                var user = _context.TaiKhoans.Include(m=>m.ungVien).FirstOrDefault(m => m.Username == CurrentUser);
                ViewData["user"] = user;
                return RedirectToAction("ListJob", "XinViecs",new {Id = user.ungVien.ID});
            }
            var XinViec = _context.XinViecs.Include(m => m.CongViec).Include(m => m.CongViec.NhaTuyenDung).Where(m => m.CongViec.TenCongViec.Contains(Search) || m.CongViec.NhaTuyenDung.TenCongTy.Contains(Search)).ToList();
            var user1 = _context.TaiKhoans.Include(m => m.ungVien).FirstOrDefault(m => m.Username == CurrentUser);
            ViewData["user"] = user1;
            return View("ListJob",XinViec);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(int IdCV,IFormFile CV,string Mota)
        {
            var LoginUser = _context.TaiKhoans.Include(m=>m.ungVien).FirstOrDefault(m => m.Username == CurrentUser);       
            XinViec xinViec = new XinViec();
            xinViec.UngVienID = LoginUser.ungVien.ID;
            xinViec.CongViecID = IdCV;
            xinViec.MoTa = Mota;
            xinViec.TrangThai = 0;
            _context.Add(xinViec);
            await _context.SaveChangesAsync();
            xinViec.CV = UploadCV(xinViec.ID, CV);
            _context.Update(xinViec);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "CongViecs");
        }
        [NonAction]
        public string UploadCV(int id, IFormFile file)
        {
            if (file != null)
            {
                var folderPath = Path.GetFullPath("./wwwroot/CV");
                folderPath = string.Format(@"{0}\{1}", folderPath, id);
                Directory.CreateDirectory(folderPath);
                var filePath = string.Format(@"{0}\{1}", folderPath, file.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                return string.Format("/{0}/{1}",id, file.FileName);
            }
            return String.Empty;
        }
        public string UploadUV(int id, IFormFile file)
        {
            if (file != null)
            {
                var folderPath = Path.GetFullPath("./wwwroot/AnhUV");
                folderPath = string.Format(@"{0}\{1}", folderPath, id);
                Directory.CreateDirectory(folderPath);
                var filePath = string.Format(@"{0}\{1}", folderPath, file.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                return string.Format("/{0}/{1}", id, file.FileName);
            }
            return String.Empty;
        }
        private bool DeleteAvatar(int Id)
        {
            try
            {
                string path = Path.GetFullPath("./wwwroot/AnhUV/" + Id);
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
