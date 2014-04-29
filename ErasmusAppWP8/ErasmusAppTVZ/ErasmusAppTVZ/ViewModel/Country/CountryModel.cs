using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace ErasmusAppTVZ.ViewModel.Country
{
    public class CountryModel : IDisposable
    {
        public List<CountryData> Countries { get; set; }
        public bool IsDataLoaded { get; set; }

        public void Dispose()
        {
            Countries = null;
        }

        //public void LoadData()
        //{
        //    Countries = CreateCountyData();
        //    IsDataLoaded = true;
        //}

        //private List<CountryData> CreateCountyData()
        //{
        //    List<CountryData> countryDataList = new List<CountryData>();
        //    Random rand = new Random();

        //    for (int i = 0; i < 20; i++)
        //    {
        //        CountryData country = new CountryData() 
        //        {
        //            Name = "Country " + i,
        //            Rating = i % 10 < 6 ? i % 10 : (10 - (i % 10)),
        //        };

        //        countryDataList.Add(country);
        //    }

        //    return countryDataList;
        //}

    }//class
}//namespace
