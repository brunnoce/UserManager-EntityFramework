﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPFinalCe.Models;

namespace TPFinalCe.Controllers
{
    public class BeneficiosController : Controller
    {
        private readonly AppDBContext _context;

        public BeneficiosController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Beneficios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Beneficios.ToListAsync());
        }

        // GET: Beneficios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beneficios = await _context.Beneficios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beneficios == null)
            {
                return NotFound();
            }

            return View(beneficios);
        }

        // GET: Beneficios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Beneficios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion")] Beneficios beneficios)
        {
            if (ModelState.IsValid)
            {
                _context.Add(beneficios);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(beneficios);
        }

        // GET: Beneficios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beneficios = await _context.Beneficios.FindAsync(id);
            if (beneficios == null)
            {
                return NotFound();
            }
            return View(beneficios);
        }

        // POST: Beneficios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion")] Beneficios beneficios)
        {
            if (id != beneficios.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(beneficios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BeneficiosExists(beneficios.Id))
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
            return View(beneficios);
        }

        // GET: Beneficios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beneficios = await _context.Beneficios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beneficios == null)
            {
                return NotFound();
            }

            return View(beneficios);
        }

        // POST: Beneficios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var beneficios = await _context.Beneficios.FindAsync(id);
            if (beneficios != null)
            {
                _context.Beneficios.Remove(beneficios);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BeneficiosExists(int id)
        {
            return _context.Beneficios.Any(e => e.Id == id);
        }
    }
}
