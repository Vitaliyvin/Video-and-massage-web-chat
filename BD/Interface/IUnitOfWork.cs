using BD.Entity;
using System;


namespace BD.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> User { get; }
        void Save();
    }
}
