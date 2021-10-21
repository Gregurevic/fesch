using fesch.Services.Storage.CustomEnums;
using System.Collections.Generic;

namespace fesch.Services.Storage.Scheduler.AttendantsModel
{
    public class AttendantInstructor
    {
        public int Id { get; set; }
        public int DataModelsId { get; set; }
        public int FlattenedId { get; set; }
        public bool Member { get; set; }
        public List<Tution> Tutions { get; set; }
        public bool FirstExaminer { get; set; }
        public bool SecondExaminer { get; set; }
        private AttendantInstructor() { }
        public AttendantInstructor(int id, int dataModelsId, bool member, List<Tution> tutions, bool firstExaminer, bool secondExaminer)
        {
            Id = id;
            DataModelsId = dataModelsId;
            Member = member;
            Tutions = tutions;
            FirstExaminer = firstExaminer;
            SecondExaminer = secondExaminer;
        }
    }
}
