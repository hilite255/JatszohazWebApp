using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.RepositoryInterfaces
{
    public interface IGameRepo
    {
        public Task<ICollection<Game>> List();
        public Task<Game> FindById(int id);
        public Task<Game> Insert(Game game);
        public Task Delete(int id);
    }
}
