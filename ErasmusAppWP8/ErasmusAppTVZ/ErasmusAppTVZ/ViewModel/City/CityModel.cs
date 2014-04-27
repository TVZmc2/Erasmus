using System;
using System.Linq;
using System.Collections.Generic;

namespace ErasmusAppTVZ.ViewModel.City
{
    public class CityModel
    {
        public List<CityData> Cities { get; set; }
        public bool isDataLoaded { get; set; }

        public void LoadData()
        {
            Cities = CreateCitiesGroup();
            isDataLoaded = true;
        }

        private List<CityData> CreateCitiesGroup()
        {
            List<CityData> cityDataList = new List<CityData>();
            Random rand = new Random();

            for (int i = 0; i < 20; i++)
            {
                CityData city = new CityData()
                {
                    Name = "City " + i,
                    Rating = i % 10 < 6 ? i % 10 : (10 - (i % 10))
                };

                cityDataList.Add(city);
            }

            //sorting by name descending
            IEnumerable<CityData> sorted;

            sorted = from item in cityDataList
                     orderby item.Name
                     select item;

            cityDataList = sorted.ToList();

            return cityDataList;
        }
    }
}
