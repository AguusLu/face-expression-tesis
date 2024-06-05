using System.Collections.Generic;
using facial_expression.WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace facial_expression.WEB.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Expresion> Expression { get; set; }
    }
}
