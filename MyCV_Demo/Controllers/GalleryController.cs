using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCV_Demo.Data;
using MyCV_Demo.Models;

namespace MyCV_Demo.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _db;
        public GalleryController(IWebHostEnvironment env, AppDbContext db)
        {
            _env = env;
            _db = db;
        }
        // GET: GalleryController
        [HttpGet]
        public ActionResult Index()
        {
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsPath))
            {
                return View(Enumerable.Empty<GalleryResultModel>());
            }

            var exts = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            var files = Directory.EnumerateFiles(uploadsPath, "*.*", SearchOption.AllDirectories)
                                 .Where(p => exts.Contains(Path.GetExtension(p)));
            var items = files.Select(fullPath =>
            {
                var relative = Path.GetRelativePath(_env.WebRootPath, fullPath).Replace("\\", "/");
                return new GalleryResultModel
                {
                    Title = Path.GetFileNameWithoutExtension(fullPath),
                    Description = "Image uploaded on " + System.IO.File.GetCreationTime(fullPath).ToString("yyyy-MM-dd HH:mm:ss"),
                    ImagePath = relative
                };
            }).ToList();
            return View(items);
        }

        [HttpGet]
        public ActionResult GetAllImages()
        {
            //TODO: Implement logic to retrieve and return all images
            return View();
        }
        [HttpDelete]
        public ActionResult DeleteImage(int id)
        {
            //TODO: Implement delete logic here
            return View();
        }

        [HttpGet]
        public ActionResult UploadImage()
        {
            return View("Upload");
        }
        [HttpPost]
        public ActionResult UploadImage(GalleryInputModel vm)
        {
            if(vm.File == null || vm.File.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a valid image file.");
                return View(vm);
            }

            var allowedTypes = new[] {"image/jpeg", "image/png", "image/gif", "image/webp" };
            if (!allowedTypes.Contains(vm.File.ContentType))
            {
                ModelState.AddModelError("File", "Only JPEG, PNG, GIF, or WEBP images are allowed.");
                return View(vm);
            }
            var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads");
            var fileName = vm.Title + Path.GetExtension(vm.File.FileName);
            var filePath = Path.Combine(uploadsRoot, fileName);

            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                vm.File.CopyTo(stream);
            }

            var newGalleryItem = new GalleryResultModel
            {
                Title = vm.Title,
                Description = vm.Description,
                ImagePath = filePath
            };
            _db.GalleryItems.Add(newGalleryItem);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
