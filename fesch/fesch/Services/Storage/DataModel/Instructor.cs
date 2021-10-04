using fesch.Services.Storage.CustomAttributes;
using fesch.Services.Storage.CustomEnums;
using System.Collections.Generic;

namespace fesch.Services.Storage.DataModel
{
    public class Instructor : Unit
    {
        public bool President { get; set; }
        public bool Secretary { get; set; }
        public bool Member { get; set; }
        public List<Tution> Tutions { get; set; }
        public List<bool> Availability { get; set; }
        public List<CourseNeptun> Courses { get; set; }
        public Instructor(int id, string name, string neptun, bool president, bool secretary, bool member, List<Tution> tutions, List<bool> availability) : base(id, name, neptun)
        {
            President = president;
            Secretary = secretary;
            Member = member;
            Tutions = tutions;
            Availability = availability;
            Courses = new List<CourseNeptun>();
        }
        public void AddCourse(CourseNeptun courses)
        {
            Courses.Add(courses);
        }
    }
}
