using Microsoft.EntityFrameworkCore;
using OA.Data;
using OA.Repo;
using System.Collections.Generic;

namespace OA.Service
{
    public class UserService : IUserService
    {
        private IRepository<User> _userRepository;
        private IRepository<UserProfile> _userProfileRepository;

        public UserService(IRepository<User> userRepository, IRepository<UserProfile> userProfileRepository)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
        }

        public IEnumerable<User> GetUsers()
        {
            return _userRepository.GetAll();
        }

        public User GetUser(long id)
        {
            return _userRepository.Get(id);
        }

        public User GetUserByUsername(string username)
        {
            return _userRepository
            .Query()
            .Include(u => u.Profile)
            .FirstOrDefault(u => u.UserName == username);
        }

        public void InsertUser(User user)
        {
            _userRepository.Insert(user);
        }
        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }

        public void DeleteUser(long id)
        {
            //UserProfile userProfile = _userProfileRepository.Get(id);
            //_userProfileRepository.Remove(userProfile);
            User user = GetUser(id);
            _userRepository.Remove(user);
            _userRepository.SaveChanges();
        }
    }
}