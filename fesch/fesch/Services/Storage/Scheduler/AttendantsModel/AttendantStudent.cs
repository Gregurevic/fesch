using fesch.Services.Storage.CustomAttributes;
using fesch.Services.Storage.CustomEnums;
using System.Collections.Generic;

namespace fesch.Services.Storage.Scheduler.AttendantsModel
{
    public class AttendantStudent
    {
        public int Id { get; set; }
        public Level Level { get; set; }
        public Language Language { get; set; }
        public Tution Tution { get; set; }
        public int SupervisorId { get; set; }
        public List<CourseNeptun> Courses { get; set; }
        private AttendantStudent() { }
        public AttendantStudent(int id, Level level, Language language, Tution tution, int supervisorId, List<CourseNeptun> courses)
        {
            Id = id;
            Level = level;
            Language = language;
            Tution = tution;
            SupervisorId = supervisorId;
            Courses = courses;
        }
    }
}
