using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baciu_Dora_Lab2.Models;

namespace Baciu_Dora_Lab2.Data
{
    public class Baciu_Dora_Lab2Context : DbContext
    {
        public Baciu_Dora_Lab2Context (DbContextOptions<Baciu_Dora_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Baciu_Dora_Lab2.Models.Book> Book { get; set; } = default!;
        public DbSet<Baciu_Dora_Lab2.Models.Publisher> Publisher { get; set; } = default!;
    }
}
