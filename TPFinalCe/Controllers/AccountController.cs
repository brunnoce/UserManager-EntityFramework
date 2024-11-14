using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPFinalCe.Models;
using WebAppEFCore.Models;

public class AccountController : Controller
{
    private readonly AppDBContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public AccountController(AppDBContext context, IPasswordHasher<Usuario> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Verificar si el usuario ya existe
            if (_context.Usuarios.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un usuario con ese email.");
                return View(model);
            }

            // Crear el usuario
            var usuario = new Usuario
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Email = model.Email,
                Password = _passwordHasher.HashPassword(null, model.Password),
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Registro exitoso, puedes iniciar sesión.";
            return RedirectToAction("Login", "Account");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (usuario != null)
            {
                var passwordValid = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);

                if (passwordValid == PasswordVerificationResult.Success)
                {
                    usuario.FechaIngreso = DateTime.Now;
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.Nombre),
                        new Claim(ClaimTypes.Email, usuario.Email),
                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    HttpContext.Session.SetInt32("UsuarioId", usuario.Id);

                    return RedirectToAction("Index", "Disciplinas");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Credenciales incorrectas.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Usuario no encontrado.");
            }
        }

        return View(model);
    }
    // Método para cerrar sesión
    public async Task<IActionResult> Logout()
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

        if (usuarioId.HasValue)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId.Value);

            if (usuario != null)
            {

                usuario.FechaLogout = DateTime.Now;
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
        }
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();

        return RedirectToAction("Login", "Account");
    }
}