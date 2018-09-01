using EpamAspMvc.Models;
using System.Data.Entity;

namespace EpamAspMvc
{
    public class PicturesContext : DbContext
    {
        public PicturesContext() : base("PicturesDb")
        {

        }

        public DbSet<Picture> Pictures { get; set; }
    }
}