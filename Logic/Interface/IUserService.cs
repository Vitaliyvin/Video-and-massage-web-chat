using BD.Entity;
using Logic.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Interface
{
    public interface IUserService
    {
        UserDto Authenticate(string username, string password);
        IEnumerable<UserDto> GetAll();
        UserDto GetById(int id);
        UserDto Create(UserDto user);
        void Update(UserDto user);
        void Delete(int id);
        void Dispose();
    }
}
