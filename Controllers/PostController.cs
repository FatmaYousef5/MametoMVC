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

namespace Mameto.Controllers
{
    public class PostController : Controller
    {
        ApplicationDbContext db;
        private readonly IWebHostEnvironment hostEnvironment;
        public PostController(ApplicationDbContext db, IWebHostEnvironment _hostEnvironment)
        {
            this.db = db;
            hostEnvironment = _hostEnvironment;
        }
        public async Task <IActionResult> Index()
        {
            var applicationDbContext = db.Posts.Include(p => p.User).Include(p => p.Comment).ThenInclude(c => c.CommentImgs).Include(p=>p.PostImgs).OrderByDescending(p => p.PId).ToList();
            return View(applicationDbContext);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Posts == null)
            {
                return NotFound();
            }

            var postModel = await db.Posts
                .Include(p => p.User).Include(p => p.Comment).ThenInclude(c => c.CommentImgs).Include(p => p.PostImgs)
                .FirstOrDefaultAsync(m => m.PId == id);
            if (postModel == null)
            {
                return NotFound();
            }

            return View(postModel);
        }


        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PostvievModel model,PostModel pst)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(model.Img == null && model.PContent== null)
            {
                return RedirectToAction("Index");
            }

            if (model.Img == null)
            {
                    pst.UId = userId;
                    pst.PContent = model.PContent;
                await db.Posts.AddAsync(pst);
                await db.SaveChangesAsync();
            }

            else
            {
                PostModel post = new PostModel
                {
                    UId = userId,
                    PContent = model.PContent,
                };
                await db.Posts.AddAsync(post);
                await db.SaveChangesAsync();
                foreach (var photo in model.Img)
                {
                    string imgsname = UploadFiles(photo);
                    var postPhoto = new postImgsModel
                    {
                        PostId = post.PId,
                        Img = imgsname,
                    };
                    await db.postImgs.AddAsync(postPhoto);
                    await db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Update(int? id)
        {
            var postModel = await db.Posts.FindAsync(id);
            if (postModel == null)
            {
                return NotFound();
            }
            return View(postModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(PostModel postModel)
        {
           
                     db.Update(postModel);
                    await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        private bool PostModelExists(int id)
        {
            return (db.Posts?.Any(e => e.PId == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var postModel = await db.Posts.Include(p => p.Comment).FirstOrDefaultAsync(m => m.PId == id);
            if (postModel == null)
            {
                return NotFound();
            }

            return View(postModel);
        }
        [HttpPost]
        public async Task<IActionResult> Delete (int id)
        {
            
            var pst = await db.Posts.FindAsync(id);
            if (pst != null)
            {
                db.Posts.Remove(pst);
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }








        public IActionResult Comment(CommentModel cmnt)
        {
            return View(cmnt.PId);
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
                    string imgsname = UploadFilescomment(img);
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


        public async Task<IActionResult> commentUpdate(int? id)
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
        public async Task<IActionResult> commentUpdate(CommentModel commentModel)
        {

            db.Update(commentModel);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> commentDelete(int? id)
        {
            var commentModel = await db.Comments.FirstOrDefaultAsync(m => m.CId == id);
            if (commentModel == null)
            {
                return NotFound();
            }

            return View(commentModel);
        }
        [HttpPost]
        public async Task<IActionResult> commentDelete(int id)
        {

            CommentModel cmnt = await db.Comments.FirstOrDefaultAsync(p => p.CId == id);
            db.Comments.Remove(cmnt);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> commentDetails(int? id)
        {
            if (id == null || db.Posts == null)
            {
                return NotFound();
            }

            var postModel = await db.Posts
                .Include(p => p.User).Include(p => p.Comment).ThenInclude(c => c.CommentImgs).Include(p => p.PostImgs)
                .FirstOrDefaultAsync(m => m.PId == id);
            if (postModel == null)
            {
                return NotFound();
            }

            return View(postModel);
        }

        //public IActionResult Download(int id)
        //{
        //    postImgsModel pst = db.postImgs.FirstOrDefault(i=>i.PostId == id);
        //    return File("Img/" + pst.Img, "Img/jpg", pst.PostId + ".jpg");
        //}

        public string UploadFiles(IFormFile file)
        {

            string path = Path.Combine(hostEnvironment.WebRootPath, "Img/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = "";
            if (file != null)
            {
                var extention = file.FileName.Split(".").Last();

                fileName = Guid.NewGuid().ToString() + "_" + file.Name + "." + extention;


                using (var fileStream = System.IO.File.Create("wwwroot/Img/" + fileName))
                {
                    file.CopyTo(fileStream);
                }
            }
            return fileName;
        }

        public string UploadFilescomment(IFormFile file)
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

