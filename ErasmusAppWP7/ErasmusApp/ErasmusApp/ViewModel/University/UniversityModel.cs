using System.Collections.Generic;

namespace ErasmusApp.ViewModel.University
{
    class UniversityModel
    {
        public List<UniversityData> Universities { get; set; }
        public bool isDataLoaded { get; set; }

        public void LoadData()
        {
            Universities = CreateUniversityData();
            isDataLoaded = true;
        }

        private List<UniversityData> CreateUniversityData()
        {
            List<UniversityData> universityDataList = new List<UniversityData>();

            for (int i = 0; i < 20; i++)
            {
                UniversityData universityData = new UniversityData() 
                {
                    Name = "University name " + i
                };

                universityDataList.Add(universityData);
            }

            return universityDataList;
        }
    }
}
