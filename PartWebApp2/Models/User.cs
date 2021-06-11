using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PartWebApp2.Models
{
    public enum UserType
    {
        Client,
        producer,
        Admin
    }
    public class User
    {
        public int Id { get; set; }
        [Required]

        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]

        [DataType(DataType.Password)]
        public string password { get; set; }
        [DataType(DataType.EmailAddress)]

        public string email { get; set; }
        [DataType(DataType.Date)]
        public DateTime birthDate { get; set; }
        public UserType Type { get; set; } = UserType.Client;
        public List<Party> parties { get; set; }

    }
}
