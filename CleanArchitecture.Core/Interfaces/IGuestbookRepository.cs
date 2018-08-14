using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Helpers;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IGuestbookRepository : IRepository<Guestbook>
    {
        List<GuestbookEntry> ListEntries(ISpecification<GuestbookEntry> spec);
    }
}