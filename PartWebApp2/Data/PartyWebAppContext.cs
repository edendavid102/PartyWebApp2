using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PartWebApp2.Models;

namespace PartWebApp2.Data
{
    public class PartyWebAppContext : DbContext
    {
        public PartyWebAppContext(DbContextOptions<PartyWebAppContext> options)
    : base(options)
        {
        }
        public DbSet<Party> Party { get; set; }
        public DbSet<PartWebApp2.Models.Performer> Performer { get; set; }
        public DbSet<PartWebApp2.Models.PartyImage> PartyImage { get; set; }
        public DbSet<PartWebApp2.Models.User> User { get; set; }
        public DbSet<PartWebApp2.Models.Area> Area { get; set; }
        public DbSet<PartWebApp2.Models.Genre> Genre { get; set; }
        public DbSet<PartWebApp2.Models.Club> Club { get; set; }
    }
}