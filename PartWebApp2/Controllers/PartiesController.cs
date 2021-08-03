using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DevTrends.MvcDonutCaching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PartWebApp2.Data;
using PartWebApp2.Models;
using PartWebApp2.Services;
using System.Data.SqlClient;
using System.Configuration;

namespace PartWebApp2.Controllers
{
    public class PartiesController : Controller
    {
        private readonly PartyWebAppContext _context;
        private readonly PartiesService _partiesService;
        private readonly ISpotifyClientService _spotifyClientService;

        //var query = context.People
        //           .GroupBy(p => p.name)
        //           .Select(g => new { name = g.Key, count = g.Count() });

        public PartiesController(PartyWebAppContext context, PartiesService service, ISpotifyClientService spotifyClientService)
        {
            _context = context;
            _partiesService = service;
            _spotifyClientService = spotifyClientService;
        }

        public int findCurrentUserId()
        {
            return Int32.Parse(HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        public void initTypeUserToViewData(User currentUser)
        {
            ViewData["UserFullName"] = currentUser.firstName + " " + currentUser.lastName;
            if (currentUser.Type == UserType.Admin)
            {
                ViewData["UserType"] = "Manager";
            }
            else if (currentUser.Type == UserType.producer)
            {
                ViewData["UserType"] = "Producer";
            }
            else
            {
                ViewData["UserType"] = "Client";
            }
        }
        public User returnCurrentUser()
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Id == findCurrentUserId());
            initTypeUserToViewData(currentUser);
            return currentUser;
        }
        // GET: Parties

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewData["PageName"] = "Index";
            initTypeUserToViewData(returnCurrentUser());
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
            ViewData["PageName"] = "All Parties";
            initTypeUserToViewData(returnCurrentUser());
            var partyWebAppContext = _context.Party.Include(p => p.area)
                        .Include(p => p.club)
                        .Include(p => p.genre)
                        .Include(p => p.partyImage)
                        .Include(p => p.performers);

            return View(await partyWebAppContext.ToListAsync());
        }

        //public async Task<IActionResult> joinClubsWithParties()
        //{
        //    var data = _context.Club.Join(
        //        _context.Party,
        //        club => club.Name,
        //        party => party.name,
        //        (club, party) => new
        //        {
        //            ClubName = club.Name,
        //            PartyName = party.name,
        //            partyGenre = party.genre
        //        }
        //                   );
        //    return View("ClubsAndParties", await data.ToListAsync());
        //}

        [Authorize]
        public async Task<IActionResult> findParty(string partyName)
        {
            initTypeUserToViewData(returnCurrentUser());
            var partyWebAppContext = _context.Party.Include(p => p.area)
                .Include(p => p.club)
                .Include(p => p.genre)
                .Include(p => p.partyImage)
                .Include(p => p.performers)
                .Where(p => p.name.Contains(partyName));

            return View("HomePage", await partyWebAppContext.ToListAsync());
        }


        [Authorize]
        public async Task<IActionResult> findPartyInMyParties(string partyName)
        {
            initTypeUserToViewData(returnCurrentUser());
            var partyWebAppContext = _context.Party.Include(p => p.area)
                .Include(p => p.club)
                .Include(p => p.genre)
                .Include(p => p.partyImage)
                .Include(p => p.performers).Where(p => p.ProducerId.Equals(findCurrentUserId()));

            return View("HomePage", await partyWebAppContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> myParties()
        {
            ViewData["PageName"] = "My Parties";
            initTypeUserToViewData(returnCurrentUser());
            const UserType clientPermissionLevel = UserType.Client;

            var partyWebAppContext = _context.Party.Include(p => p.area)
               .Include(p => p.club)
               .Include(p => p.genre)
               .Include(p => p.partyImage)
               .Include(p => p.performers);

            Boolean isProducer = returnCurrentUser().Type > clientPermissionLevel;
            if (isProducer)
            {
                partyWebAppContext.Where(p => p.ProducerId == findCurrentUserId());
            }
            else
            {
                partyWebAppContext.Where(p => p.users.Contains(returnCurrentUser()));
            }
            return View("HomePage", await partyWebAppContext.ToListAsync());
        }

        [Authorize]
        public IEnumerable<Party> mostPopularParties()
        {
            return (_partiesService.mostPopularParties());
        }

        // GET: Parties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            initTypeUserToViewData(returnCurrentUser());
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

        // GET: Parties/Create

        [Authorize(Roles = "Admin, producer")]
        public IActionResult Create()
        {
            ViewData["Genres"] = new SelectList(_context.Set<Genre>(), "Id", "Type");
            ViewData["Clubs"] = new SelectList(_context.Set<Club>(), "Id", "Name");
            ViewData["Areas"] = new SelectList(_context.Set<Area>(), "Id", "Type");
            ViewData["PerformersId"] = new SelectList(_context.Set<Performer>(), "Id", "SpotifyId");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, producer")]
        public async Task<IActionResult> Create([Bind("Id,name,eventDate,minimalAge,startTime,genreId,areaId,clubId,ProducerId,maxCapacity,price,ticketsPurchased")] Party party, string imageUrl, List<int> performersId)
        {
            if (ModelState.IsValid)
            {
                party.ProducerId = findCurrentUserId();

                party.ticketsPurchased = 0;
                imageUrl = _partiesService.defaultImageIfIsNull(imageUrl);
                _partiesService.addImageToParty(party, imageUrl);

                await _context.SaveChangesAsync();

                _partiesService.addPerformersToParty(party, performersId);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(party);
        }


        public async Task<IActionResult> Payment(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(int partyId, int numOfTickets)
        {
            var currentUserId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = _context.User.FirstOrDefault(u => u.Id == Int32.Parse(currentUserId));
            _partiesService.addTicketsCountToParty(partyId, numOfTickets, currentUser);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(myParties));
        }


        //// GET: Parties/Edit/5
        //[Authorize(Roles = "Admin, producer")]
        //public async Task<IActionResult> EditNullPartyImage(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var party = await _context.Party.FindAsync(id);
        //    if (party == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["Areas"] = new SelectList(_context.Set<Area>(), nameof(Area.Id), nameof(Area.Type));
        //    ViewData["Clubs"] = new SelectList(_context.Set<Club>(), nameof(Club.Id), nameof(Club.Name));
        //    ViewData["Genres"] = new SelectList(_context.Set<Genre>(), nameof(Genre.Id), nameof(Genre.Type));

        //    return View(party);
        //}


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, producer")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,eventDate,minimalAge,startTime,genreId,areaId,clubId,ProducerId,maxCapacity,price,ticketsPurchased")] Party party)
        {
            if (id != party.Id)
            {
                return NotFound();
            }
            if (returnCurrentUser().Id == party.ProducerId || returnCurrentUser().Type == UserType.Admin)
            {
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
                    return View("Details");
                }
                ViewData["Areas"] = new SelectList(_context.Set<Area>(), nameof(Area.Id), nameof(Area.Type));
                ViewData["Clubs"] = new SelectList(_context.Set<Club>(), nameof(Club.Id), nameof(Club.Name));
                ViewData["Genres"] = new SelectList(_context.Set<Genre>(), nameof(Genre.Id), nameof(Genre.Type));
                return View(party);
            }
            else
            {
                ViewData["Error"] = "You Cant Edit this Party, its not yours!";
            }
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

        private bool PartyExists(int id)
        {
            return _context.Party.Any(e => e.Id == id);
        }
    }
}
