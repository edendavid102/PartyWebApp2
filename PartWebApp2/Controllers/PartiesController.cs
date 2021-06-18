﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PartWebApp2.Data;
using PartWebApp2.Models;
using PartWebApp2.Services;

namespace PartWebApp2.Controllers
{
    public class PartiesController : Controller
    {
        private readonly PartyWebAppContext _context;
        private readonly ISpotifyClientService _spotifyClientService;

        public PartiesController(PartyWebAppContext context, ISpotifyClientService spotifyClientService)
        {
            _context = context;
            _spotifyClientService = spotifyClientService;
        }

        // GET: Parties
        public async Task<IActionResult> Index()
        {
            var partyWebAppContext = _context.Party.Include(p => p.area);
            return View(await partyWebAppContext.ToListAsync());
        }

        // GET: Parties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _context.Party
                .Include(a => a.area)
                .Include(a => a.club)
                .Include(a => a.genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }
            return View(party);
        }


        // GET: Parties/Create
        public IActionResult Create()
        {
            ViewData["genreId"] = new SelectList(_context.Set<Genre>(), "Id", "Type");
            ViewData["clubId"] = new SelectList(_context.Set<Club>(), "Id", "Name");
            ViewData["areaId"] = new SelectList(_context.Set<Area>(), "Id", "Type");
            return View();
        }

        // POST: Parties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,price,eventDate,startTime,minimalAge,areaId,maxCapacity,ProducerId,genreId,clubId")] Party party)
        {
            if (ModelState.IsValid)
            {
                _context.Add(party);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["genreId"] = new SelectList(_context.Set<Genre>(), "Id", "Id", party.genreId);
            ViewData["clubId"] = new SelectList(_context.Set<Club>(), "Id", "Id", party.clubId);
            ViewData["areaId"] = new SelectList(_context.Set<Area>(), "Id", "Id", party.areaId);
            return View(party);
        }

        // GET: Parties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _context.Party.FindAsync(id);
            if (party == null)
            {
                return NotFound();
            }
            ViewData["areaId"] = new SelectList(_context.Set<Area>(), "Id", "Id", party.areaId);
            return View(party);
        }

        // POST: Parties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,price,eventDate,startTime,minimalAge,areaId,maxCapacity,ProducerId")] Party party)
        {
            if (id != party.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(party);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartyExists(party.Id))
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
            ViewData["areaId"] = new SelectList(_context.Set<Area>(), "Id", "Id", party.areaId);
            return View(party);
        }

        // GET: Parties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _context.Party
                .Include(p => p.area)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        // POST: Parties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var party = await _context.Party.FindAsync(id);
            _context.Party.Remove(party);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartyExists(int id)
        {
            return _context.Party.Any(e => e.Id == id);
        }
    }
}
