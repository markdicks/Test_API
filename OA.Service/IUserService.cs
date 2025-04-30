using OA.Data;
using System.Collections.Generic;

namespace OA.Service
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User GetUser(long id);
        User GetUserByUsername(string username);
        void InsertUser(User user);
        void UpdateUser(User user);
        void DeleteUser(long id);
    }
}