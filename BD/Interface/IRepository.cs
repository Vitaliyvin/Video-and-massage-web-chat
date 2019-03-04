using System;
using System.Collections.Generic;
using System.Text;

namespace BD.Interface
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Create(T item);
        T Find(string item);
        void Update(T item);
        void Delete(int id);
        bool Any(string username);
    }
}
