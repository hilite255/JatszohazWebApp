using Domain.Models;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class RentService
    {
        private readonly IRentRepo rentRepo;

        public RentService(IRentRepo rentRepo)
        {
            this.rentRepo = rentRepo;
        }
        
        public async Task<List<Game>> GetGamesInTimespan(DateTime from, DateTime to)
        {
            return await rentRepo.GetGamesInTimespan(from, to);
        }

        public async Task<Rent> CreateNewRent(string userId, DateTime from, DateTime to, List<Game> games)
        {
            var rent = await rentRepo.CreateNewRent(from, to, Rent.RentStatus.Pending, userId);
            foreach (var g in games)
            {
                await rentRepo.AddGameToRent(rent, g);
            }
            return rent;
        }

        public async Task<RentComment> AddCommentToRent(string userId, string comment, Rent rent)
        {
            return await rentRepo.AddCommentToRent(userId, comment, rent);
        }

        public async Task<List<Rent>> ListRents()
        {
            var rents = await rentRepo.List();
            return rents;
        }

        public List<Game> GetGamesForRent(Rent r)
        {
            if (r == null)
                return null;
            return rentRepo.GetGamesForRent(r);
        }

        public async Task<Rent> GetRentById(int id)
        {
            return await rentRepo.GetRentById(id);
        }

        public async Task<List<RentComment>> GetCommentsForRent(Rent r)
        {
            return await rentRepo.GetCommentsForRent(r);
        }

        public async void ChangeStatusOfRent(string status, Rent rent)
        {
            rentRepo.ChangeStatusOfRent(status, rent);
        }
    }
}
