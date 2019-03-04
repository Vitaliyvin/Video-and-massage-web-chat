using BD.EF;
using BD.Entity;
using BD.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BD.Repositories
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private ApplicationContext applicationContext;
        private UserRepository userRepository;

        public UnitOfWork(string connection)
        {
            DbContextOptionsBuilder<ApplicationContext> dBContextOption = new DbContextOptionsBuilder<ApplicationContext>();
            dBContextOption.UseSqlServer(connection);
            applicationContext = new ApplicationContext(dBContextOption.Options);
        }

        IRepository<User> IUnitOfWork.User => userRepository ?? new UserRepository(applicationContext);


        public void Save()
        {
            applicationContext.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    applicationContext.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
