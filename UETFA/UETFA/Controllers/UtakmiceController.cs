﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UETFA.Data;
using UETFA.Models;

namespace UETFA.Controllers
{
    public class UtakmiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UtakmiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Utakmice
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.nazivi1 = new List<SelectListItem>();
            ViewBag.nazivi2 = new List<SelectListItem>();
            List<Utakmica> utakmice = _context.Utakmica.ToList();
            List<Tim> timovi = _context.Tim.ToList();
            foreach (var u in utakmice)
            {
                Tim t1 = timovi.Find(t => t.ID == u.idTima1);
                Tim t2 = timovi.Find(t => t.ID == u.idTima2);
                ViewBag.nazivi1.Add(new SelectListItem() { Text = t1.ime, Value = t1.ID.ToString() });
                ViewBag.nazivi2.Add(new SelectListItem() { Text = t2.ime, Value = t2.ID.ToString() });

            }
            var statusSort =
                await _context.Utakmica.OrderByDescending(m => m.statusUtakmice == "U toku").ThenBy(m => m.statusUtakmice == "Zavrsena").ToListAsync();

            return View(statusSort);
        }
        
        public async Task<IActionResult> Notifikacije()
        {
            ViewBag.nazivi1 = new List<SelectListItem>();
            ViewBag.nazivi2 = new List<SelectListItem>();
            List<Utakmica> utakmice = _context.Utakmica.ToList();
            List<Tim> timovi = _context.Tim.ToList();
            foreach (var u in utakmice)
            {
                if(u.statusUtakmice == "Zavrsena")
                {
                    Tim t1 = timovi.Find(t => t.ID == u.idTima1);
                    Tim t2 = timovi.Find(t => t.ID == u.idTima2);
                    ViewBag.nazivi1.Add(new SelectListItem() { Text = t1.ime, Value = t1.ID.ToString() });
                    ViewBag.nazivi2.Add(new SelectListItem() { Text = t2.ime, Value = t2.ID.ToString() });

                }
                
            }

            var notifikacija =
                await _context.Utakmica.Where(m => m.statusUtakmice == "Zavrsena").ToListAsync();

            return View(notifikacija);
        }



        // GET: Utakmice/Details/5
        //[Authorize(Roles = "Admin, Premium")]
        public async Task<IActionResult> Details(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var utakmica = await _context.Utakmica
                .FirstOrDefaultAsync(m => m.ID == ID);
            if (utakmica == null)
            {
                return NotFound();
            }
            ViewBag.nazivi1 = new List<SelectListItem>();
            ViewBag.nazivi2 = new List<SelectListItem>();
            List<Utakmica> utakmice = _context.Utakmica.ToList();
            List<Tim> timovi = _context.Tim.ToList();
            foreach (var u in utakmice)
            {
                if (u.ID == ID)
                {
                    Tim t1 = timovi.Find(t => t.ID == u.idTima1);
                    Tim t2 = timovi.Find(t => t.ID == u.idTima2);
                    ViewBag.nazivi1.Add(new SelectListItem() { Text = t1.ime, Value = t1.ID.ToString() });
                    ViewBag.nazivi2.Add(new SelectListItem() { Text = t2.ime, Value = t2.ID.ToString() });
                }
            }

            return View(utakmica);
        }

        // GET: Utakmice/Create
        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Sudije = new List<SelectListItem>();
            ViewBag.Timovi = new List<SelectListItem>();
            List<Tim> timovi = _context.Tim.ToList();
            foreach (var p in timovi)
                ViewBag.Timovi.Add(new SelectListItem() { Text = p.ime, Value = p.ID.ToString() });
            ViewBag.status = new List<SelectListItem>();
            ViewBag.status.Add( new SelectListItem() {Text= "Nije pocelo" });
            ViewBag.status.Add(new SelectListItem() { Text = "U toku" });
            return View();
        }

        // POST: Utakmice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("ID,statusUtakmice,datumUtakmice,idTima1,idTima2,rezTim1,rezTim2,Sudija")] Utakmica utakmica)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utakmica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utakmica);
        }

        // GET: Utakmice/Edit/5
        //[Authorize(Roles = "Admin, Sudija")]
        public async Task<IActionResult> Edit(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var utakmica = await _context.Utakmica.FindAsync(ID);
            if (utakmica == null)
            {
                return NotFound();
            }
            ViewBag.Timovi = new List<SelectListItem>();
            List<Tim> timovi = _context.Tim.ToList();
            foreach (var p in timovi)
                ViewBag.Timovi.Add(new SelectListItem() { Text = p.ime, Value = p.ID.ToString() });
            ViewBag.status = new List<SelectListItem>();
            ViewBag.status.Add(new SelectListItem() { Text = "Nije pocelo" });
            ViewBag.status.Add(new SelectListItem() { Text = "U toku" });
            return View(utakmica);
        }

        // POST: Utakmice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int ID, [Bind("ID,statusUtakmice,datumUtakmice,idTima1,idTima2,rezTim1,rezTim2,Sudija")] Utakmica utakmica)
        {
            if (ID != utakmica.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utakmica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtakmicaExists(utakmica.ID))
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

            return View(utakmica);
        }


        public async Task<IActionResult> ZavrsiUtakmicu (int ID)
        {
            var utakmica =
                    await _context.Utakmica.Where(m => m.ID == ID).ToListAsync();

            if (utakmica[0].statusUtakmice != "Zavrsena")
            {

                var tim1 =
                    await _context.Tim.Where(m => m.ID == utakmica[0].idTima1).ToListAsync();
                var tim2 =
                    await _context.Tim.Where(m => m.ID == utakmica[0].idTima2).ToListAsync();

                await _context.SaveChangesAsync();

                tim1[0].ID = utakmica[0].idTima1;
                tim2[0].ID = utakmica[0].idTima2;

                tim1[0].datiGolovi += utakmica[0].rezTim1;
                tim2[0].datiGolovi += utakmica[0].rezTim2;

                tim1[0].primljeniGolovi += utakmica[0].rezTim2;
                tim2[0].primljeniGolovi += utakmica[0].rezTim1;

                tim1[0].brojOdigranihUtakmica++;
                tim2[0].brojOdigranihUtakmica++;

                if (utakmica[0].rezTim1 > utakmica[0].rezTim2)
                {
                    tim1[0].bodovi += 3;
                    tim1[0].brojPobjeda++;
                    tim2[0].brojPoraza++;
                }
                else if (utakmica[0].rezTim1 < utakmica[0].rezTim2)
                {
                    tim2[0].bodovi += 3;
                    tim2[0].brojPobjeda++;
                    tim1[0].brojPoraza++;
                }
                else
                {
                    tim1[0].bodovi++;
                    tim2[0].bodovi++;
                    tim1[0].brojNerjesenih++;
                    tim2[0].brojNerjesenih++;
                }
                utakmica[0].statusUtakmice = "Zavrsena";
                _context.Tim.Update(tim1[0]);
                await _context.SaveChangesAsync();
                _context.Tim.Update(tim2[0]);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }






        // GET: Utakmice/Delete/5
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var utakmica = await _context.Utakmica
                .FirstOrDefaultAsync(m => m.ID == ID);
            if (utakmica == null)
            {
                return NotFound();
            }
            ViewBag.nazivi1 = new List<SelectListItem>();
            ViewBag.nazivi2 = new List<SelectListItem>();
            List<Utakmica> utakmice = _context.Utakmica.ToList();
            List<Tim> timovi = _context.Tim.ToList();
            foreach (var u in utakmice)
            {
                if (u.ID == ID)
                {
                    Tim t1 = timovi.Find(t => t.ID == u.idTima1);
                    Tim t2 = timovi.Find(t => t.ID == u.idTima2);
                    ViewBag.nazivi1.Add(new SelectListItem() { Text = t1.ime, Value = t1.ID.ToString() });
                    ViewBag.nazivi2.Add(new SelectListItem() { Text = t2.ime, Value = t2.ID.ToString() });
                }
            }
            return View(utakmica);
        }

        // POST: Utakmice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ID)
        {
            var utakmica = await _context.Utakmica.FindAsync(ID);
            _context.Utakmica.Remove(utakmica);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtakmicaExists(int ID)
        {
            return _context.Utakmica.Any(e => e.ID == ID);
        }
    }
}
