using System;
using System.Collections.Generic;
using System.Text;

namespace HouseService.DataLayer
{
    public class HouseDTO
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string StreetAddress { get; set; }

        public string Postcode { get; set; }

        public double SizeInSquareFeet { get; set; }

        public int NoOfRooms { get; set; }

        public int NoOfGarage { get; set; }

    }
}
