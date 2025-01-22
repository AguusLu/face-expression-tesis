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
        public async Task<IActionResult> CaptureImage([FromForm] string imageData)
        {
            if (!string.IsNullOrEmpty(imageData))
            {
                var base64Data = imageData.Substring(imageData.IndexOf(',') + 1);
                var imageBytes = Convert.FromBase64String(base64Data);

                // Guardar temporalmente la imagen
                var tempImagePath = $"wwwroot/images/{DateTime.Now.Ticks}.jpg";
                await System.IO.File.WriteAllBytesAsync(tempImagePath, imageBytes);

                // Cargar datos de ejemplo
                MLModel.ModelInput sampleData = new MLModel.ModelInput
                {
                    ImageSource = imageBytes
                };

                // Predecir la clasificación
                var result = MLModel.Predict(sampleData);
                var predictedLabel = result.PredictedLabel;

                // Guardar la imagen con el nombre de la clasificación
                bool saved = false;
                string finalImagePath = string.Empty;

                while (!saved)
                {
                    finalImagePath = $"wwwroot/images/{predictedLabel}_{count}.jpg";
                    if (!System.IO.File.Exists(finalImagePath))
                    {
                        System.IO.File.Move(tempImagePath, finalImagePath);
                        saved = true;
                    }
                    count++;
                }

                // Guardar en la base de datos
                var expression = new Expresion
                {
                    nombreImagen = Path.GetFileName(finalImagePath),
                    clasificacion = predictedLabel
                };
                _db.Expression.Add(expression);
                _db.SaveChanges();

                return Content($"{Path.GetFileName(finalImagePath)}|{predictedLabel}");
            }

            return BadRequest("No se proporcionó ningún dato de imagen.");
        }
    }
}
