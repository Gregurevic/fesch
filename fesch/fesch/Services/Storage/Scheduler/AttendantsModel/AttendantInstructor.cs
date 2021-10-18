namespace fesch.Services.Storage.Scheduler.AttendantsModel
{
    public class AttendantInstructor
    {
        public int Id { get; set; }
        public int DataModelsId { get; set; }
        public int FlattenedId { get; set; }
        public bool Member { get; set; }
        public bool FirstExaminer { get; set; }
        public bool SecondExaminer { get; set; }
        private AttendantInstructor() { }
        public AttendantInstructor(int id, int dataModelsId, bool member, bool firstExaminer, bool secondExaminer)
        {
            Id = id;
            DataModelsId = dataModelsId;
            Member = member;
            FirstExaminer = firstExaminer;
            SecondExaminer = secondExaminer;
        }
    }
}
