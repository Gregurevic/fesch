using fesch.Services.Storage.CustomAttributes;
using System.Collections.Generic;

namespace fesch.Services.Storage.Scheduler.AttendantsModel
{
    public class AttendantInstructor
    {
        public int Id { get; set; }
        public bool Member { get; set; }
        public List<bool> Availability { get; set; }
        public List<CourseNeptun> Courses { get; set; }
        private AttendantInstructor() { }
        public AttendantInstructor(int id, bool member, List<bool> availability, List<CourseNeptun> courses)
        {
            Id = id;
            Member = member;
            Availability = availability;
            Courses = courses;
        }
    }
}
