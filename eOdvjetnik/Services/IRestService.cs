using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eOdvjetnik.Models;

namespace eOdvjetnik.Services
{
    public interface IRestService
    {
        Task<List<DevInfoItem>> RefreshDataAsync();
        Task SaveDevInfoItemAsync(DevInfoItem devInfoItem, bool isNewItem);
        Task DeleteDevInfoItemAsync(string id);
    }
}
