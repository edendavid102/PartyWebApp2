using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PartWebApp2.Models
{
    public class Party
    {
        public int Id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Event Date")]
        public DateTime eventDate { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Open Doors")]
        public DateTime startTime { get; set;}
        [Display(Name = "Genre")]
        public Genre genre { get; set; }
        [Display(Name = "Minimal Age")]
        public int minimalAge { get; set; }
        public int areaId { get; set; }
        public Area area { get; set; } 
        public int maxCapacity { get; set; }
        public List<User> users { get; set; }
        public int ProducerId { get; set; }
        [Display(Name = "Performers")]
        public List<Performer> performers { get; set; }
        [Display(Name = "Club")]
        //public int clubId { get; set; }
        public Club club { get; set; }
        [Display(Name = "Image")]
        public PartyImage image { get; set; }
    }
}
