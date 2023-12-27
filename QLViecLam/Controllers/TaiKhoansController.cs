using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLViecLam.Models;

namespace QLViecLam.Controllers
{
    public class TaiKhoansController : BaseController
    {
        private readonly QLViecLamDBContext _context;

        public TaiKhoansController(QLViecLamDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string User,string Pass)
        {
            var user = _context.TaiKhoans.FirstOrDefault(m => m.Username == User);
            if(user != null)
            {
                SHA256 hashMethod = SHA256.Create();
                if(Md5.Md5.VerifyHash(hashMethod, Pass,user.Password))
                {
                    CurrentUser = user.Username;
                    return RedirectToAction("Index", "CongViecs");
                }    
            }    
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(string Username, string Pass, string Address, string Phone, string Name, string Email, IFormFile File, int Account)
        {
            if(Account == 1)
            {
                TaiKhoan tk = new TaiKhoan();
                UngVien uv = new UngVien();
                tk.Username = Username;
                SHA256 hashMethod = SHA256.Create();
                tk.Password = Md5.Md5.GetHash(hashMethod, Pass);
                _context.Add(tk);
                await _context.SaveChangesAsync();
                uv.TenUngVien = Name;
                uv.DiaChi = Address;
                uv.Email = Email;
                uv.DienThoai = Phone;
                uv.TaiKhoanID = tk.ID;
                _context.Add(uv);
                await _context.SaveChangesAsync();
                uv.Avatar = UploadUV(uv.ID, File);
                _context.Update(uv);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            if (Account == 0)
            {
                TaiKhoan tk = new TaiKhoan();
                NhaTuyenDung ntd = new NhaTuyenDung();
                tk.Username = Username;
                SHA256 hashMethod = SHA256.Create();
                tk.Password = Md5.Md5.GetHash(hashMethod, Pass);
                _context.Add(tk);
                await _context.SaveChangesAsync();
                ntd.TenCongTy = Name;
                ntd.DiaChi = Address;
                ntd.Email = Email;
                ntd.DienThoai = Phone;
                ntd.TaiKhoanID = tk.ID;
                _context.Add(ntd);
                await _context.SaveChangesAsync();
                ntd.Avatar = UploadNTD(ntd.ID, File);
                _context.Update(ntd);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View();
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
                return string.Format("{0}/{1}",id, file.FileName);
            }
            return String.Empty;
        }
        [NonAction]
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
                return string.Format("/{0}/{1}",id, file.FileName);
            }
            return String.Empty;
        }
        public async Task<IActionResult> Logout()
        {
            CurrentUser = "";
            return RedirectToAction("Index","CongViecs");
        }
    }
}
