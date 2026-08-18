namespace NasaApodGallery.DTOs
{
    public class ApodDto
    {
        // The date of the picture (example: "2024-05-15")
        public string Date { get; set; }

        // Title of the astronomy picture
        public string Title { get; set; }

        // Long description / explanation of the picture
        public string Explanation { get; set; }

        // URL of the image (or video)
        public string Url { get; set; }

        // High resolution URL (sometimes available)
        public string HdUrl { get; set; }

        // "image" or "video"
        public string MediaType { get; set; }

        // Version of the NASA service (usually "v1")
        public string ServiceVersion { get; set; }

        // Optional - name of the copyright holder
        public string Copyright { get; set; }
    }
}