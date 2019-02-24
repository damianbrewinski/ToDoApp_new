using ToDoApp.Areas.Zadania.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;


namespace ToDoApp.Services
{
    public class ZadaniaContext : DbContext
    {
        public ZadaniaContext() : base("ZadaniaContext")
        {

        }
        
        public DbSet<ToDoModel> Zadania { get; set; }
       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Wylacza nazewnictwo mnogie nazw tabel
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}