using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PartWebApp2.Models
{
    public class HomePage
    {
        public List<Club> clubs { get; set; }
        public List<Performer> performers { get; set; }
        public List<Party> parties { get; set; }
    }
}
