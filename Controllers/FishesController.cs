using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WebApplicationFish.Models;
using WebApplicationFish.Services;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationFish.Controllers
{
    public class FishesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public FishesController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var fishes = context.Fishes.OrderByDescending(p => p.Id).ToList();
            return View(fishes);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(FishDto fishDto)
        {
            if (fishDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }
            if (!ModelState.IsValid)
            {
                return View(fishDto);
            }

            // Save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(fishDto.ImageFile.FileName);
            string imageFullPath = Path.Combine(environment.WebRootPath, "Fish_images", newFileName);

            using (var stream = System.IO.File.Create(imageFullPath))
            {
                fishDto.ImageFile.CopyTo(stream);
            }

            // Save the new catch to database
            Fish fish = new Fish()
            {
                Name = fishDto.Name,
                Method = fishDto.Method,
                Type = fishDto.Type,
                Place = fishDto.Place,
                Weather = fishDto.Weather,
                Weight = fishDto.Weight,
                ImageFileName = newFileName,
                CaughtAt = DateTime.Now,
            };

            context.Fishes.Add(fish);
            context.SaveChanges();

            return RedirectToAction("Index", "Fishes");
        }
        public IActionResult Edit(int id)
        {
            var fish = context.Fishes.Find(id);

            if (fish == null) 
            {
                return RedirectToAction("Index", "Fishes");
            }

            // Create FishDto from fish
            var fishDto = new FishDto()
            {
                Name = fish.Name,
                Method = fish.Method,
                Type = fish.Type,
                Place = fish.Place,
                Weather = fish.Weather,
                Weight= fish.Weight,



            };

            ViewData["FishId"] = fish.Id;
            ViewData["ImageFilename"] = fish.ImageFileName;
            ViewData["CaughtAt"] = fish.CaughtAt.ToString("MM/dd/yyyy");

            return View(fishDto);
        }
        [HttpPost]
        public IActionResult Edit(int id, FishDto fishDto)
        {
            var fish = context.Fishes.Find(id);

            if (fish == null)
            {
                return RedirectToAction("Index", "Fishes");
            }

            if (!ModelState.IsValid)
            {
                ViewData["FishId"] = fish.Id;
                ViewData["ImageFilename"] = fish.ImageFileName;
                ViewData["CaughtAt"] = fish.CaughtAt.ToString("MM/dd/yyyy");

                return View(fishDto);


            }
            // Update the image file if we have a new image file
            string newFileName = fish.ImageFileName; // Keep the old file name by default
            if (fishDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(fishDto.ImageFile.FileName);
                string imageFullPath = Path.Combine(environment.WebRootPath, "Fish_images", newFileName);
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    fishDto.ImageFile.CopyTo(stream);
                }

                // Delete the old image if a new one was uploaded
                string oldImageFullPath = Path.Combine(environment.WebRootPath, "Fish_images", fish.ImageFileName);
                System.IO.File.Delete(oldImageFullPath);
            }

            // update the catch in the database
            fish.Name = fishDto.Name;
            fish.Method = fishDto.Method;
            fish.Type = fishDto.Type;
            fish.Place = fishDto.Place;
            fish.Weather = fishDto.Weather;
            fish.Weight = fishDto.Weight;
            fish.ImageFileName = newFileName;

            context.SaveChanges();

            return RedirectToAction("Index", "Fishes");

        }

        public IActionResult Delete(int id)
        {
            var fish = context.Fishes.Find(id);
            if (fish == null)
            {
                return RedirectToAction("Index", "Fishes");
            }

            string imageFullPath = environment.WebRootPath + "/Fish_images/" + fish.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Fishes.Remove(fish);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Fishes");
        }
        
    }
}
