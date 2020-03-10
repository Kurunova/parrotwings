using System.Collections.Generic;
using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUserService
    {
        bool Register(User user);

        bool CanUserSignIn(string email, string password);

        User GetUser(string email, string password);

        IEnumerable<User> GetUserByPartOfName(string partOfName, string excludeName);

        IEnumerable<User> GetUserTop(int count, string excludeName);
    }
}