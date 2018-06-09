namespace DocumentManagementProject.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DocumentDbContext : DbContext
    {
        public virtual DbSet<DocumentModel> Documents { get; set; }
        public DocumentDbContext()
            : base("name=DocumentDbContext")
        {
        }

    }

}