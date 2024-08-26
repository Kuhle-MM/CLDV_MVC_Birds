using CLDV_MVC_Birds.Models;
using CLDV_MVC_Birds.Services;
using Microsoft.AspNetCore.Mvc;

namespace CLDV_MVC_Birders.Controllers
{
    public class BirderController : Controller
    {
        
        private readonly TableStorageService _tableStorageService;

        public BirderController( TableStorageService tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }
        public async Task<IActionResult> Index()
        {
            var birders = await _tableStorageService.GetAllBirdersAsync();
            return View(birders);
        }

        [HttpPost]
        public async Task<IActionResult> AddBirder(Birder birder)
        {
                birder.PartitionKey = "BirderPartition";
                birder.RowKey = Guid.NewGuid().ToString();
                await _tableStorageService.addBirderAsync(birder);
                return RedirectToAction("Index");
            
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBirder(string partitionKey, string rowKey, Birder birder)
        {
            //delete table entity
            await _tableStorageService.DeleteBirderAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AddBirder()
        {
            return View();
        }
    }
}
