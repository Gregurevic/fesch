using fesch.Services.IO;
using fesch.Services.Storage;
using Gurobi;

namespace fesch.Services.Scheduler.ExamAttendants
{
    static class Variables
    {
        public static GRBVar[,,] iGRB;
        public static GRBVar[,,] sGRB;
        public static void Set(GRBModel model)
        {
            /// dimensions
            int I = DataModels.Service.getInstructors().Count;
            int S = DataModels.Service.getStudents().Count;
            int R = Params.Service.getR();
            int T = Params.Service.getT();
            /// GRB Vars
            iGRB = new GRBVar[I, T, R];
            sGRB = new GRBVar[S, T, R];
            /// init
            for (int i = 0; i < I; i++)
            {
                for (int t = 0; t < T; t++)
                {
                    for (int r = 0; r < R; r++)
                    {
                        iGRB[i, t, r] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "i" + i + t + r);
                    }
                }
            }
            for (int s = 0; s < S; s++)
            {
                for (int t = 0; t < T; t++)
                {
                    for (int r = 0; r < R; r++)
                    {
                        sGRB[s, t, r] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "s" + s + t + r);
                    }
                }
            }
        }
    }
}
