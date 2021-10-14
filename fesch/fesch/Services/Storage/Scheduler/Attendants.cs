using fesch.Services.Storage.CustomAttributes;
using fesch.Services.Storage.CustomEnums;
using fesch.Services.Storage.DataModel;
using fesch.Services.Storage.Scheduler.AttendantsModel;
using System.Collections.Generic;

namespace fesch.Services.Storage.Scheduler
{
    class Attendants
    {
        private static Attendants instance = null;
        public static Attendants Service
        {
            get
            {
                if (instance == null)
                {
                    instance = new Attendants();
                }
                return instance;
            }
        }
        public List<TimeSlot> TimeSlots { get; set; }
        public List<AttendantStudent> Students { get; set; }
        public List<AttendantInstructor> Instructors { get; set; }
        public int DimensionD { get; set; }
        public int DimensionC { get; set; }
        public int DimensionS { get; set; }
        public int DimensionI { get; set; }
        public int DimensionOS { get; set; } /// ordinal short
        public int DimensionOL { get; set; } /// ordinal long
        private Attendants()
        {
            TimeSlots = new List<TimeSlot>();
            Students = new List<AttendantStudent>();
            Instructors = new List<AttendantInstructor>();
            // percben mérlyük 0:00-tól a Start Finnish-t, Tution = constraint, másik kettő = objective
            // másolunk mindent, amit lehet Fragmentből
            // i guess not

            /// Students
            foreach (Student s in DataModels.Service.getStudents())
            {
                /// courses
                List<CourseNeptun> courses = new List<CourseNeptun>();
                courses.Add(s.FirstCourse);
                bool shortExam = s.Language == Language.HUN && s.Level == Level.BSC;
                if (!shortExam) courses.Add(s.SecondCourse);
                /// construction
                Students.Add(new AttendantStudent(
                    s.Id,
                    s.Level,
                    s.Language,
                    s.Tution,
                    shortExam,
                    DataModels.Service.getInstructors().Find(i => i.Neptun.Match(s.Supervisor)).Id,
                    courses
                ));
            }
            /// instructors
            foreach (Instructor i in DataModels.Service.getInstructors())
            {
                Instructors.Add(new AttendantInstructor(
                    i.Id,
                    i.Member,
                    i.Availability,
                    i.Courses));
            }
            /// dimensions
            DimensionD = Structures.Service.DimensionD;
            DimensionC = Structures.Service.DimensionC;
            DimensionS = Students.Count;
            DimensionI = Instructors.Count;
            DimensionOS = 11;
            DimensionOL = 9;
        }
    }
}
