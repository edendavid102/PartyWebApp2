using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PartWebApp2.Models
{
    public enum GenreType
    {
        None = 0x0,

        [Display(Name = "Hip Hop")]
        hipHop = 0x1,
        [Display(Name = "Rock")]
        rock = 0x2,
        [Display(Name = "Techno")]
        techno = 0x3,
        [Display(Name = "House")]
        house = 0x4,
        [Display(Name = "Pop")]
        pop = 0x5,
    }

    public class Genre
    {
        public int Id { get; set; }
        public GenreType Type { get; set; }
        public List<Party> parties { get; set; }
    }

}
