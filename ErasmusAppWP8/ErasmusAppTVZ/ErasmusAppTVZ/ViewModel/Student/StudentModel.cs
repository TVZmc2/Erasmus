using System.Collections.Generic;

namespace ErasmusAppTVZ.ViewModel.Student
{
    class StudentModel
    {
        public List<StudentData> Students { get; set; }

        public static List<StudentData> CreateStudentData()
        {
            List<StudentData> studentDataList = new List<StudentData>();

            for (int i = 0; i < 2000; i++)
            {
                StudentData studentData = new StudentData() 
                {
                    Name = "Name " + i,
                    //University = "University " + i % 10,
                    Age = (i % 10) + 18
                };

                studentDataList.Add(studentData);
            }

            return studentDataList;
        }
    }
}
