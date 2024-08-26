using facial_expression.WEB.Data;
using facial_expression.WEB.Models;
using Facial_expression_WEB;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace facial_expression.WEB.Controllers
{
    public class RetroController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<RetroController> _logger;
        private static int count = 0;

        public RetroController(ApplicationDbContext db, ILogger<RetroController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Retro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CaptureImage(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                // Save the image to a temporary path
                var imagePath = $"wwwroot/images/user_image_{DateTime.Now.Ticks}.jpg";
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Load the image for prediction
                var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
                MLModel.ModelInput sampleData = new MLModel.ModelInput()
                {
                    ImageSource = imageBytes,
                };

                // Predict emotion
                var result = MLModel.Predict(sampleData);

                // Save image with predicted label
                bool saved = false;
                while (!saved)
                {
                    var newPath = $"wwwroot/images/{result.PredictedLabel + count}.jpg";
                    if (System.IO.File.Exists(newPath))
                    {
                        count++;
                    }
                    else
                    {
                        System.IO.File.Move(imagePath, newPath);
                        count++;
                        saved = true;
                    }
                }

                // Save prediction and image info to database
                var model = new Expresion
                {
                    ImageFile = imageFile,
                    clasificacion = result.PredictedLabel,
                    nombreImagen = $"{result.PredictedLabel + count}.jpg"
                };

                _db.Expression.Add(model);
                await _db.SaveChangesAsync();

                // Return the prediction result
                return Content(result.PredictedLabel);
            }

            return BadRequest("No image file provided.");
        }
    }
}
