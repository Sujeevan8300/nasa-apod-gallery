using NasaApodGallery.DTOs;
using NasaApodGallery.Models;

namespace NasaApodGallery.Services
{
    public interface IApodRepository
    {
        Task InsertIfNotExistsAsync(ApodDto apodDto);
        Task<List<Apod>> GetAllAsync();
    }
}