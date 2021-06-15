using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace PartWebApp2.Models
{

    public enum AreaType
    {
        [Display(Name = "Center")]
        center = 1,
        [Display(Name = "North")]
        north = 2,
        [Display(Name = "South")]
         south= 3,
        [Display(Name = "Hashron")]
        hasharon = 4,
    }
    public class Area
    {
        public int Id { get; set; }
        public AreaType Type { get; set; }
        public List<Party> Parties { get; set; }
    }
}
