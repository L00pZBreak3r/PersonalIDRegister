using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

using DocumentLibrary.Models;

namespace DocumentLibrary.Contexts
{
  public class IdentificationDocumentContext : DbContext
  {
    public IdentificationDocumentContext()
            : base("DbConnection")
    { }

    public DbSet<IdentificationDocument> Documents { get; set; }
  }
}
