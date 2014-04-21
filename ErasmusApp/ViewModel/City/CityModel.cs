using System;
using System.Collections.Generic;


namespace ErasmusApp.ViewModel.City
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
                };

                cityDataList.Add(city);
            }

            return cityDataList;
        }
    }
}
