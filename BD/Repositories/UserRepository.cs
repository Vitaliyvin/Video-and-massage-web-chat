using BD.EF;
using BD.Entity;
using BD.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BD.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private ApplicationContext db;

        public UserRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public void Create(User item)
        {
            db.Users.Add(item);
        }
        public bool Any(string username)
        {
            return db.Users.Any(x => x.Username == username);
        }

        public void Delete(int id)
        {
            User user = db.Users.Find(id);
            if (user != null)
                db.Users.Remove(user);
        }

        public User Get(int id)
        {
            return db.Users.Find(id);
        }
        public User Find(string username)
        {
            return db.Users.SingleOrDefault(us => us.Username == username);
        }

        public IEnumerable<User> GetAll()
        {
            return db.Users;
        }

        public void Update(User item)
        {
            db.Users.Update(item);
            //db.Entry(item).State = EntityState.Modified;
        }
    }
}
