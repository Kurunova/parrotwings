using BusinessLogicLayer.Models;

namespace ServiceLayer.Interfaces
{
    public interface IAuthorizationService
    {
        void Register(User user);

        void Login(User user);
    }
}