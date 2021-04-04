using Microsoft.EntityFrameworkCore;
using ServiceDesk.Web.Data;

namespace ServiceDesk.IntegrationTests
{
    public class Utilities
    {
        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            var deleteSql = @"
                DELETE FROM dbo.Comments; 
                DELETE FROM dbo.Tickets;";
            db.Database.ExecuteSqlRaw(deleteSql);
        }
    }
}
