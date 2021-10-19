namespace fesch.Services.Storage.Scheduler.AttendantsModel
{
    public class AttedantsFinalexam
    {
        public int Id { get; set; }
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
        private AttedantsFinalexam() { }
        public AttedantsFinalexam(int id, string summary, string sequence, string time, string studentName, string studentNeptun, string tutionLevelLanguage, string supervisorName, string courses, string faculty, string examiners, string president, string member, string external, string secretary)
        {
            Id = id;
            Summary = summary;
            Sequence = sequence;
            Time = time;
            StudentName = studentName;
            StudentNeptun = studentNeptun;
            TutionLevelLanguage = tutionLevelLanguage;
            SupervisorName = supervisorName;
            Courses = courses;
            Faculty = faculty;
            Examiners = examiners;
            President = president;
            Member = member;
            External = external;
            Secretary = secretary;
        }
    }
}
