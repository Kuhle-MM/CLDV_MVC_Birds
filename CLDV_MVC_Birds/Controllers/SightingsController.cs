using CLDV_MVC_Birds.Models;
using CLDV_MVC_Birds.Services;
using Microsoft.AspNetCore.Mvc;

namespace CLDV_MVC_Sightings.Controllers
{
    public class SightingsController : Controller
    {
		private readonly QueueService _queueService;
		private readonly TableStorageService _tableStorageService;

		public SightingsController(QueueService QueueService, TableStorageService tableStorageService)
		{
			_queueService = QueueService;
			_tableStorageService = tableStorageService;
		}
		public async Task<IActionResult> Index()
		{
			var sightings = await _tableStorageService.GetAllSightingAsync();
			return View(sightings);
		}



		public async Task<IActionResult> Register()
		{
			var birders = await _tableStorageService.GetAllBirdersAsync();
			var birds = await _tableStorageService.GetAllBirdsAsync();

			if (birders == null || birders.Count == 0)
			{
				ModelState.AddModelError("", "No Birders found. Please add birders first");
				return View();
			}
			if (birds == null || birds.Count == 0)
			{
				ModelState.AddModelError("", "No Birds found. Please add birds first");
				return View();
			}
			ViewData["Birders"] = birders;
			ViewData["Birds"] = birds;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(Sighting sighting)
		{
			if(ModelState.IsValid)
			{
				sighting.Sighting_Date = DateTime.SpecifyKind(sighting.Sighting_Date, DateTimeKind.Utc);
				sighting.PartitionKey = "SightingPartition";
				sighting.RowKey = Guid.NewGuid().ToString();
				await _tableStorageService.AddSightingAsync(sighting);

				string message = $"New sighting by birder {sighting.Birder_ID} if bird Bird {sighting.Bird_ID} at {sighting.Sighting_Location} on {sighting.Sighting_Date}";
				await _queueService.SendMessage(message);
				return RedirectToAction("Index");
			}

			var birders = await _tableStorageService.GetAllBirdersAsync();
			var birds = await _tableStorageService.GetAllBirdsAsync();
			ViewData["Birders"] = birders;
			ViewData["Birds"] = birds ;
			return View(sighting);

		}

		
	}
}
