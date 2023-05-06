using Microsoft.EntityFrameworkCore;

namespace Exam.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }


    }
}
