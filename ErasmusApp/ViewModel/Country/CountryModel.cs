using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace ErasmusApp.ViewModel.Country
{
    public class CountryModel
    {
        public List<CountryData> Countries { get; set; }
        public bool IsDataLoaded { get; set; }

        public void LoadData()
        {
            Countries = CreateCountyData();
            IsDataLoaded = true;
        }

        private List<CountryData> CreateCountyData()
        {
            List<CountryData> countryDataList = new List<CountryData>();
            Random rand = new Random();

            for (int i = 0; i < 20; i++)
            {
                CountryData country = new CountryData() 
                {
                    Name = "Country " + i,
                    Grade = i % 10 < 6 ? i % 10 : (10 - (i % 10)),
                    Flag = new SolidColorBrush(Color.FromArgb((byte)255, (byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255)))
                };

                countryDataList.Add(country);
            }

            return countryDataList;
        }

    }//class
}//namespace
