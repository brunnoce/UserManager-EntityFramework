using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPFinalCe.Models;

namespace TPFinalCe.Controllers
{
    public class SociosController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _env;

        public SociosController(AppDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Socios
        public async Task<IActionResult> Index(string? busquedaNombre, int? disciplinaId, int? estadoId)
        {
            var socios = _context.Socios
                .Include(s => s.Disciplina)
                .Include(s => s.Estado)
                .Include(s => s.Cuotas)
                .Include(s => s.Beneficios)
                .AsQueryable(); 

            if (!string.IsNullOrEmpty(busquedaNombre))
            {
                socios = socios.Where(s => s.Nombre.Contains(busquedaNombre));
            }
            if (disciplinaId.HasValue)
            {
                socios = socios.Where(s => s.DisciplinaId == disciplinaId);
            }
            if (estadoId.HasValue)
            {
                socios = socios.Where(s => s.EstadoId == estadoId);
            }

            foreach (var socio in socios)
            {
                socio.ActualizarEstadoYBeneficios();
                _context.Update(socio);
            }

            await _context.SaveChangesAsync(); 

            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "Id", "Nombre", disciplinaId);
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", estadoId);

            return View(await socios.ToListAsync());
        }
        public async Task<IActionResult> PublicIndex()
        {
            return View(await _context.Socios.Include(s => s.Disciplina).Include(s => s.Estado).Include(s => s.Beneficios).ToListAsync());
        }

        // GET: Socios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socio = await _context.Socios
                .Include(s => s.Disciplina)
                .Include(s => s.Estado)
                .Include(s => s.Cuotas) 
                .Include(s => s.Beneficios)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (socio == null)
            {
                return NotFound();
            }

            socio.ActualizarEstadoYBeneficios();

            return View(socio);
        }

        // GET: Socios/Create
        public IActionResult Create()
        {
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "Id", "Nombre");
            return View();
        }

        // POST: Socios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,DNI,FechaNacimiento,Foto,DisciplinaId")] Socio socio)
        {
            if (ModelState.IsValid)
            {
                socio.EstadoId = 1;
                socio.FechaAlta = DateTime.Now.Date;

                var archivos = HttpContext.Request.Form.Files;
                if (archivos?.Count > 0)
                {
                    var archivoFoto = archivos[0];
                    if (archivoFoto.Length > 0)
                    {
                        var pathDestino = Path.Combine(_env.WebRootPath, "images/socios");
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFoto.FileName);

                        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                        {
                            await archivoFoto.CopyToAsync(filestream);
                            socio.Foto = archivoDestino;
                        }
                    }
                }

                _context.Add(socio);
                await _context.SaveChangesAsync();

                var cuotaExistente = _context.Cuotas.FirstOrDefault(c => c.SocioId == socio.Id);
                if (cuotaExistente != null)
                {
                    _context.Cuotas.Remove(cuotaExistente);
                    await _context.SaveChangesAsync();
                }

                // primera cuota
                var cuota = new Cuota
                {
                    SocioId = socio.Id,
                    EmisionCuota = DateTime.Now,
                    FechaVencimiento = DateTime.Now.AddMonths(1),
                    Monto = 2000,
                    Pagada = false
                };
                _context.Cuotas.Add(cuota);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "Id", "Nombre", socio.DisciplinaId);
            return View(socio);
        }

        // GET: Socios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socio = await _context.Socios
                .Include(s => s.Cuotas) 
                .FirstOrDefaultAsync(s => s.Id == id);

            if (socio == null)
            {
                return NotFound();
            }

            socio.ActualizarEstadoYBeneficios();

            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "Id", "Nombre", socio.DisciplinaId);
            ViewData["EstadoId"] = new SelectList(_context.Estados, "Id", "Descripcion", socio.EstadoId);
            ViewData["BeneficiosId"] = new SelectList(_context.Beneficios, "Id", "Descripcion", socio.EstadoId);
            return View(socio);
        }

        // POST: Socios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,DNI,FechaNacimiento,Foto,FechaAlta,DisciplinaId,EstadoId,Beneficios")] Socio socio)
        {
            if (id != socio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Manejo de la carga de archivos
                var archivos = HttpContext.Request.Form.Files;
                if (archivos?.Count > 0)
                {
                    var archivoFoto = archivos[0];
                    if (archivoFoto.Length > 0)
                    {
                        var pathDestino = Path.Combine(_env.WebRootPath, "images/socios");
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFoto.FileName);

                        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                        {
                            await archivoFoto.CopyToAsync(filestream);
                            // Eliminar la foto anterior si existe
                            if (!string.IsNullOrEmpty(socio.Foto))
                            {
                                var archivoViejo = Path.Combine(pathDestino, socio.Foto);
                                if (System.IO.File.Exists(archivoViejo))
                                {
                                    System.IO.File.Delete(archivoViejo);
                                }
                            }
                            socio.Foto = archivoDestino;
                        }
                    }
                }

                try
                {
                    _context.Update(socio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SocioExists(socio.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "Id", "Nombre", socio.DisciplinaId);
            return View(socio);
        }

        // GET: Socios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socio = await _context.Socios
                .Include(s => s.Disciplina)
                .Include(s => s.Estado)
                .Include(s => s.Cuotas) // Incluir las cuotas
                .FirstOrDefaultAsync(m => m.Id == id);

            if (socio == null)
            {
                return NotFound();
            }

            return View(socio);
        }

        // POST: Socios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var socio = await _context.Socios.FindAsync(id);
            if (socio != null)
            {
                _context.Socios.Remove(socio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SocioExists(int id)
        {
            return _context.Socios.Any(e => e.Id == id);
        }
    }
}
