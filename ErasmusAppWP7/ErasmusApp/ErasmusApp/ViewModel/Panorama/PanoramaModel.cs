using ErasmusApp.ViewModel.University;
using ErasmusApp.ViewModel.Student;
using ErasmusApp.ViewModel.Event;

namespace ErasmusApp.ViewModel.Panorama
{
    class PanoramaModel
    {
        public UniversityModel University { get; set; }
        public StudentModel Student { get; set; }
        public EventModel Event { get; set; }

        public PanoramaModel()
        {
            University = new UniversityModel();
            Student = new StudentModel();
            Event = new EventModel();

            University.LoadData();
            Student.LoadData();
            Event.LoadData();
        }
    }
}
