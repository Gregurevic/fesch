using fesch.Services.Storage.CustomEnums;
using fesch.Services.Storage.Scheduler;
using fesch.Services.Storage.Scheduler.StructureModel;
using System.Collections.Generic;
using System.Linq;

namespace fesch.Services.Scheduler.Structure
{
    static class Reader
    {
        public static void Get()
        {
            int I = Variables.iGRB.GetLength(0);
            int D = Variables.iGRB.GetLength(1);
            int C = Variables.iGRB.GetLength(2);
            /// Fragment Id
            int index = 0;
            /// stores Fragment Ids for Tution adjustment
            List<int> adjustmentIndex = new List<int>();
            for (int d = 0; d < D; d++)
            {
                for (int c = 0; c < C; c++)
                {
                    bool fragmentUsed = false;
                    int iIndex1 = -1;
                    int iIndex2 = -1;
                    for (int i = 0; i < I; i++)
                    {
                        if (Variables.iGRB[i, d, c].X == 1)
                        {
                            fragmentUsed = true;
                            if (iIndex1 == -1) { iIndex1 = i; }
                            else { iIndex2 = i; }
                        }
                    }
                    if (fragmentUsed)
                    {
                        /// the tution can be defined by the instructors's tutions
                        /// or we store the undecided fragment's index for later adjustment
                        IEnumerable<Tution> FragmentTutions = from tution in 
                                               Structures.Service.Instructors[iIndex1].Tutions.
                                               Intersect(Structures.Service.Instructors[iIndex2].Tutions) 
                                               select tution;
                        Tution FragmentTution = Tution.INFO;
                        if (FragmentTutions.ToArray().Length == 1) { FragmentTution = FragmentTutions.ToArray()[0]; }
                            else { adjustmentIndex.Add(index); }
                        /// generate Fragment
                        Structures.Service.Fragments.Add(new StructureFragment(
                            index,
                            d,
                            c,
                            Structures.Service.Instructors[iIndex1].Id,
                            Structures.Service.Instructors[iIndex2].Id,
                            FragmentTution
                        ));
                        index++;
                    }
                }
            }
            int adjustment = Structures.Service.Fragments.FindAll(f => f.Tution == Tution.INFO).Count - Structures.Service.InfoFragmentCount;
            for (int a = 0; a < adjustment; a++)
            {
                Structures.Service.Fragments.Find(f => f.Id == adjustmentIndex[a]).Tution = Tution.VILL;
            }
        }
    }
}
