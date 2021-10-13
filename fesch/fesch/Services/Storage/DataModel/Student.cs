using fesch.Services.Storage.CustomAttributes;
using fesch.Services.Storage.CustomEnums;

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
            Level = CustomEnumConverter.ToLevel(level);
            Language = CustomEnumConverter.ToLanguage(language);
            Tution = CustomEnumConverter.ToTution(tution);
            Supervisor = new Neptun(supervisor);
            FirstCourse = new CourseNeptun(firstCourse);
            if (Language == Language.ENG || Level == Level.MSC)
            {
                SecondCourse = new CourseNeptun(secondCourse);
            }
        }
    }
}
