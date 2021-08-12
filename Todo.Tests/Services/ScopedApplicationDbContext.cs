using System;
using Todo.Data;

namespace Todo.Tests.Services
{
    public class ScopedApplicationDbContext : IDisposable
    {
        public ScopedApplicationDbContext()
        {
            Context = TestApplicationDbContextFactory.Create();
        }
        
        public ApplicationDbContext Context { get; }
        
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}