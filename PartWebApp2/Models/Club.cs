using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace PartWebApp2.Models
{
    public  class Club
    {
        public int id { get; set; }
        public string name { get; set; }
        public string locationID { get; set; }
    }
}
