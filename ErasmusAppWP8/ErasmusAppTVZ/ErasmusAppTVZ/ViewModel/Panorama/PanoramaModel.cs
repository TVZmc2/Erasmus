using ErasmusAppTVZ.ViewModel.University;
using ErasmusAppTVZ.ViewModel.Student;
using ErasmusAppTVZ.ViewModel.Event;
using System.Collections.Generic;

namespace ErasmusAppTVZ.ViewModel.Panorama
{
    class PanoramaModel
    {
        public List<UniversityData> Universities { get; set; }
        public List<StudentData> Students { get; set; }
        public List<EventData> Events { get; set; }

        public PanoramaModel()
        {
            Universities = UniversityModel.CreateUniversityData();
            Students = StudentModel.CreateStudentData();
            Events = EventModel.CreateEventData();
        }
    }
}
