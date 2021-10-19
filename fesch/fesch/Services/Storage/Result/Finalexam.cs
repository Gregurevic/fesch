using fesch.Services.Storage.Scheduler.AttendantsModel;

namespace fesch.Services.Storage.Result
{
    public class Finalexam
    {
        public string Summary { get; set; }
        public string Sequence { get; set; }
        public string Time { get; set; }
        public string StudentName { get; set; }
        public string StudentNeptun { get; set; }
        public string TutionLevelLanguage { get; set; }
        public string SupervisorName { get; set; }
        public string Courses { get; set; }
        public string Faculty { get; set; }
        public string Examiners { get; set; }
        public string President { get; set; }
        public string Member { get; set; }
        public string External { get; set; }
        public string Secretary { get; set; }
        private Finalexam() { }
        public Finalexam(AttedantsFinalexam afe)
        {
            Summary = afe.Summary;
            Sequence = afe.Sequence;
            Time = afe.Time;
            StudentName = afe.StudentName;
            StudentNeptun = afe.StudentNeptun;
            TutionLevelLanguage = afe.TutionLevelLanguage;
            SupervisorName = afe.SupervisorName;
            Courses = afe.Courses;
            Faculty = afe.Faculty;
            Examiners = afe.Examiners;
            President = afe.President;
            Member = afe.Member;
            External = afe.External;
            Secretary = afe.Secretary;
        }
    }
}
