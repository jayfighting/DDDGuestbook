using System;
using System.Collections.Generic;

namespace CleanArchitecture.Core.SharedKernel
{
    // This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
    public abstract class BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }

        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
    }
}