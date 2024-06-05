using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using facial_expression.WEB.Data;
using facial_expression.WEB.Models;
using Microsoft.Extensions.Logging;

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

        [HttpGet]
        public IActionResult Retro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Camara(string imageData)
        {
            if (!string.IsNullOrEmpty(imageData))
            {
                // Obtener el contenido de la imagen en bytes
                var base64Data = imageData.Split(',')[1];
                var bytes = Convert.FromBase64String(base64Data);

                // Guardar la imagen en el servidor
                var imagePath = $"wwwroot/images/user_image_{DateTime.Now.Ticks}.jpg";
                await System.IO.File.WriteAllBytesAsync(imagePath, bytes);

                // Realizar cualquier otro procesamiento necesario aquí, como el análisis de expresiones faciales

                // Log
                _logger.LogInformation("Imagen capturada del usuario guardada en: {ImagePath}", imagePath);

                // Devuelve una respuesta si es necesario
                return Content("Imagen capturada del usuario guardada con éxito en el servidor.");
            }

            // Si no se proporcionó ninguna imagen, devuelve un error
            return BadRequest("No se proporcionó ninguna imagen.");
        }
    }
}