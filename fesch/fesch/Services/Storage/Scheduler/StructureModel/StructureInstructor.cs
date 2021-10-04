using fesch.Services.Storage.CustomEnums;
using System.Collections.Generic;

namespace fesch.Services.Storage.Scheduler.StructureModel
{
    public class StructureInstructor
    {
        public int Id { get; set; }
        public bool President { get; set; }
        public bool Secretary { get; set; }
        public List<Tution> Tutions { get; set; }
        public List<bool> Presence { get; set; }
        public StructureInstructor(int id, bool president, bool secretary, List<Tution> tutions, List<bool> presence)
        {
            Id = id;
            President = president;
            Secretary = secretary;
            Tutions = tutions;
            Presence = presence;
        }
    }
}
