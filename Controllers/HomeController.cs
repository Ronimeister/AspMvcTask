using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using EpamAspMvc.Models;
using Microsoft.Ajax.Utilities;

namespace EpamAspMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly PicturesContext _context = new PicturesContext();

        public ActionResult Index()
        {
            List<Picture> pictures = _context.Pictures.Take(6).ToList();
            ImageViewModel model = new ImageViewModel
            {
                PathList = CreatePathList(pictures)
            };

            return View(model);
        }

        public ActionResult About(string filter = null, int page = 1, int pageSize = 2)
        {
            ViewBag.Message = "Gallery page";
            ViewBag.filter = filter;
            List<Picture> pictures = _context.Pictures
                .Where(x => filter == null || (x.PictureDescription.Contains(filter)))
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            ImageViewModel model = new ImageViewModel
            {
                PathList = CreatePathList(pictures),
                CurrentPage = page,
                PageSize = pageSize,
                TotalRecords = pictures.Count,
                TotalPages = CalculatePageCount(pageSize)
            };

            return View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Create()
        {
            CreateViewModel vm = new CreateViewModel();
            vm.PictureInner = new Picture();

            return View(vm);
        }

        [HttpPost]
        public ActionResult Create(Picture picture, IEnumerable<HttpPostedFileBase> files)
        {
            CreateViewModel vm = new CreateViewModel();

            if (!ModelState.IsValid)
            {
                vm.PictureInner = picture;

                return View(vm);
            }
                
            if (files.Count() == 0 || files.FirstOrDefault() == null)
            {
                ViewBag.error = "Please choose a file";
                vm.PictureInner = picture;

                return View(vm);
            }

            vm.PictureInner = new Picture();
            foreach (var file in files)
            {
                if (file.ContentLength == 0) continue;

                vm.PictureInner.PictureDescription = picture.PictureDescription;
                var fileName = Guid.NewGuid().ToString();
                var extension = System.IO.Path.GetExtension(file.FileName).ToLower();

                using (var img = Image.FromStream(file.InputStream))
                {
                    vm.PicturePath = String.Format("/images/{0}{1}", fileName, extension);
                    SaveToFolder(img, fileName, extension, new Size(300, 300), vm.PicturePath);
                }

                vm.PictureInner.Name = fileName;
                vm.PictureInner.Type = extension;
                _context.Pictures.Add(vm.PictureInner);
                _context.SaveChanges();
            }

            return RedirectPermanent("/home");
        }

        public Size NewImageSize(Size imageSize, Size newSize)
        {
            Size finalSize;
            double tempval;
            if (imageSize.Height > newSize.Height || imageSize.Width > newSize.Width)
            {
                if (imageSize.Height > imageSize.Width)
                    tempval = newSize.Height / (imageSize.Height * 1.0);
                else
                    tempval = newSize.Width / (imageSize.Width * 1.0);

                finalSize = new Size((int)(tempval * imageSize.Width), (int)(tempval * imageSize.Height));
            }
            else
                finalSize = imageSize;

            return finalSize;
        }

        private List<string> CreatePathList(List<Picture> pictures)
        {
            List<string> result = new List<string>();
            foreach (var i in pictures)
            {
                result.Add("../images/" + i.Name + i.Type);
            }

            return result;
        }

        private void SaveToFolder(Image img, string fileName, string extension, Size newSize, string pathToSave)
        {
            // Get new resolution
            Size imgSize = NewImageSize(img.Size, newSize);
            using (Image newImg = new Bitmap(img, imgSize.Width, imgSize.Height))
            {
                newImg.Save(Server.MapPath(pathToSave), img.RawFormat);
            }
        }

        private int CalculatePageCount(int pageSize)
        {
            int picturesCount = _context.Pictures.Count();
            int result = picturesCount / pageSize;

            if (picturesCount % 2 != 0)
            {
                result++;

            }

            return result;
        }
    }
}


/*

_context.Pictures.AddRange(new List<Picture>()
{
    new Picture { PictureDescription = "First", PictureBytes = System.IO.File.ReadAllBytes(@"D:\WEB\EpamAspMvc\EpamAspMvc\images\img_5terre_wide.jpg")},
    new Picture { PictureDescription = "Second", PictureBytes = System.IO.File.ReadAllBytes(@"D:\WEB\EpamAspMvc\EpamAspMvc\images\img_lights_wide.jpg")},
    new Picture { PictureDescription = "Third", PictureBytes = System.IO.File.ReadAllBytes(@"D:\WEB\EpamAspMvc\EpamAspMvc\images\img_mountains_wide.jpg")},
    new Picture { PictureDescription = "Fourth", PictureBytes = System.IO.File.ReadAllBytes(@"D:\WEB\EpamAspMvc\EpamAspMvc\images\img_nature_wide.jpg")},
    new Picture { PictureDescription = "Fifth", PictureBytes = System.IO.File.ReadAllBytes(@"D:\WEB\EpamAspMvc\EpamAspMvc\images\img_snow_wide.jpg")},
    new Picture { PictureDescription = "Six", PictureBytes = System.IO.File.ReadAllBytes(@"D:\WEB\EpamAspMvc\EpamAspMvc\images\img_woods_wide.jpg")}
});

_context.SaveChanges();
*/
