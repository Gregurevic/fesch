using fesch.Services.Storage.CustomEnums;

namespace fesch.Services.Storage.Scheduler.AttendantsModel
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public int Start { get; set; }
        public int Finnish { get; set; }
        public Level Level { get; set; }
        public Language Language { get; set; }
        public Tution Tution { get; set; }
        public int DayIndex { get; set; }
        public int ChamberIndex { get; set; }
        public int PresidentId { get; set; }
        public int SecretaryId { get; set; }
        private TimeSlot() { }
        public TimeSlot(int id, int start, int finnish, Level level, Language language, Tution tution, int dayIndex, int chamberIndex, int presidentId, int secretaryId)
        {
            Id = id;
            Start = start;
            Finnish = finnish;
            Level = level;
            Language = language;
            Tution = tution;
            DayIndex = dayIndex;
            ChamberIndex = chamberIndex;
            PresidentId = presidentId;
            SecretaryId = secretaryId;
        }
    }
}
