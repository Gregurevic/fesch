using fesch.Services.Storage.Scheduler;
using Gurobi;

namespace fesch.Services.Scheduler.ExamAttendants
{
    static class Objective
    {
        /// dimensions
        private static int S = Variables.S;
        private static int F = Variables.F;

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
            for (int s = 0; s < S; s++)
            {
                for (int f = 0; f < F; f++)
                {
                    for (int o = 0; o < Variables.sor[s].Length; o++)
                    {
                        for(int me = 0; me < Variables.sme[s].Length; me++)
                        {
                            /// calculate if instructor[i] is available at a given ordinal[s]
                            GRBQuadExpr available = Variables.MultiplyAt(
                                Attendants.Service.Students[s].Short ? Variables.SAM : Variables.LAM, 
                                Variables.sor[s],
                                Attendants.Service.SME[s][me].DataModelsId,
                                Attendants.Service.Fragments[f].Day);
                            /// if student[s] instructor[me -> .Id] association exists, ergo sme[s][me] true
                            /// add penalty if instructor[me -> .Id] is unavailable
                            penalty.Add(Variables.sme[s][me] - available); //EZ ITT MÉG NEM JÓ, ÉDES ISTENEM, MIÉRT?!
                        }
                    }
                }
            }
            return penaltyScore * penalty;
        }
    }
}
