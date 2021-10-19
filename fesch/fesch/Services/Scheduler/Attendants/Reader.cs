using fesch.Services.Storage;
using fesch.Services.Storage.CustomEnums;
using fesch.Services.Storage.Scheduler;
using fesch.Services.Storage.Scheduler.AttendantsModel;

namespace fesch.Services.Scheduler.ExamAttendants
{
    static class Reader
    {
        private static int S = Variables.S;
        private static int F = Variables.F;
        private static int OS = Variables.OS;
        private static int OL = Variables.OL;
        public static void Get()
        {
            for (int f = 0; f < F; f++)
            {
                /// HELPER VARIABLES
                /// summary
                string summary = DataModels.Service.FirstDay.AddDays(Attendants.Service.Fragments[f].Day).ToString("yyyy. mm. dd. (dddd)");
                summary += " Terem" + Attendants.Service.Fragments[f].Chamber + " ";
                summary += Attendants.Service.Fragments[f].Tution.ToString();
                for (int o = 0; o < OS; o++)
                {
                    for (int s = 0; s < S; s++)
                    {
                        if (o < Variables.sor[s].Length)
                        {
                            if (Variables._constraint_no_duplicate_ordinals[f * OS * S + o * S + s].X == 1)
                            {
                                /// HELPER VARIABLES
                                /// time
                                int time = 8 * 60;
                                time += Attendants.Service.Students[s].Short ? 60 : 50;
                                time += o * (Attendants.Service.Students[s].Short ? 40 : 50);
                                time += Attendants.Service.Students[s].Short ? (o >= 6 ? 50 : 0 ) : (o >= 5 ? 50 : 0);
                                /// courses
                                string courses = DataModels.Service.getCourses().FindAll(c => c.Neptun.Match(DataModels.Service.getStudents()[s].FirstCourse))[0].Name;
                                if (!Attendants.Service.Students[s].Short) courses += ", " + DataModels.Service.getCourses().FindAll(c => c.Neptun.Match(DataModels.Service.getStudents()[s].SecondCourse))[0].Name;
                                /// member and examiners
                                string member = "";
                                string firstExaminer = "";
                                string secondExaminer = "";
                                for (int me = 0; me < Variables.sme[s].Length; me++)
                                {
                                    if (Variables.sme[s][me].X == 1)
                                    {
                                        if (Attendants.Service.SME[s][me].Member)
                                        {
                                            member = DataModels.Service.getInstructor(Attendants.Service.SME[s][me].DataModelsId).Name;
                                        }
                                        if (Attendants.Service.SME[s][me].FirstExaminer)
                                        {
                                            firstExaminer = DataModels.Service.getInstructor(Attendants.Service.SME[s][me].DataModelsId).Name;
                                        }
                                        if (!Attendants.Service.Students[s].Short && Attendants.Service.SME[s][me].SecondExaminer)
                                        {
                                            secondExaminer = DataModels.Service.getInstructor(Attendants.Service.SME[s][me].DataModelsId).Name;
                                        }
                                    }
                                }
                                string examiners = firstExaminer;
                                if (!Attendants.Service.Students[s].Short) examiners += ", " + secondExaminer;
                                /// CONSTRUCT FINALEXAM
                                Attendants.Service.Finalexams[f, o] = new AttedantsFinalexam(
                                    f * OS + o, 
                                    summary, 
                                    "_" + (o + 1).ToString(),
                                    (time / 60).ToString() + ":" + (time % 60).ToString(),
                                    DataModels.Service.getStudents()[s].Name,
                                    DataModels.Service.getStudents()[s].Neptun.ToString(), 
                                    CustomEnumConverter.FromCombination(DataModels.Service.getStudents()[s].Tution, DataModels.Service.getStudents()[s].Level, DataModels.Service.getStudents()[s].Language),
                                    DataModels.Service.getInstructors().FindAll(i => i.Neptun.Match(DataModels.Service.getStudents()[s].Supervisor))[0].Name, 
                                    courses, 
                                    "", 
                                    examiners,
                                    DataModels.Service.getInstructor(Attendants.Service.Fragments[f].PresidentId).Name, 
                                    member, 
                                    "",
                                    DataModels.Service.getInstructor(Attendants.Service.Fragments[f].SecretaryId).Name
                                );
                            }
                        }
                    }
                    /// if no finalexam has been scheduled at given ordnial
                    /// fill it with empty data
                    if (Attendants.Service.Finalexams[f, o] == null)
                    {
                        Attendants.Service.Finalexams[f, o] = new AttedantsFinalexam(
                            f * OS + o, 
                            summary,
                            "_" + (o + 1).ToString(), 
                            "", "", "", "", "", "", "", "", "", "", "", ""
                        );
                    }
                }
            }
        }
    }
}
