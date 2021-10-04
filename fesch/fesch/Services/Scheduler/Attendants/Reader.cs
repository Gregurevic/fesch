using fesch.Services.IO;
using fesch.Services.Storage;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace fesch.Services.Scheduler.ExamAttendants
{
    static class Reader
    {
        public static void Get()
        {
            DateTime firstDay = Params.Service.GetExamDate();
            int I = Variables.iGRB.GetLength(0);
            int S = Variables.sGRB.GetLength(0);
            int T = Variables.sGRB.GetLength(1);
            int R = Variables.sGRB.GetLength(2);
            int counter = 0;
            for (int r = 0; r < R; r++)
            {
                for (int t = 0; t < T; t++)
                {
                    for (int s = 0; s < S; s++)
                    {
                        if (Variables.sGRB[s, t, r].X == 1)
                        {
                            /// present student
                            Student student = DataModels.Service.getStudent(s);
                            /// present instructors
                            List<Instructor> instructors = new List<Instructor>();
                            for (int i = 0; i < I; i++)
                            {
                                if (Variables.iGRB[i, t, r].X == 1)
                                {
                                    instructors.Add(DataModels.Service.getInstructor(i));
                                }
                            }
                            /// time of exam
                            DateTime tempDate = firstDay.AddDays((t / 10))
                                                        .AddMinutes(((t % 10) < 5) ? 9 * 60 + (t % 10) * 45 : 14 * 60 + ((t % 10) - 5) * 45);
                            /// student's courses
                            string courses = "";
                            foreach (int id in student.CourseIds)
                            {
                                courses = (String.IsNullOrEmpty(courses)) ? DataModels.Service.getCourse(id).Name 
                                                                          : courses + " && " + DataModels.Service.getCourse(id).Name;
                            }
                            /// courses' examiners
                            string examiners = "";
                            foreach (Instructor e in instructors.FindAll(x => x.CourseIds.Contains(student.CourseIds[0]) || (student.CourseIds.Count == 2 && x.CourseIds.Contains(student.CourseIds[1]))))
                            {
                                examiners = (String.IsNullOrEmpty(examiners)) ? e.Name : examiners + " && " + e.Name;
                            }
                            /// constructing the exam
                            DataModels.Service.addExam(new Exam(
                                counter++, //id
                                tempDate.ToString("yyyy.MM.d. (dddd)", CultureInfo.CreateSpecificCulture("hu-HU")) + " R" + r.ToString() + " " + student.Program, //summary
                                tempDate.ToString("yyyy.MM.d. (dddd)", CultureInfo.CreateSpecificCulture("hu-HU")), //date
                                "R" + r.ToString(), //room id
                                "S" + (((t % 10) == 0) ? 10 : (t % 10)).ToString(), //sequence number
                                tempDate.ToString("HH:mm", CultureInfo.CreateSpecificCulture("hu-HU")),
                                student.Name, //student name
                                DataModels.Service.getInstructor(student.SupervisorId).Name, //supervisor name
                                courses, //courses
                                "temp faculty", //faculty
                                examiners, //examiners
                                instructors.Find(x => x.President).Name, //president
                                instructors.Find(x => x.Member).Name, //member
                                "external", //external member
                                instructors.Find(x => x.Secretary).Name //secretary
                            ));
                        }
                    }
                }
            }
        }
    }
}
