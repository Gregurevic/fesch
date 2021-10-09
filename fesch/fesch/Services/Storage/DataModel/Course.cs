using fesch.Services.Storage.CustomAttributes;

namespace fesch.Services.Storage.DataModel
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CourseNeptun Neptun { get; set; }
        private Course() { }
        public Course(int id, string name, string neptun)
        {
            Id = id;
            Name = name;
            Neptun = new CourseNeptun(neptun);
        }
    }
}
