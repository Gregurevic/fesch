using fesch.Services.Storage.Scheduler;
using Gurobi;

namespace fesch.Services.Scheduler.ExamStructure
{
    static class Variables
    {
        public static GRBVar[,,] iGRB;
        public static void Set(GRBModel model)
        {
            /// dimensions
            int I = Structure.Service.Instructors.Count;
            int D = Structure.Service.DimensionD;
            int C = Structure.Service.DimensionC;
            /// GRB Vars
            iGRB = new GRBVar[I, D, C];
            /// init
            for (int i = 0; i < I; i++)
            {
                for (int d = 0; d < D; d++)
                {
                    for (int c = 0; c < C; c++)
                    {
                        iGRB[i, d, c] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "i_" + i + "_" + d + "_" + c);
                    }
                }
            }
        }
    }
}
