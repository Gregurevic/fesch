using fesch.Services.Storage.CustomEnums;

namespace fesch.Services.Storage.Scheduler.AttendantsModel
{
    class AttendantFragment
    {
        public int Id { get; set; }
        public Tution Tution { get; set; }
        public int Day { get; set; }
        public int Chamber { get; set; }
        public int PresidentId { get; set; }
        public int SecretaryId { get; set; }
        private AttendantFragment() { }
        public AttendantFragment(int id, Tution tution, int day, int chamber, int presidentId, int secretaryId)
        {
            Id = id;
            Tution = tution;
            Day = day;
            Chamber = chamber;
            PresidentId = presidentId;
            SecretaryId = secretaryId;
        }
    }
}
