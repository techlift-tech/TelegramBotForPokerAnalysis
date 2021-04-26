using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechliftTelegramBot.Models;

namespace TechliftTelegramBot.Services
{
    public interface IPlayerInfoService
    {
        public Task<List<Player>> GetPlayer(int agencyId);
    }
}
