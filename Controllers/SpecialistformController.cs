using Mameto.Data;
using Mameto.Models;
using Mameto.viewModels;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Mameto.Migrations;
using Mameto.DTO;

namespace Mameto.Controllers
{
    public class SpecialistformController : Controller
    {
        ApplicationDbContext db;
        private readonly IWebHostEnvironment hostEnvironment;
        public SpecialistformController(ApplicationDbContext db, IWebHostEnvironment _hostEnvironment)
        {
            this.db = db;
            hostEnvironment = _hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(SpecialistUpdateDTO model)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            {
                currentUser.Phone = model.Phone;
                currentUser.Country = model.Country;
                currentUser.City = model.City;
                currentUser.Address = model.Address;
                currentUser.Bio = model.Bio;
            }
            await db.SaveChangesAsync();

            foreach(var photo in model.Img)
            {
                string imgsname = UploadFiles(photo);
                var specialistPhoto = new SpecialistcertifcateModel
                {
                    UId = currentUser.Id,
                    Img = imgsname,
                };
                await db.certificates.AddAsync(specialistPhoto);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public string UploadFiles(IFormFile file)
        {

            string path = Path.Combine(hostEnvironment.WebRootPath, "specialistCertificate/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = "";
            if (file != null)
            {
                var extention = file.FileName.Split(".").Last();

                fileName = Guid.NewGuid().ToString() + "_" + file.Name + "." + extention;


                using (var fileStream = System.IO.File.Create("wwwroot/specialistCertificate/" + fileName))
                {
                    file.CopyTo(fileStream);
                }
            }
            return fileName;
        }
    }
}
