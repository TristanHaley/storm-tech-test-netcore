using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Todo.Data;

namespace Todo.Tests.Services
{
    public static class TestApplicationDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseSqlite(CreateInMemoryDatabase())
                         .Options;

            var applicationDbContext = new ApplicationDbContext(options);
            applicationDbContext.Database.EnsureCreated();
            applicationDbContext.SaveChanges();

            return applicationDbContext;
        }
        
        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

    }
}