using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPFinalCe.Models;

namespace TPFinalCe.Controllers
{
    public class CuotasController : Controller
    {
        private readonly AppDBContext _context;

        public CuotasController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Cuotas
        public async Task<IActionResult> Index(string isPagada)
        {
            var appDBContext = _context.Cuotas.Include(c => c.Socio).AsQueryable();

            if (!string.IsNullOrEmpty(isPagada))
            {
                bool? filterPagada = null;

                if (isPagada == "Pagadas")
                {
                    filterPagada = true;
                }
                else if (isPagada == "No Pagadas")
                {
                    filterPagada = false;
                }

                if (filterPagada.HasValue)
                {
                    appDBContext = appDBContext.Where(c => c.Pagada == filterPagada.Value);
                }
            }
            return View(await appDBContext.ToListAsync());
        }

        // GET: Cuotas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuota = await _context.Cuotas
                .Include(c => c.Socio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cuota == null)
            {
                return NotFound();
            }

            return View(cuota);
        }

        // GET: Cuotas/Create
        public IActionResult Create()
        {
            ViewData["SocioId"] = new SelectList(_context.Socios, "Id", "Apellido");
            return View();
        }

        // POST: Cuotas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SocioId,Monto,Pagada")] Cuota cuota)
        {
            cuota.EmisionCuota = DateTime.Now;
            cuota.FechaVencimiento = cuota.EmisionCuota.AddDays(30);

            var socioActual = await _context.Socios.FindAsync(cuota.SocioId);
            if (socioActual == null)
            {
                return NotFound();
            }

            var apellidoSocio = socioActual.Apellido;

            var cuotasAnterioresMismoApellido = _context.Cuotas
                .Include(c => c.Socio)
                .Where(c => c.Socio.Apellido == apellidoSocio);

            _context.Cuotas.RemoveRange(cuotasAnterioresMismoApellido);

            _context.Add(cuota);
            await _context.SaveChangesAsync();

            await ActualizarEstadoSocioSiCuotasVencidas(cuota.SocioId);

            return RedirectToAction(nameof(Index));
        }

        // GET: Cuotas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuota = await _context.Cuotas.FindAsync(id);
            if (cuota == null)
            {
                return NotFound();
            }
            ViewData["SocioId"] = new SelectList(_context.Socios, "Id", "Apellido", cuota.SocioId);
            return View(cuota);
        }

        // POST: Cuotas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SocioId,Monto,EmisionCuota,FechaVencimiento,Pagada")] Cuota cuota)
        {
            if (id != cuota.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(cuota);
                    await _context.SaveChangesAsync();

                    await ActualizarEstadoSocioSiCuotasVencidas(cuota.SocioId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuotaExists(cuota.Id))
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

            ViewData["SocioId"] = new SelectList(_context.Socios, "Id", "Apellido", cuota.SocioId);
            return View(cuota);
        }

        // GET: Cuotas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuota = await _context.Cuotas
                .Include(c => c.Socio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cuota == null)
            {
                return NotFound();
            }

            return View(cuota);
        }

        // POST: Cuotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cuota = await _context.Cuotas.FindAsync(id);
            if (cuota != null)
            {
                _context.Cuotas.Remove(cuota);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CuotaExists(int id)
        {
            return _context.Cuotas.Any(e => e.Id == id);
        }

        private async Task ActualizarEstadoSocioSiCuotasVencidas(int socioId)
        {
            var cuotasImpagas = await _context.Cuotas
                .Where(c => c.SocioId == socioId && !c.Pagada && c.FechaVencimiento < DateTime.Now.AddMonths(-1))
                .ToListAsync();

            if (cuotasImpagas.Any())
            {
                var socio = await _context.Socios.FindAsync(socioId);
                if (socio != null)
                {
                    socio.EstadoId = 2; 
                    _context.Update(socio);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
