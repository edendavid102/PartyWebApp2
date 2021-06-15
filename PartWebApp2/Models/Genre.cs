using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PartWebApp2.Models
{
    public enum GenreType
    {
        [Display(Name = "Hip Hop")]
        hipHop = 1,
        [Display(Name = "Rock")]
        rock = 2,
        [Display(Name = "Techno")]
        techno = 3,
        [Display(Name = "House")]
        house = 4,
        [Display(Name = "Pop")]
        pop = 5,
    }

    public class Genre
    {
        public int Id { get; set; }
        public GenreType Type { get; set; }
        public List<Party> parties { get; set; }
    }

}
