using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EduVerse.Data
{
    public class EduVerseContextFactory : IDesignTimeDbContextFactory<EduVerseContext>
    {
        public EduVerseContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<Program>()
                .Build();

            var connString = config["ORACLE_CONN_STRING"];

            var optionsBuilder = new DbContextOptionsBuilder<EduVerseContext>();
            optionsBuilder.UseOracle(connString);

            return new EduVerseContext(optionsBuilder.Options);
        }
    }
}
