﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DbCore : DbContext
    {
        public DbCore()
            : base("name=DbCore")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TBL_SLI_CITY> TBL_SLI_CITY { get; set; }
        public virtual DbSet<TBL_SLI_CONTINENT> TBL_SLI_CONTINENT { get; set; }
        public virtual DbSet<TBL_SLI_COUNTRY> TBL_SLI_COUNTRY { get; set; }
        public virtual DbSet<TBL_SLI_CURRENCY> TBL_SLI_CURRENCY { get; set; }
        public virtual DbSet<TBL_SLI_DISTRICT> TBL_SLI_DISTRICT { get; set; }
        public virtual DbSet<TBL_SLI_PROVINCE> TBL_SLI_PROVINCE { get; set; }
    }
}
