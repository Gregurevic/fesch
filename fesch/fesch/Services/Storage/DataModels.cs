using fesch.Services.Storage.CustomAttributes;
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
        private List<Course> courses;
        private List<Instructor> instructors;
        private List<Student> students;
        private DataModels()
        {
            courses = new List<Course>();
            instructors = new List<Instructor>();
            students = new List<Student>();
            FirstDay = new DateTime(2021, 6, 14);
        }
        /// gányolás 100
        public DateTime FirstDay { get; set; }
        /// COURSES
        public List<Course> getCourses()
        {
            return courses;
        }
        public Course getCourse(int id)
        {
            return courses.Find(i => i.Id == id);
        }
        public void addCourse(Course course)
        {
            courses.Add(course);
        }
        /// INSTRUCTORS
        public List<Instructor> getInstructors()
        {
            return instructors;
        }
        public Instructor getInstructor(int id)
        {
            return instructors.Find(i => i.Id == id);
        }
        public void addInstructor(Instructor instructor)
        {
            instructors.Add(instructor);
        }
        public int PresidentCount()
        {
            return instructors.FindAll(x => x.President).Count;
        }
        public Instructor FindInstructor(Neptun neptun)
        {
            return instructors.Find(i => i.Neptun.Match(neptun));
        }
        /// STUDENTS
        public List<Student> getStudents()
        {
            return students;
        }
        public Student getStudent(int id)
        {
            return students.Find(i => i.Id == id);
        }
        public void addStudent(Student student)
        {
            students.Add(student);
        }
    }
}
