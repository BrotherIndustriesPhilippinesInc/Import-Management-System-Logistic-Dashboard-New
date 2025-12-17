namespace LogisticDashboard.Web.Services
{
    public interface IImageService
    {
        // Returns the relative path to the image (e.g., "/uploads/image.jpg")
        Task<string> UploadImageAsync(IFormFile file);
    }

    public class LocalImageService : IImageService
    {

        private readonly IWebHostEnvironment _environment;

        public LocalImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            // 1. Basic validation (Don't trust user input!)
            if (file == null || file.Length == 0)
            {
                return null; // Or throw an exception if you prefer strictness
            }

            // 2. Generate a unique filename to prevent overwrites
            // We use Guid to ensure uniqueness, keeping the original extension
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            // 3. Define the path: wwwroot/uploads
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

            // Ensure directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, fileName);

            // 4. Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 5. Return the URL path relative to the website root
            return $"/uploads/{fileName}";
        }
    }
}
