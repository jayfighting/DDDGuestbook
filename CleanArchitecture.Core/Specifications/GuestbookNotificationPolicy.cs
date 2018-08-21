using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CleanArchitecture.Core.Specifications
{
    public class GuestbookNotificationPolicy : BaseSpecification<GuestbookEntry>
    {
        public GuestbookNotificationPolicy(int entryAddedId = 0) : 
            base(e => e.DateTimeCreated > DateTimeOffset.UtcNow.AddDays(-1) // created after 1 day ago
            && e.Id != entryAddedId)
        {
        }
    }
}
