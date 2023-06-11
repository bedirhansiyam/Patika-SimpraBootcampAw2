using Aw2.Base.Models;
using Aw2.Data.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Net.Http;

namespace Aw2.Data.Context;

public class Aw2DbContext : DbContext
{
    public Aw2DbContext(DbContextOptions<Aw2DbContext> options) : base(options)
    {

    }

    public DbSet<Staff> Staff { get; set; }

    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries();

        foreach (var entity in entities)
        {
            if(entity.Entity is BaseModel trackable)
            {
                switch (entity.State)
                {
                    case EntityState.Modified:
                        trackable.UpdatedAt = DateTime.UtcNow;
                        entity.Property("CreatedAt").IsModified = false;
                        break;

                    case EntityState.Added:
                        trackable.CreatedAt = DateTime.UtcNow;
                        break;

                }
            }
            
        }
    }
}