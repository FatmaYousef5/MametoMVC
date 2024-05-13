using Mameto.Data;
using Mameto.Models;
using Mameto.viewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Mameto.Controllers
{
    public class CommentController : Controller
    {
        ApplicationDbContext db;
        private readonly IWebHostEnvironment hostEnvironment;
        public CommentController(ApplicationDbContext db, IWebHostEnvironment _hostEnvironment)
        {
            this.db = db;
            hostEnvironment = _hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = db.Comments.Include(c => c.Post).Include(c => c.User).Include(c => c.CommentImgs);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(CommentviewModel model, CommentModel cmnt)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pcomment = db.Comments.AsSplitQuery().FirstOrDefault(c => c.UId == model.UId && c.PId == model.PId);
            if (model.CImg == null && model.CContent == null)
            {
                return RedirectToAction("Index");
            }
            if (model.CImg == null)
            {
                cmnt.UId = UserId;
                cmnt.PId = model.PId;
                cmnt.CContent = model.CContent;
                await db.Comments.AddAsync(cmnt);
                await db.SaveChangesAsync();
            }
            else
            {
                pcomment = new CommentModel
                {
                    UId = UserId,
                    PId = model.PId,
                    CContent = model.CContent
                };
                await db.Comments.AddAsync(pcomment);
                await db.SaveChangesAsync();
                foreach (var img in model.CImg)
                {
                    string imgsname = UploadFiles(img);
                    var cimg = new commentImgsModel
                    {
                        commentId = pcomment.CId,
                        Img = imgsname,
                    };
                    await db.commentImgs.AddAsync(cimg);
                    await db.SaveChangesAsync();

                }
            }
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Update(int? id)
        {
            var commentModel = await db.Comments.FindAsync(id);
            if (commentModel == null)
            {
                return NotFound();
            }
            return View(commentModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CommentModel commentModel)
        {

            db.Update(commentModel);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int? id)
        {
            var commentModel = await db.Comments.FirstOrDefaultAsync(m => m.CId == id);
            if (commentModel == null)
            {
                return NotFound();
            }

            return View(commentModel);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            CommentModel cmnt = await db.Comments.FirstOrDefaultAsync(p => p.CId == id);
            db.Comments.Remove(cmnt);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public string UploadFiles(IFormFile file)
        {

            string path = Path.Combine(hostEnvironment.WebRootPath, "cimg/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = "";
            if (file != null)
            {
                var extention = file.FileName.Split(".").Last();

                fileName = Guid.NewGuid().ToString() + "_" + file.Name + "." + extention;


                using (var fileStream = System.IO.File.Create("wwwroot/cimg/" + fileName))
                {
                    file.CopyTo(fileStream);
                }
            }
            return fileName;
        }

    }
}
