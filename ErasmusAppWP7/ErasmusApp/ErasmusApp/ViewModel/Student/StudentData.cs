using System.Collections.Generic;

namespace ErasmusApp.ViewModel.Student
{
    public enum SocialNetworks
    {
        Facebook = 1,
        Twitter = 2,
        GooglePlus = 3
    }

    class StudentData
    {
        public string Name { get; set; }
        public string University { get; set; }
        public int Age { get; set; }
        public char Sex { get; set; }
        public string Email { get; set; }
        public Dictionary<SocialNetworks, string> SocialContacts;
        public List<string> Languages { get; set; }
        public List<string> Interests { get; set; }
    }
}
