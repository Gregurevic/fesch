using fesch.Services.Storage.DataModel;
using System;
using System.Collections.Generic;

namespace fesch.Services.Storage
{
    public class DataModels
    {
        private static DataModels instance = null;
        public static DataModels Service
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataModels();
                }
                return instance;
            }
        }
        /// FirstDay: Storage/Structures.cs will correct this value
        /// according to the number of days needed to conduct the exams
        public DateTime FirstDay { get; set; }
        public bool TimeLimit { get; set; }
        public int DayCount { get; set; }
        /// parttakers of the finals
        public List<Course> Courses { get; set; }
        public List<Instructor> Instructors { get; set; }
        public List<Student> Students { get; set; }
        private DataModels()
        {
            Courses = new List<Course>();
            Instructors = new List<Instructor>();
            Students = new List<Student>();
        }
    }
}
