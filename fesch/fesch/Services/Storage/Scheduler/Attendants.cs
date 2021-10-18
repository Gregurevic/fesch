using fesch.Services.Storage.CustomAttributes;
using fesch.Services.Storage.CustomEnums;
using fesch.Services.Storage.DataModel;
using fesch.Services.Storage.Scheduler.AttendantsModel;
using fesch.Services.Storage.Scheduler.StructureModel;
using System.Collections.Generic;
using System.Linq;

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
        public List<AttendantStudent> Students { get; set; }
        public List<AttendantFragment> Fragments { get; set; }
        public bool[,] Availability { get; set; }
        public AttendantInstructor[][] SME { get; set; }
        public int DimensionF { get; set; }
        public int DimensionS { get; set; }
        public int DimensionOS { get; set; } /// ordinal short dimension
        public int DimensionOL { get; set; } /// ordinal long dimension
        public int SMEFlattenedLength { get; set; }
        private Attendants()
        {
            /// init attributes
            Fragments = new List<AttendantFragment>();
            Students = new List<AttendantStudent>();
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
                    s.Language == Language.ENG,
                    s.Tution != Tution.BPRO ? s.Tution : Tution.INFO,
                    shortExam,
                    DataModels.Service.getInstructors().Find(i => i.Neptun.Match(s.Supervisor)).Id,
                    courses
                ));
            }
            /// Fragments
            int idx = 0;
            foreach (StructureFragment sf in Structures.Service.Fragments)
            {
                List<CourseNeptun> courses = new List<CourseNeptun>();
                courses.AddRange(DataModels.Service.getInstructor(sf.PresidentId).Courses);
                courses.AddRange(DataModels.Service.getInstructor(sf.SecretaryId).Courses);
                Fragments.Add(new AttendantFragment(
                    idx,
                    sf.Tution,
                    sf.DayIndex,
                    sf.ChamberIndex,
                    sf.PresidentId,
                    sf.SecretaryId,
                    courses.Distinct().ToList()
                ));
                idx++;
            }
            /// Availability
            Availability = new bool[DataModels.Service.getInstructors().Count, DataModels.Service.getInstructor(0).Availability.Count];
            for (int i = 0; i < DataModels.Service.getInstructors().Count; i++)
            {
                for (int a = 0; a < DataModels.Service.getInstructor(0).Availability.Count; a++)
                {
                    Availability[i, a] = DataModels.Service.getInstructor(i).Availability[a];
                }
            }
            /// SME stands for student-member-examiner-matrix
            SME = new AttendantInstructor[Students.Count][];
            List<Instructor> members = DataModels.Service.getInstructors().FindAll(i => i.Member);
            for (int s = 0; s < Students.Count; s++)
            {
                Student student = DataModels.Service.getStudent(s);
                List<Instructor> examiners = DataModels.Service.getInstructors().FindAll(
                    i => Students[s].Short ? i.Courses.Contains(student.FirstCourse) : (i.Courses.Contains(student.FirstCourse) || i.Courses.Contains(student.SecondCourse)));
                List<Instructor> meInstructors = new List<Instructor>();
                meInstructors.AddRange(members);
                meInstructors.AddRange(examiners);
                meInstructors = meInstructors.Distinct().ToList();
                SME[s] = new AttendantInstructor[meInstructors.Count];
                for (int me = 0; me < meInstructors.Count; me++)
                {
                    SME[s][me] = new AttendantInstructor(
                        s * meInstructors.Count + me,
                        meInstructors[me].Id,
                        meInstructors[me].Member,
                        meInstructors[me].Courses.Contains(Students[s].Courses[0]),
                        !Students[s].Short && meInstructors[me].Courses.Contains(Students[s].Courses[1])
                    );
                }
            }
            /// SME's FlattenedLength
            List<int> DataModelIds = new List<int>();
            for (int s = 0; s < Students.Count; s++)
            {
                for (int me = 0; me < SME[s].Length; me++)
                {
                    if (!DataModelIds.Contains(SME[s][me].DataModelsId)) DataModelIds.Add(SME[s][me].DataModelsId);
                }
            }
            SMEFlattenedLength = DataModelIds.Count;
            /// SME's flattened id
            for (int s = 0; s < Students.Count; s++)
            {
                for (int me = 0; me < SME[s].Length; me++)
                {
                    SME[s][me].FlattenedId = DataModelIds.IndexOf(SME[s][me].DataModelsId);
                }
            }
            /// Dimensions
            DimensionF = Fragments.Count;
            DimensionS = Students.Count;
            DimensionOS = 11; /// maximum 11 short exams can be conducted per fragment
            DimensionOL = 9; /// maximum 9 long exams can be conducted per fragment
            /// getter functions
            instructorCount = DataModels.Service.getInstructors().Count;
            dayCount = Structures.Service.DimensionD;
            /// free up memory space by terminating Scheduler-Structure-Storage
            //[TODO]
        }

        private int instructorCount;
        private int dayCount;

        public int GetI()
        {
            return instructorCount;
        }

        public int GetD()
        {
            return dayCount;
        }
    }
}
