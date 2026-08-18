using Microsoft.AspNetCore.Mvc;
using NasaApodGallery.Services;

namespace NasaApodGallery.Controllers
{
    public class HomeController : Controller
    {
        private readonly INasaApodService _nasaService;
        private readonly IApodRepository _apodRepository;

        public HomeController(INasaApodService nasaService, IApodRepository apodRepository)
        {
            _nasaService = nasaService;
            _apodRepository = apodRepository;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Decide the date range (last 7 days including today)
            DateTime endDate = DateTime.UtcNow.Date;
            DateTime startDate = endDate.AddDays(-6);

            // 2. Call NASA API and get the list of pictures
            var apodListFromNasa = await _nasaService.GetApodRangeAsync(startDate, endDate);

            // 3. Save each picture into the database (only if it does not already exist)
            foreach (var item in apodListFromNasa)
            {
                await _apodRepository.InsertIfNotExistsAsync(item);
            }

            // 4. Read all pictures from the database
            var allPictures = await _apodRepository.GetAllAsync();

            // 5. Send the list to the View (gallery page)
            return View(allPictures);
        }
    }
}