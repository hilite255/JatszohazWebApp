using Domain.Models;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class UserService
    {
        private readonly IUserRepo userRepo;
        private readonly ITokenGenerator tokenGenerator;

        public UserService(IUserRepo userRepo, ITokenGenerator tokenGenerator)
        {
            this.userRepo = userRepo;
            this.tokenGenerator = tokenGenerator;
        }

        public async Task<bool> Register(string username, string password, string email)
        {
            var res = await userRepo.Create(username, password, email);
            return res.Succeeded;
        }

        public async Task<object> Login(string username, string password)
        {
            var user = await userRepo.Login(username, password);
            if (user == null)
                return null;
            return tokenGenerator.GenerateToken(user);
        }
    }

    public interface ITokenGenerator
    {
        object GenerateToken(User user);
    }
}
