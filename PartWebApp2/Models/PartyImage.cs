using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PartWebApp2.Models
{
    public class PartyImage
    {
        public int id { get; set; }
        [Required]
        [DataType(DataType.ImageUrl)]
        public string imageUrl { get; set; }
        public int partyId { get; set; }
        public Party party { get; set; }
    }
}
