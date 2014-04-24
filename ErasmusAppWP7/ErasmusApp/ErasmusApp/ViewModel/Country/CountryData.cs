using System.Windows.Media;

namespace ErasmusApp.ViewModel.Country
{
    public class CountryData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Rating { get; set; }
        public SolidColorBrush Flag { get; set; }
    }
}
