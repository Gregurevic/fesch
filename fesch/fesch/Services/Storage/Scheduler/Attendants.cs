using fesch.Services.Storage.Scheduler.AttendantsModel;
using System.Collections.Generic;

namespace fesch.Services.Storage.Scheduler
{
    class Attendants
    {
        private static Attendants instance = null;
        public static Attendants Service
        {
            get
            {
                if (instance == null)
                {
                    instance = new Attendants();
                }
                return instance;
            }
        }
        private Attendants() {}
        public List<Fragment> Fragments { get; set; }
    }
}
