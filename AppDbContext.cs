using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuestionExchange.Models;
namespace QuestionExchange
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var mapper = new Npgsql.NpgsqlSnakeCaseNameTranslator();
            var types = modelBuilder.Model.GetEntityTypes().ToList();

            // Refer to tables in snake_case internally
            types.ForEach(e => e.Relational().TableName = mapper.TranslateMemberName(e.Relational().TableName));
            
            // Refer to columns in snake_case internally
            types.SelectMany(e => e.GetProperties())
                .ToList()
                .ForEach(p => p.Relational().ColumnName = mapper.TranslateMemberName(p.Relational().ColumnName));
        }
    }
}
