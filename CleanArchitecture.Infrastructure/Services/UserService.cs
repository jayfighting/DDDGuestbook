//using System;
//using System.Collections.Generic;
//using System.Linq;
//using CleanArchitecture.Core.Entities;
//using CleanArchitecture.Core.Helpers;
//using CleanArchitecture.Core.Interfaces;
//using CleanArchitecture.Infrastructure.Data;

//namespace CleanArchitecture.Infrastructure.Services
//{
//    public class UserService : IUserService
//    {
//        private readonly AppDbContext _dbContext;

//        public UserService(AppDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public User Authenticate(string username, string password)
//        {
//            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
//                return null;

//            var user = _dbContext.Users.SingleOrDefault(x => x.Username == username);

//            // check if username exists
//            if (user == null)
//                return null;

//            // check if password is correct
//            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
//                return null;

//            // authentication successful
//            return user;
//        }

//        public IEnumerable<User> GetAll()
//        {
//            return _dbContext.Users;
//        }

//        public User GetById(int id)
//        {
//            return _dbContext.Users.Find(id);
//        }

//        public User Create(User user, string password)
//        {
//            // validation
//            if (string.IsNullOrWhiteSpace(password))
//                throw new AppException("Password is required");

//            if (_dbContext.Users.Any(x => x.Username == user.Username))
//                throw new AppException("Username \"" + user.Username + "\" is already taken");

//            byte[] passwordHash, passwordSalt;
//            CreatePasswordHash(password, out passwordHash, out passwordSalt);

//            user.PasswordHash = passwordHash;
//            user.PasswordSalt = passwordSalt;

//            _dbContext.Users.Add(user);
//            _dbContext.SaveChanges();

//            return user;
//        }

//        public void Update(User userParam, string password = null)
//        {
//            var user = _dbContext.Users.Find(userParam.Id);

//            if (user == null)
//                throw new AppException("User not found");

//            if (userParam.Username != user.Username)
//            {
//                // username has changed so check if the new username is already taken
//                if (_dbContext.Users.Any(x => x.Username == userParam.Username))
//                    throw new AppException("Username " + userParam.Username + " is already taken");
//            }

//            // update user properties
//            user.FirstName = userParam.FirstName;
//            user.LastName = userParam.LastName;
//            user.Username = userParam.Username;

//            // update password if it was entered
//            if (!string.IsNullOrWhiteSpace(password))
//            {
//                byte[] passwordHash, passwordSalt;
//                CreatePasswordHash(password, out passwordHash, out passwordSalt);

//                user.PasswordHash = passwordHash;
//                user.PasswordSalt = passwordSalt;
//            }

//            _dbContext.Users.Update(user);
//            _dbContext.SaveChanges();
//        }

//        public void Delete(int id)
//        {
//            var user = _dbContext.Users.Find(id);
//            if (user != null)
//            {
//                _dbContext.Users.Remove(user);
//                _dbContext.SaveChanges();
//            }
//        }

//        // private helper methods

//        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
//        {
//            if (password == null) throw new ArgumentNullException("password");
//            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

//            using (var hmac = new System.Security.Cryptography.HMACSHA512())
//            {
//                passwordSalt = hmac.Key;
//                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//            }
//        }

//        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
//        {
//            if (password == null) throw new ArgumentNullException("password");
//            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
//            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
//            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

//            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
//            {
//                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//                for (int i = 0; i < computedHash.Length; i++)
//                {
//                    if (computedHash[i] != storedHash[i]) return false;
//                }
//            }

//            return true;
//        }
//    }
//}
