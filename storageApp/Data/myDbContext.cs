using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using storageApp.Models;

namespace storageApp.Data
{
    public class myDbContext : DbContext
    {
        //add models 
        

        //constructor
        public myDbContext(DbContextOptions<myDbContext> options)
        : base(options)
        {

        }
        public DbSet<logData> Logs {get;set;}
    }
}