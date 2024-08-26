using CLDV_MVC_Birds.Models;
using CLDV_MVC_Birds.Services;
using Microsoft.AspNetCore.Mvc;

namespace CLDV_MVC_Birds.Controllers
{
    public class BirdController : Controller
    {
        private readonly BlobService _blobService;
        private readonly TableStorageService _tableStorageService;

        public BirdController(BlobService blobService, TableStorageService tableStorageService)
        {
            _blobService = blobService;
            _tableStorageService = tableStorageService;
        }
        public async Task<IActionResult> Index()
        {
            var birds = await _tableStorageService.GetAllBirdsAsync();
            return View(birds);
        }

        

        [HttpPost]
        public async Task<IActionResult> AddBird(Bird bird, IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                var imageUrl = await _blobService.UploadsAsync(stream,file.FileName);
                bird.ImageUrl = imageUrl;
            }
            if (ModelState.IsValid)
            {
                bird.PartitionKey = "BirdPartition";
                bird.RowKey =  Guid.NewGuid().ToString();
                await _tableStorageService.addBirdAsync(bird);
                return RedirectToAction("Index");
            }
            return View(bird);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBird (string partitionKey, string rowKey, Bird bird)
        {
            if (bird != null && !string.IsNullOrEmpty(bird.ImageUrl))
            {
                ////Delete the blob image
                await _blobService.DeleteBlobAsync(bird.ImageUrl);
            }

            //delete table entity
            await _tableStorageService.DeleteBirdAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public  IActionResult AddBird()
        {
            return View();
        }
    }
}
