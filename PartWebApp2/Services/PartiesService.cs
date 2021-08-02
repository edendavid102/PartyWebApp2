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
using Microsoft.AspNetCore.Identity;
using PartWebApp2.Services;
using Microsoft.AspNetCore.Http;

namespace PartWebApp2.Services
{
    public class PartiesService
    {
        private readonly PartyWebAppContext _context;

        public PartiesService(PartyWebAppContext context)
        {
            _context = context;
        }

        public HomePage getDataForHomePage(HomePage homePage)
        {
            homePage.clubs = new List<Club>();
            homePage.parties = new List<Party>();
            homePage.performers = new List<Performer>();
            List<Party> allParties = _context.Party.Include(p => p.partyImage).ToList();
            List<Performer> allPerformers = _context.Performer.Include(p => p.parties).ToList();
            List<Club> allClubs = _context.Club.ToList();
            if (allParties != null && allPerformers != null && allClubs != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    homePage.parties.Add(allParties[i]);
                    homePage.performers.Add(allPerformers[i]);
                    homePage.clubs.Add(allClubs[i]);
                }
            }
            return homePage;
        }

        public List<Performer> allPerformers(List<Performer> performers)
        {
            performers = _context.Performer.ToList();
            return performers;
        }

        public void addPerformersToParty(Party party, List<string> perfomersSpotifyIds)
        {
            foreach (string spotifyId in perfomersSpotifyIds)
            {
                if (party.performers == null) party.performers = new List<Performer>();
                var existingPerformer = _context.Performer.FirstOrDefault(performer => performer.SpotifyId == spotifyId);
                bool isPerformerExistInDb = existingPerformer != null;
                if (isPerformerExistInDb)
                {
                    addExistingPerformerToParty(party, existingPerformer);
                } else
                {
                    var newPerformer = new Performer {
                        SpotifyId = spotifyId,
                        parties = new List<Party>()
                    };
                    addNewPerformerToParty(party, newPerformer);
                }
            }
        }

        public void addNewPerformerToParty(Party party, Performer performer)
        {
            performer.parties.Add(party);
            party.performers.Add(performer);
            _context.Performer.Add(performer);
            _context.Add(party);
        }

        public void addExistingPerformerToParty(Party party, Performer performer)
        {
            if (performer.parties != null) performer.parties.Add(party);
            else
            {
                performer.parties = new List<Party>();
                performer.parties.Add(party);
            }
            party.performers.Add(performer);
            _context.Update(performer);
            _context.Add(party);
        }

        public void addImageToParty(Party party, string Url)
        {
            PartyImage partyImage = new PartyImage();
            partyImage.imageUrl = Url;
            partyImage.Party = party;
            partyImage.PartyId = party.Id;
            _context.PartyImage.Add(partyImage);
            party.partyImage = partyImage;
            _context.Party.Add(party);
        }
        public async Task<PartyImage> createPartyImageAsync([Bind("Id,imageUrl,PartyId")] PartyImage partyImage)
        {
            _context.Add(partyImage);
            await _context.SaveChangesAsync();

            return partyImage;
        }

        public List<Party> mostPopularParties()
        {
            List<Party> mostPopular = new List<Party>();
            var sortPartiesByCountOfUsers = _context.Party.Include(p => p.area).Include(a => a.club).Include(t => t.genre).ToList();
            sortPartiesByCountOfUsers.OrderByDescending(u => u.users.Count());
            List<Party> mostPopularParties = new List<Party>();
            for (var i = 0; i < 5; i++)
            {
                mostPopularParties.Add(sortPartiesByCountOfUsers[i]);
            }
            return mostPopularParties;
        }

        public List<Party> sortByAreaType(int AreaType)
        {
            int i = 0;
            List<Party> sotyByArea = new List<Party>();
            var parties = _context.Party.Include(p => p.area).Include(a => a.club).Include(t => t.genre).ToList();
            while (sotyByArea.Count() <= 5 && i < parties.Count())
            {
                if (parties[i].area.Id == AreaType)
                {
                    sotyByArea.Add(parties[i]);
                }
            }
            return sotyByArea;
        }


        public void addProducerId(Party party, User user)
        {
            var p = _context.Party.FirstOrDefault(p => p.Id == party.Id);
            var u = _context.User.FirstOrDefault(u => u.Id == user.Id);
            if (p != null && u != null)
            {
                if (u.parties == null)
                {
                    u.parties = new List<Party>();
                }
                p.ProducerId = u.Id;
                u.parties.Add(p);
            }
        }

        public async void addPerformersToParty(Party party, Performer performer)
        {
            var per = _context.Performer.FirstOrDefault(per => per.SpotifyId == performer.SpotifyId);
            var p = _context.Party.FirstOrDefault(p => p.Id == party.Id);
            if (p != null && per != null)
            {
                var u = _context.Performer.FirstOrDefault(u => u.SpotifyId == u.SpotifyId);
                if (u.parties == null)
                {
                    u.parties = new List<Party>();
                }
                if (p.performers == null)
                {
                    p.performers = new List<Performer>();
                }
                u.parties.Add(party);
                p.performers.Add(u);
            }
            await _context.SaveChangesAsync();
        }
    }
}
