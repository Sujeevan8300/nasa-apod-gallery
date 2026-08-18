using NasaApodGallery.DTOs;

namespace NasaApodGallery.Services
{
    public interface INasaApodService
    {
        // This method will call NASA API and return a list of pictures
        Task<List<ApodDto>>GetApodRangeAsync(DateTime startDate, DateTime endDate);
    }
}