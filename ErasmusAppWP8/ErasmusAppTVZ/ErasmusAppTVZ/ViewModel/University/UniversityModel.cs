using System;
using System.Collections.Generic;

namespace ErasmusAppTVZ.ViewModel.University
{
    public class UniversityModel
    {
        public List<UniversityData> Universities { get; set; }
        public bool IsDataLoaded { get; set; }

        public static List<UniversityData> CreateUniversityData()
        {
            List<UniversityData> universityDataList = new List<UniversityData>();

            for (int i = 0; i < 20; i++)
            {
                UniversityData universityData = new UniversityData()
                {
                    Name = "University " + i
                };

                universityDataList.Add(universityData);
            }

            return universityDataList;
        }

    }//class
}//namespace
