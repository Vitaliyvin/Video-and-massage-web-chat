using AutoMapper;
using BD.Entity;
using BD.Interface;
using BD.Repositories;
using Logic.DTO;
using Logic.Helpers;
using Logic.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Service
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService(string connection)
        {
            Database = new UnitOfWork(connection);
        }
        public void Dispose()
        {
            Database.Dispose();
        }

        public UserDto Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = Database.User.Find(username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDto>()).CreateMapper();
            UserDto userDto = mapper.Map<User, UserDto>(user);
            return userDto;
        }

        public IEnumerable<UserDto> GetAll()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDto>()).CreateMapper();
            IEnumerable<UserDto> userDto = mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(Database.User.GetAll());
            return userDto;
        }

        public UserDto GetById(int id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDto>()).CreateMapper();
            UserDto userDto = mapper.Map<User, UserDto>(Database.User.Get(id));
            return userDto;
        }

        public UserDto Create(UserDto userDto)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDto, User>()).CreateMapper();
            User user = mapper.Map<UserDto, User>(userDto);

            // validation
            if (string.IsNullOrWhiteSpace(userDto.Password))
                throw new AppException("Password is required");

            if (Database.User.Any(user.Username))
                throw new AppException("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            Database.User.Create(user);
            Database.Save();
            //AutoMapperConfigurationException
            var mapperDto = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDto>()).CreateMapper();
            return mapperDto.Map<User, UserDto>(user);
        }

        public void Update(UserDto userParam)
        {
            var user = Database.User.Get(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (Database.User.Any(userParam.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");
            }

            // update user properties
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.Username = userParam.Username;
            user.Email = userParam.Email;
            user.DateOfBirthday = userParam.DateOfBirthday;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(userParam.Password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(userParam.Password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            Database.User.Update(user);
            Database.Save();
        }

        public void Delete(int id)
        {
            var user = Database.User.Get(id);
            if (user != null)
            {
                Database.User.Delete(id);
                Database.Save();
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }


    }
}
