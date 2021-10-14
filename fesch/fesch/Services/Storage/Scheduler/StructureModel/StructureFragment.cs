using fesch.Services.Storage.CustomEnums;

namespace fesch.Services.Storage.Scheduler.StructureModel
{
    public class StructureFragment
    {
        public int Id { get; set; }
        public int DayIndex { get; set; }
        public int ChamberIndex { get; set; }
        public int PresidentId { get; set; }
        public int SecretaryId { get; set; }
        public Tution Tution { get; set; }
        private StructureFragment() { }
        public StructureFragment(int id, int dayIndex, int chamberIndex, int presidentId, int secretaryId, Tution tution)
        {
            Id = id;
            DayIndex = dayIndex;
            ChamberIndex = chamberIndex;
            PresidentId = presidentId;
            SecretaryId = secretaryId;
            Tution = tution;
        }
    }
}
