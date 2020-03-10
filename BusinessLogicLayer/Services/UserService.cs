using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore.Internal;
using DBUser = DataAccessLayer.Entities.User;

namespace BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<DBUser> _userRepository;
        private readonly ITransactionService _transactionService;

        public UserService(IRepository<DBUser> userRepository, ITransactionService transactionService)
        {
            _userRepository = userRepository;
            _transactionService = transactionService;
        }

        public bool Register(User user)
        {
            var result = false;
            if (IsValid(user))
            {
                if (IsUserExistWithTheSameEmail(user.Email))
                {
                    throw new ConditionException("User with the same email already exist");
                }

                if (IsUserExistWithTheSameName(user.Name))
                {
                    throw new ConditionException("User with the same name already exist");
                }

                var dbUser = Map(user);
                _userRepository.Create(dbUser);
                user.Id = dbUser.Id;
                _transactionService.CreateStart(dbUser.Id);
                result = true;
            }
            return result;
        }

        public bool CanUserSignIn(string email, string password)
        {
            return _userRepository.Find(user => user.Email == email && user.Password == password).Any();
        }

        public User GetUser(string email, string password)
        {
            var dbUser = _userRepository.Find(user => user.Email == email && user.Password == password).FirstOrDefault();
            return AutoMapperConfiguration.Mapper.Map(dbUser, new User());
        }

        public IEnumerable<User> GetUserByPartOfName(string partOfName, string excludeName)
        {
            var dbUser = _userRepository.Find(u => u.Name.Contains(partOfName) && u.Name != excludeName).ToList();
            var users = dbUser.Select(u => AutoMapperConfiguration.Mapper.Map(u, new User())).ToList();
            return users;
        }

        public IEnumerable<User> GetUserTop(int count, string excludeName)
        {
            var dbUser = _userRepository.Find(u => u.Name != excludeName).Take(count).ToList();
            var users = dbUser.Select(u => AutoMapperConfiguration.Mapper.Map(u, new User()));
            return users;
        }

        private bool IsValid(User user)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(user);

            Validator.TryValidateObject(user, context, results, true);

            return !EnumerableExtensions.Any(results);
        }

        private bool IsUserExistWithTheSameEmail(string email)
        {
            return EnumerableExtensions.Any(_userRepository.Find(user => user.Email == email));
        }

        private bool IsUserExistWithTheSameName(string name)
        {
            return EnumerableExtensions.Any(_userRepository.Find(user => user.Name == name));
        }

        private DBUser Map(User user)
        {
            var dbUser = AutoMapperConfiguration.Mapper.Map(user, new DBUser());
            dbUser.CreatedAt = DateTime.Now;
            return dbUser;
        }
    }
}