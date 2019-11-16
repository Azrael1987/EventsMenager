using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Evento.Infrastructure.Services
{
    public interface IMenagmentService
    {
        Task UpdateConfiguration(string target);
        Task SetLoggLevel(int level);
    }
}
