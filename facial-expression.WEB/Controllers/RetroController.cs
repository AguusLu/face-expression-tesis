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

                var imagePath = $"wwwroot/images/user_image_{DateTime.Now.Ticks}.jpg";
                await System.IO.File.WriteAllBytesAsync(imagePath, imageBytes);

                // Predicción y guardado como antes

                // Retornar solo el nombre de archivo, no el camino completo
                var fileName = Path.GetFileName(imagePath);
                return Content(fileName);
            }

            return BadRequest("No se proporcionó ningún dato de imagen.");
        }
    }
}
