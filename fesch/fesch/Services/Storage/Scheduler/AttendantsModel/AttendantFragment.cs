using fesch.Services.Storage.CustomAttributes;
using fesch.Services.Storage.CustomEnums;
using System.Collections.Generic;

namespace fesch.Services.Storage.Scheduler.AttendantsModel
{
    class AttendantFragment
    {
        public int Id { get; set; }
        public Tution Tution { get; set; }
        public int Day { get; set; }
        public int Chamber { get; set; }
        public int PresidentId { get; set; }
        public int SecretaryId { get; set; }
        public List<CourseNeptun> Courses { get; set; }
        private AttendantFragment() { }
        public AttendantFragment(int id, Tution tution, int day, int chamber, int presidentId, int secretaryId, List<CourseNeptun> courses)
        {
            Id = id;
            Tution = tution;
            Day = day;
            Chamber = chamber;
            PresidentId = presidentId;
            SecretaryId = secretaryId;
            Courses = courses;
        }
    }
}
