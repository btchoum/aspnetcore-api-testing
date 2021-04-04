using Microsoft.EntityFrameworkCore;
using ServiceDesk.Web.Data;

namespace ServiceDesk.IntegrationTests
{
    public class Utilities
    {
        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            db.Database.ExecuteSqlRaw("DELETE FROM dbo.Tickets");
        }
    }
}
