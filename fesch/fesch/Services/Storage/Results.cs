using fesch.Services.Storage.Result;
using fesch.Services.Storage.Scheduler;
using System.Collections.Generic;

namespace fesch.Services.Storage
{
    public class Results
    {
        private static Results instance = null;
        public static Results Service
        {
            get
            {
                if (instance == null)
                {
                    instance = new Results();
                }
                return instance;
            }
        }
        public List<Finalexam> Finalexams { get; set; }
        private Results()
        {
            Finalexams = new List<Finalexam>();
            int F = Attendants.Service.Finalexams.GetLength(0);
            int O = Attendants.Service.Finalexams.GetLength(1);
            for (int f = 0; f < F; f++)
            {
                for (int o = 0; o < O; o++)
                {
                    Finalexams.Add(new Finalexam(Attendants.Service.Finalexams[f, o]));
                }
            }
            /// dispose
            //[TODO] Attendants
            //[TODO] DataModels
        }
    }
}
