using facial_expression.WEB.Data;
using facial_expression.WEB.Models;
using Facial_expression_WEB;
using Microsoft.AspNetCore.Mvc;

namespace facial_expression.WEB.Controllers
{
    public class CamaraController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CamaraController> _logger;
        private static int count = 0;
        public CamaraController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Camara()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Camara(Expresion model)
        {
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                // Aquí puedes realizar las operaciones necesarias con la imagen, como guardarla en disco, procesarla, etc.
                // model.ImageFile contiene la imagen enviada

                // Ejemplo: Guardar la imagen en una ubicación específica
                var imagePath = $"wwwroot/images/{model.ImageFile.Name}.jpg";
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(stream);
                }

                //Load sample data
                var imageBytes = System.IO.File.ReadAllBytes(@$"wwwroot/images/{model.ImageFile.Name}.jpg");
                MLModel.ModelInput sampleData = new MLModel.ModelInput()
                {
                    ImageSource = imageBytes,
                };

                //Load model and predict output
                var result = MLModel.Predict(sampleData);

                model.clasificacion = result.PredictedLabel;

                //ViewBag.Respuesta = result.PredictedLabel;

                Console.WriteLine("Respuesta:" + result.PredictedLabel);

                bool saved = false;
                while (!saved)
                {
                    if (System.IO.File.Exists($"wwwroot/images/{result.PredictedLabel + count}.jpg"))
                    {
                        count++;
                    }
                    else
                    {
                        System.IO.File.Move($"wwwroot/images/{model.ImageFile.Name}.jpg", $"wwwroot/images/{result.PredictedLabel + count}.jpg");
                        model.nombreImagen = $"{result.PredictedLabel + count}.jpg";
                        count++;
                        saved = true;
                    }

                }

                _db.Expression.Add(model);
                _db.SaveChanges();

                // Devuelve la predicción obtenida
                return Content(result.PredictedLabel);
            }

            // Si no se envió una imagen válida, puedes redirigir a una vista de error o mostrar un mensaje de validación en el formulario.
            ModelState.AddModelError("ImageFile", "Por favor, seleccione una imagen.");

            // Vuelve a mostrar el formulario de carga de imágenes con el mensaje de error
            return null;
        }
    }
}
