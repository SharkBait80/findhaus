using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HouseService.DataLayer
{
    public interface IHouseDataService
    {
        Task<HouseDTO> GetHouse(string Id);

        Task<IEnumerable<HouseDTO>> ListHouses(string query=null, long? RecordIndex = null, long NoOfRecords = 10);

        Task<string> Create(HouseDTO house);

        Task Update(HouseDTO house);

        Task Delete(string Id);
    }
}
