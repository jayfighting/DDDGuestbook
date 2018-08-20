using CleanArchitecture.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventDispatcher dispatcher)
            : base(options)
        {
            _dispatcher = dispatcher;
        }

        public DbSet<Guestbook> Guestbooks { get; set; }
        public DbSet<GuestbookEntry> GuestbookEntries { get; set; }
        public DbSet<ToDoItem> ToDoItems { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Blogging;Trusted_Connection=True;");
        //    }
        //}

        public override int SaveChanges()
        {
            int result = base.SaveChanges();

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity<int>>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach (var domainEvent in events)
                {
                    _dispatcher.Dispatch(domainEvent);
                }
            }

            return result;
        }
    }
}