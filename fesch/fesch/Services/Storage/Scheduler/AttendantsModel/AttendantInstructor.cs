namespace fesch.Services.Storage.Scheduler.AttendantsModel
{
    public class AttendantInstructor
    {
        public int Id { get; set; }
        public int DataModelsId { get; set; }
        public bool Member { get; set; }
        public bool Examiner { get; set; }
        private AttendantInstructor() { }
        public AttendantInstructor(int id, int dataModelsId, bool member, bool examiner)
        {
            Id = id;
            DataModelsId = dataModelsId;
            Member = member;
            Examiner = examiner;
        }
    }
}
