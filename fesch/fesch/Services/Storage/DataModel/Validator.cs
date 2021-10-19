using fesch.Services.Exceptions;
using fesch.Services.Storage.CustomAttributes;
using fesch.Services.Storage.CustomEnums;
using System.Collections.Generic;
using System.Linq;

namespace fesch.Services.Storage.DataModel
{
    public static class Validator
    {
        public static void Check()
        {
            NoNullAttributes();
            AvailabilitySizeMatch();
            EachCourseHasExaminer();
            EachNeptunIsUnique();
            EachCourseNeptunIsUnique();
            SupervisorExists();
        }

        private static void NoNullAttributes()
        {
            if (DataModels.Service.DayCount == null ||
                DataModels.Service.TimeLimit == null ||
                DataModels.Service.FirstDay == null ||
                DataModels.Service.Instructors == null ||
                DataModels.Service.Students == null ||
                DataModels.Service.Courses == null)
                throw new ValidationException("some attributes are set to null");
        }

        private static void AvailabilitySizeMatch()
        {
            int cnt = DataModels.Service.Instructors[0].Availability.Count;
            foreach (Instructor i in DataModels.Service.Instructors)
            {
                if (i.Availability.Count != cnt) throw new ValidationException("instructor availability size mismatch");
            }
        }

        private static void EachCourseHasExaminer()
        {
            foreach (Student s in DataModels.Service.Students)
            {
                if (DataModels.Service.Instructors.FindAll(i => i.Courses.Contains(s.FirstCourse)).Count == 0)
                    throw new ValidationException("no examiner exists for course: " + s.FirstCourse.Code);
                if ((s.Language == Language.ENG || s.Level == Level.MSC) && DataModels.Service.Instructors.FindAll(i => i.Courses.Contains(s.SecondCourse)).Count == 0)
                    throw new ValidationException("no examiner exists for course: " + s.SecondCourse.Code);
            }
        }

        private static void EachNeptunIsUnique()
        {
            List<Neptun> neptuns = new List<Neptun>();
            foreach (Student s in DataModels.Service.Students)
            {
                neptuns.Add(s.Neptun);
            }
            foreach (Instructor i in DataModels.Service.Instructors)
            {
                neptuns.Add(i.Neptun);
            }
            if (neptuns.Distinct().Count() != neptuns.Count)
                throw new ValidationException("student and instructor neptun codes are not unique");
        }

        private static void EachCourseNeptunIsUnique()
        {
            List<CourseNeptun> courseNeptuns = new List<CourseNeptun>();
            foreach (Course c in DataModels.Service.Courses)
            {
                courseNeptuns.Add(c.Neptun);
            }
            if (courseNeptuns.Distinct().Count() != courseNeptuns.Count)
                throw new ValidationException("course neptun codes are not unique");
        }

        private static void SupervisorExists()
        {
            foreach (Student s in DataModels.Service.Students)
            {
                if (DataModels.Service.Instructors.FindAll(i => i.Neptun.Match(s.Supervisor)).Count == 0)
                    throw new ValidationException("student supervisor neptun matches no instructor neptun");
            }
        }
    }
}
