using fesch.Services.Storage.Scheduler;
using Gurobi;

namespace fesch.Services.Scheduler.ExamAttendants
{
    static class Objective
    {
        /// dimensions
        private static int S = Variables.S;
        private static int I = Variables.I;
        private static int D = Variables.D;
        private static int C = Variables.C;

        public static void Set(GRBModel model)
        {
            model.SetObjective(
                InstructorCount() +
                InstructorAvailability(), 
                GRB.MINIMIZE);
        }

        private static GRBLinExpr InstructorCount()
        {
            int penaltyScore = 10;
            GRBLinExpr penalty = 0;
            for (int i = 0; i < I; i++)
            {
                for (int d = 0; d < D; d++)
                {
                    for (int c = 0; c < C; c++)
                    {
                        penalty.AddTerm(1, Variables.iGRB[i, d, c]);
                    }
                }
            }
            return penaltyScore * penalty;
        }

        private static GRBQuadExpr InstructorAvailability()
        {
            int penaltyScore = 1000;
            GRBQuadExpr penalty = 0;
            for (int d = 0; d < D; d++)
            {
                for (int c = 0; c < C; c++)
                {
                    for (int i = 0; i < I; i++)
                    {
                        for(int s = 0; s < S; s++)
                        {
                            /// calculate if instructor[i] is available at a given ordinal[s]
                            GRBQuadExpr available = (Attendants.Service.Students[s].Short) ?
                                Variables.MultiplyAt(Variables.SAM, Variables.ordinal[s], i, d) :
                                Variables.MultiplyAt(Variables.LAM, Variables.ordinal[s], i, d);
                            /// both student[s] and instructor[i] are present on given 'd' day
                            GRBQuadExpr matching = Variables.iGRB[i, d, c] * Variables.sGRB[s, d, c];
                            /// penalty if they both need to be present, but instructor[i] is unavailable
                            penalty.Add(matching - available);
                        }
                    }
                }
            }
            return penaltyScore * penalty;
        }
    }
}
