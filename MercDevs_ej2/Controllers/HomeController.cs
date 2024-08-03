using MercDevs_ej2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

//Para cerrar Sesión
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

//FIN cerrar sesión

namespace MercDevs_ej2.Controllers
{
    [Authorize] //Para que no autorice ingresar a ningun controlador sin estar Autenticado
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MercyDeveloperContext _context;

        public HomeController(ILogger<HomeController> logger, MercyDeveloperContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Filtra las fichas técnicas con estado 1 (activo)
            var datosFichaTecnicaActivos = await _context.Datosfichatecnicas
                .Where(d => d.Estado == 1)  // Asegúrate de que el valor 1 es correcto para el estado activo
                .Include(d => d.RecepcionEquipo)  // Incluye la relación si es necesario para la vista
                .ThenInclude(r => r.IdClienteNavigation)  // Incluye el cliente si lo necesitas en la vista
                .ToListAsync();

            return View(datosFichaTecnicaActivos);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Para Salir sesión
        public async Task<IActionResult>  LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Ingresar", "Login");
        }
        //Fin Salir Sesión
    }
}
