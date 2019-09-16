using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace HouseService.DataLayer.Aurora
{
    public class HouseDataService : IHouseDataService
    {
        HouseDbContext dbContext = null;

        public HouseDataService()
        {
            dbContext = new HouseDbContext();
            this.dbContext.Database.EnsureCreated();
        }

        public async Task<string> Create(HouseDTO house)
        {
            dbContext.Add(house);
            await dbContext.SaveChangesAsync();
            return house.Id;
        }

        public async Task Delete(string Id)
        {
            var house = dbContext.Houses.FirstOrDefault(a => a.Id == Id);
            if (house != null)
            {
                dbContext.Remove(house);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<HouseDTO> GetHouse(string Id)
        {
            var house = await dbContext.Houses.Where(a => a.Id == Id).ToAsyncEnumerable().FirstOrDefault();

            return house;
        }

        public async Task<IEnumerable<HouseDTO>> ListHouses(string query = null, long? RecordIndex = null, long NoOfRecords = 10)
        {
            IQueryable<HouseDTO> houseQuery = null;

            if (query != null)

                houseQuery = dbContext.Houses.Where(a => a.Title.Contains(query) || a.Description.Contains(query));
            else
                houseQuery = dbContext.Houses;

            if (RecordIndex != null)
                houseQuery = houseQuery.Skip(Convert.ToInt32(RecordIndex.Value));

            houseQuery = houseQuery.Take(Convert.ToInt32(NoOfRecords));

            return await houseQuery.ToAsyncEnumerable().ToList();
        }

        public async Task Update(HouseDTO house)
        {
            dbContext.Entry(house).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
    }
}
