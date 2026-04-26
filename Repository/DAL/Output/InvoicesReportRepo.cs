using Entity;
using System;
using System.Linq;
using Entity.Model;
using Entity.ModelView;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
   public class InvoicesReportRepo : CurdOrg<Invoice>
    {
        //public OrgContext db;

        public InvoicesReportRepo(string Schema) : base(Schema)
        {
            //if (this.db == null)
            //    this.db = new OrgContext(new DbContextOptions<OrgContext>(), Schema);
        }
    }
}