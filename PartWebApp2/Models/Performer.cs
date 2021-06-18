using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using SpotifyAPI.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartWebApp2.Models
{
    public class Performer
    {
        public int Id { get; set; }
        public string SpotifyId { get; set; }
        public List<Party> parties { get; set; }
    }
}
