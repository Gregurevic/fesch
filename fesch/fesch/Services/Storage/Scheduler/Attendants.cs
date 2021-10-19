using fesch.Services.Storage.CustomAttributes;
using fesch.Services.Storage.CustomEnums;
using fesch.Services.Storage.DataModel;
using fesch.Services.Storage.Scheduler.AttendantsModel;
using fesch.Services.Storage.Scheduler.StructureModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fesch.Services.Storage.Scheduler
{
    class Attendants : IDisposable
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
        public AttedantsFinalexam[,] Finalexams { get; set; }
        public int DimensionF { get; set; }
        public int DimensionS { get; set; }
        public int DimensionOS { get; set; } /// ordinal short dimension
        public int DimensionOL { get; set; } /// ordinal long dimension
        public int DimensionI { get; set; }
        public int DimensionD { get; set; }
        public int SMEFlattenedLength { get; set; }
        private Attendants()
        {
            /// init attributes
            Fragments = new List<AttendantFragment>();
            Students = new List<AttendantStudent>();
            /// Students
            foreach (Student s in DataModels.Service.Students)
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
                    s.Tution == Tution.BPRO,
                    shortExam,
                    DataModels.Service.Instructors.Find(i => i.Neptun.Match(s.Supervisor)).Id,
                    courses
                ));
            }
            /// Fragments
            int idx = 0;
            foreach (StructureFragment sf in Structures.Service.Fragments)
            {
                List<CourseNeptun> courses = new List<CourseNeptun>();
                courses.AddRange(DataModels.Service.Instructors[sf.PresidentId].Courses);
                courses.AddRange(DataModels.Service.Instructors[sf.SecretaryId].Courses);
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
            Availability = new bool[DataModels.Service.Instructors.Count, DataModels.Service.Instructors[0].Availability.Count];
            for (int i = 0; i < DataModels.Service.Instructors.Count; i++)
            {
                for (int a = 0; a < DataModels.Service.Instructors[0].Availability.Count; a++)
                {
                    Availability[i, a] = DataModels.Service.Instructors[i].Availability[a];
                }
            }
            /// SME stands for student-member-examiner-matrix
            SME = new AttendantInstructor[Students.Count][];
            List<Instructor> members = DataModels.Service.Instructors.FindAll(i => i.Member);
            for (int s = 0; s < Students.Count; s++)
            {
                Student student = DataModels.Service.Students[s];
                List<Instructor> examiners = DataModels.Service.Instructors.FindAll(
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
            DimensionI = DataModels.Service.Instructors.Count;
            DimensionD = Structures.Service.DimensionD;
            /// free up memory space by terminating Storage/Structures.cs
            Structures.Service.Dispose();
            /// Finalexams init
            Finalexams = new AttedantsFinalexam[DimensionF, DimensionOS];
        }

        /// IDISPOSEABLE
        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                Students = null;
                Fragments = null;
                Availability = null;
                SME = null;
                Finalexams = null;
            }
            _disposed = true;
        }
        ~Attendants()
        {
            Dispose(false);
        }
    }
}
