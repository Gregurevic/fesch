namespace fesch.Services.Storage.FinalExam
{
    public class FinalExam
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Date { get; set; }
        public string Room { get; set; }
        public string Seq { get; set; }
        public string Time { get; set; }
        public string Student { get; set; }
        public string Supervisor { get; set; }
        public string Course { get; set; }
        public string Faculty { get; set; }
        public string Examiner { get; set; }
        public string President { get; set; }
        public string Member { get; set; }
        public string External { get; set; }
        public string Secretary { get; set; }
        private FinalExam() { }
        public FinalExam(int id, string summary, string date, string room, string seq, string time, string student, string supervisor, string course, string faculty, string examiner, string president, string member, string external, string secretary)
        {
            Id = id;
            Summary = summary;
            Date = date;
            Room = room;
            Seq = seq;
            Time = time;
            Student = student;
            Supervisor = supervisor;
            Course = course;
            Faculty = faculty;
            Examiner = examiner;
            President = president;
            Member = member;
            External = external;
            Secretary = secretary;
        }
    }
}
