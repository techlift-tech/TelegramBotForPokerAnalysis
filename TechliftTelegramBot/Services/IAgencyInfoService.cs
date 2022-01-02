using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TechliftTelegramBot.Models;

namespace TechliftTelegramBot.Services
{
    public interface IAgencyInfoService
    {
        public Task<Agency> GetAgency(int telegramId);
    }
}
