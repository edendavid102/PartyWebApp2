using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private readonly PartiesService _partiesService;
        private readonly ISpotifyClientService _spotifyClientService;

        public PartiesController(PartyWebAppContext context, PartiesService service, ISpotifyClientService spotifyClientService)
        {
            _context = context;
            _partiesService = service;
            _spotifyClientService = spotifyClientService;
        }

        // GET: Parties
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var partyWebAppContext = _context.Party.Include(p => p.area)
                .Include(p => p.club)
                .Include(p => p.genre)
                .Include(p => p.partyImage)
                .Include(p => p.performers);

            return View(await partyWebAppContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> homePage()
        {
            var partyWebAppContext = _context.Party.Include(p => p.area)
                        .Include(p => p.club)
                        .Include(p => p.genre)
                        .Include(p => p.partyImage)
                        .Include(p => p.performers);
            return View(await partyWebAppContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> findParty(string partyName)
        {
            var partyWebAppContext = _context.Party.Include(p => p.area)
                .Include(p => p.club)
                .Include(p => p.genre)
                .Include(p => p.partyImage)
                .Include(p => p.performers)
                .Where(p => p.name.Contains(partyName));

            return View("homePage", await partyWebAppContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> findPartyInMyParties(string partyName)
        {
            var currentUserId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var partyWebAppContext = _context.Party.Include(p => p.area)
                .Include(p => p.club)
                .Include(p => p.genre)
                .Include(p => p.partyImage)
                .Include(p => p.performers).Where(p => p.ProducerId.Equals(currentUserId));

            return View("Index", await partyWebAppContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> myParties()
        {
            var currentUserId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = _context.User.FirstOrDefault(u => u.Id == Int32.Parse(currentUserId));
            const int clientPermissionLevel = 0;
            Boolean isProducer = currentUser.Type > clientPermissionLevel;
            if (isProducer)
            {
                var partyWebAppContext = _context.Party.Include(p => p.area)
                .Include(p => p.club)
                .Include(p => p.genre)
                .Include(p => p.partyImage)
                .Include(p => p.performers)
                .Where(p => p.ProducerId == Int32.Parse(currentUserId));
                return View(await partyWebAppContext.ToListAsync());
            }
            else
            {
                var partyWebAppContext = _context.Party.Include(p => p.area)
                .Include(p => p.club)
                .Include(p => p.genre)
                .Include(p => p.partyImage)
                .Include(p => p.performers)
                .Where(p => p.users.All(u => u.Id == Int32.Parse(currentUserId)));

                return View("homePage", await partyWebAppContext.ToListAsync());
            }
        }

        [Authorize]
        public List<Party> mostPopularParties()
        {
            return (_partiesService.mostPopularParties());
        }

        // GET: Parties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _context.Party
                .Include(p => p.area)
                .Include(p => p.club)
                .Include(p => p.genre)
                .Include(p => p.partyImage)
                .Include(p => p.performers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }
            if (party.performers != null)
            {
                List<string> performersSpotifyIds = party.performers.Select(performer => performer.SpotifyId).ToList();
                var artists = await _spotifyClientService.GetArtists(performersSpotifyIds);
                ViewBag.artists = artists.ToArray();
            }

            return View(party);
        }

        // GET: Parties/Create

        [Authorize(Roles = "Admin, producer")]
        public IActionResult Create()
        {
            ViewData["Genres"] = new SelectList(_context.Set<Genre>(), "Id", "Type");
            ViewData["Clubs"] = new SelectList(_context.Set<Club>(), "Id", "Name");
            ViewData["Areas"] = new SelectList(_context.Set<Area>(), "Id", "Type");
           
            return View();
        }

        // POST: Parties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, producer")]
        public async Task<IActionResult> Create([Bind("Id,name,eventDate,minimalAge,startTime,genreId,areaId,clubId,ProducerId,maxCapacity,price,ticketsPurchased")] Party party, string imageUrl, List<string> performersId)
        {
            if (ModelState.IsValid)
            {
                var producerId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                party.ProducerId = Int32.Parse(producerId);
                party.ticketsPurchased = 0;
                party.performers = new List<Performer>();
                _partiesService.addPerformersToParty(party, performersId);
                _partiesService.addImageToParty(party, imageUrl);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Genres"] = new SelectList(_context.Set<Genre>(), "Id", "Type");
            ViewData["Clubs"] = new SelectList(_context.Set<Club>(), "Id", "Name");
            ViewData["Areas"] = new SelectList(_context.Set<Area>(), "Id", "Type");
            return View(party);
        }
        // GET: Parties/Edit/5
        [Authorize(Roles = "Admin, producer")]
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

            ViewData["Areas"] = new SelectList(_context.Set<Area>(), nameof(Area.Id), nameof(Area.Type));
            ViewData["Clubs"] = new SelectList(_context.Set<Club>(), nameof(Club.Id), nameof(Club.Name));
            ViewData["Genres"] = new SelectList(_context.Set<Genre>(), nameof(Genre.Id), nameof(Genre.Type));
            
            return View(party);
        }

        // POST: Parties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, producer")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,eventDate,minimalAge,startTime,genreId,areaId,clubId,ProducerId,maxCapacity,price,ticketsPurchased")] Party party)
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
                return RedirectToAction(nameof(myParties));
            }
            ViewData["Areas"] = new SelectList(_context.Set<Area>(), nameof(Area.Id), nameof(Area.Type));
            ViewData["Clubs"] = new SelectList(_context.Set<Club>(), nameof(Club.Id), nameof(Club.Name));
            ViewData["Genres"] = new SelectList(_context.Set<Genre>(), nameof(Genre.Id), nameof(Genre.Type));

            return View(party);
        }

        // GET: Parties/Delete/5
        [Authorize(Roles = "Admin, producer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _context.Party
                .Include(p => p.area)
                .Include(p => p.club)
                .Include(p => p.genre)
                .Include(p => p.partyImage)
                .Include(p => p.performers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        //Get
        [Authorize]
        public async Task<IActionResult> buyTickets(int? id, int count)
        {

            if (id == null)
            {
                return NotFound();
            }
            var party = await _context.Party
                .Include(p => p.area)
                .Include(p => p.club)
                .Include(p => p.genre)
                .Include(p => p.partyImage)
                .Include(p => p.performers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }
            return View(party);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> buyTickets(int id, int count)
        {
            var currentUserId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = _context.User.FirstOrDefault(u => u.Id == Int32.Parse(currentUserId));

            var party = await _context.Party.FindAsync(id);
            party.users.Add(currentUser);
            party.ticketsPurchased += count;

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
            return View(party);
        }

        // POST: Parties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, producer")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var party = await _context.Party.FindAsync(id);
            _context.Party.Remove(party);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<string> GetArtistIdBySearchParams(string queryParams)
        {
            return await _spotifyClientService.getArtistIdBySearchParams(queryParams);
        }

        private bool PartyExists(int id)
        {
            return _context.Party.Any(e => e.Id == id);
        }
    }
}
