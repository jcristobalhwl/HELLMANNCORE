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
    
    public partial class DB_MANIFEST : DbContext
    {
        public DB_MANIFEST()
            : base("name=DB_MANIFEST")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TBL_ADU_ADUANAAGENT> TBL_ADU_ADUANAAGENT { get; set; }
        public virtual DbSet<TBL_ADU_ADUANADESTINATION> TBL_ADU_ADUANADESTINATION { get; set; }
        public virtual DbSet<TBL_ADU_MANIFEST> TBL_ADU_MANIFEST { get; set; }
        public virtual DbSet<TBL_ADU_MANIFESTSHIPMENTDETAILDOC> TBL_ADU_MANIFESTSHIPMENTDETAILDOC { get; set; }
        public virtual DbSet<TBL_ADU_MANIFESTSHIPMENTDOC> TBL_ADU_MANIFESTSHIPMENTDOC { get; set; }
        public virtual DbSet<TBL_ADU_MASTERINFORMATION> TBL_ADU_MASTERINFORMATION { get; set; }
        public virtual DbSet<TBL_ADU_WAREDESCRIPTION> TBL_ADU_WAREDESCRIPTION { get; set; }
        public virtual DbSet<TBL_MAN_MANIFEST> TBL_MAN_MANIFEST { get; set; }
        public virtual DbSet<TBL_ADU_FLIGHT> TBL_ADU_FLIGHT { get; set; }
        public virtual DbSet<TBL_ADU_WEBTRACKING> TBL_ADU_WEBTRACKING { get; set; }
    }
}
