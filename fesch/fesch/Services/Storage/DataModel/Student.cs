using fesch.Services.Storage.CustomAttributes;
using fesch.Services.Storage.CustomEnums;
using System;

namespace fesch.Services.Storage.DataModel
{
    public class Student : Unit
    {
        public Level Level { get; set; }
        public Language Language { get; set; }
        public Tution Tution { get; set; }
        public Neptun Supervisor { get; set; }
        public CourseNeptun FirstCourse { get; set; }
        public CourseNeptun SecondCourse { get; set; }
        public Student(int id, string name, string neptun, string level, string language, string tution, string supervisor, string firstCourse, string secondCourse) : base(id, name, neptun)
        {
            Level = (Level)Enum.Parse(typeof(Level), level, true);
            Language = (Language)Enum.Parse(typeof(Language), language, true);
            Tution = (Tution)Enum.Parse(typeof(Tution), tution, true);
            Supervisor = new Neptun(supervisor);
            FirstCourse = new CourseNeptun(firstCourse);
            if (Language == Language.angol || Level == Level.MSc)
            {
                SecondCourse = new CourseNeptun(secondCourse);
            }
        }
    }
}
