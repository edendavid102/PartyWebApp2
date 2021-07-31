using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PartWebApp2.Models
{
    public class Party
    {
        public int Id { get; set; }

        [Display(Name = "Party Name")]
        public string name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Event Date")]
        public DateTime eventDate { get; set; }

        [Display(Name = "Minimal Age")]
        public int minimalAge { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Open Doors")]
        public DateTime startTime { get; set; }
        [Display(Name = "Tickets purchased")]
        public int ticketsPurchased { get; set; }


        //All OneToMany 
        public int genreId { get; set; }
        public Genre genre { get; set; }
        public int areaId { get; set; }
        public Area area { get; set; }
        public int clubId { get; set; }
        [Display(Name = "Club")]
        public Club club { get; set; }
        public int ProducerId { get; set; }


        [Display(Name = "Maximum Capacity")]
        public int maxCapacity { get; set; }

        [Display(Name = "Price in NIS")]
        public double price { get; set; }

        //Many To Many 
        public List<Performer> performers { get; set; }
        public List<User> users { get; set; }

        //One To One 
        public PartyImage partyImage { get; set; }

    }
}
