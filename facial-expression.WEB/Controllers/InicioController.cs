using facial_expression.WEB.Data;
using facial_expression.WEB.Models;
using Facial_expression_WEB;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace facial_expression.WEB.Controllers;


    public class InicioController : Controller
    {

        private readonly ILogger<InicioController> _logger;
        private static int count = 0;
        private readonly ApplicationDbContext _db;

        public InicioController(ILogger<InicioController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

    public IActionResult Inicio()
    {
        return View();
    }


    public IActionResult Privacy()
        {
            return View();
        }
    }

