using CURDOperationWithImageUploadCore5_Demo.Data;
using CURDOperationWithImageUploadCore5_Demo.Models;
using CURDOperationWithImageUploadCore5_Demo.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CURDOperationWithImageUploadCore5_Demo.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public InventoryController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            db = context;
            webHostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.Inventory.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await db.Inventory
                .FirstOrDefaultAsync(m => m.Id == id);

            var speakerViewModel = new InventoryViewModel()
            {
                Id = inventory.Id,
                ProductName = inventory.ProductName,
                Price = inventory.Price,
                Category = inventory.Category,
                Quantity = inventory.Quantity,
                Desc = inventory.Desc,
                ExistingImage = inventory.ProfilePicture
            };

            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                Inventories inventory= new Inventories               
                {
                    ProductName = model.ProductName,
                    Price = model.Price,
                    Category = model.Category,
                    Quantity = model.Quantity,
                    Desc = model.Desc,
                    ProfilePicture = uniqueFileName
                };

                db.Add(inventory);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory= await db.Inventory.FindAsync(id);
            var speakerViewModel = new InventoryViewModel()
            {
                Id = inventory.Id,
                ProductName = inventory.ProductName,
                Price = inventory.Price,
                Category = inventory.Category,
                Quantity = inventory.Quantity,
                Desc = inventory.Desc,
                ExistingImage = inventory.ProfilePicture
            };

            if (inventory == null)
            {
                return NotFound();
            }
            return View(speakerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InventoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var inventory = await db.Inventory.FindAsync(model.Id);
                inventory.ProductName = model.ProductName;
                inventory.Price = model.Price;
                inventory.Category = model.Category;
                inventory.Quantity = model.Quantity;
                inventory.Desc = model.Desc;

                if (model.SpeakerPicture != null)
                {
                    if (model.ExistingImage != null)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath, "Uploads", model.ExistingImage);
                        System.IO.File.Delete(filePath);
                    }

                    inventory.ProfilePicture = ProcessUploadedFile(model);
                }
                db.Update(inventory);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();

            //if (id != model.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(speaker);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!SpeakerExists(speaker.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(speaker);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await db.Inventory
                .FirstOrDefaultAsync(m => m.Id == id);

            var speakerViewModel = new InventoryViewModel()
            {
                Id = inventory.Id,
                ProductName = inventory.ProductName,
                Price = inventory.Price,
                Category = inventory.Category,
                Quantity = inventory.Quantity,
                Desc = inventory.Desc,
                ExistingImage = inventory.ProfilePicture
            };
            if (inventory == null)
            {
                return NotFound();
            }

            return View(speakerViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var speaker = await db.Inventory.FindAsync(id);
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", speaker.ProfilePicture);
            db.Inventory.Remove(speaker);
            if (await db.SaveChangesAsync() > 0)
            {
                if (System.IO.File.Exists(CurrentImage))
                {
                    System.IO.File.Delete(CurrentImage);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SpeakerExists(int id)
        {
            return db.Inventory.Any(e => e.Id == id);
        }

        private string ProcessUploadedFile(InventoryViewModel model)
        {
            string uniqueFileName = null;

            if (model.SpeakerPicture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.SpeakerPicture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.SpeakerPicture.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
