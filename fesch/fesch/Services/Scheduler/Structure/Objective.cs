using fesch.Services.Storage.Scheduler;
using Gurobi;
using System;

namespace fesch.Services.Scheduler.Structure
{
    static class Objective
    {
        private static int I = Variables.iGRB.GetLength(0);
        private static int D = Variables.iGRB.GetLength(1);
        private static int C = Variables.iGRB.GetLength(2);
        public static void Set(GRBModel model)
        {
            model.SetObjective(
                MinimizeChamnbers() +
                EvenLoad() +
                Availability(), 
                GRB.MINIMIZE
            );
        }

        private static GRBLinExpr MinimizeChamnbers()
        {
            int penaltyScore = 10;
            GRBLinExpr penalty = 0;
            for (int i = 0; i < I; i++)
            {
                for (int d = 0; d < D; d++)
                {
                    /// for every extra chamber on each day for every president and secretary
                    /// c = 1;
                    for (int c = 1; c < C; c++)
                    {
                        penalty.AddTerm(Math.Pow(penaltyScore, c), Variables.iGRB[i, d, c]);
                    }
                }
            }
            return penalty;
        }

        private static GRBLinExpr EvenLoad()
        {
            int penaltyScore = 100;
            GRBLinExpr penalty = 0;
            int P = Structures.Service.Instructors.FindAll(i => i.President).Count;
            for (int p = 0; p < P; p++)
            {
                penalty.AddTerm(1 / P, Variables._objective_PresidentPositiveDeviation[p]);
                penalty.AddTerm(- 1 / P, Variables._objective_PresidentNegativeDeviation[p]);
            }
            int S = Structures.Service.Instructors.FindAll(i => i.Secretary).Count;
            for (int s = 0; s < S; s++)
            {
                penalty.AddTerm(1 / S, Variables._objective_SecretaryPositiveDeviation[s]);
                penalty.AddTerm(-1 / S, Variables._objective_SecretaryNegativeDeviation[s]);
            }
            return penalty *penaltyScore;
        }

        private static GRBLinExpr Availability()
        {
            int penaltyScore = 1000;
            GRBLinExpr penalty = 0;
            for (int i = 0; i < I; i++)
            {
                for (int d = 0; d < D; d++)
                {
                    for (int c = 0; c < C; c++)
                    {
                        if (!Structures.Service.Instructors[i].Presence[d])
                        {
                            penalty.AddTerm(1, Variables.iGRB[i, d, c]);
                        }
                    }
                }
            }
            return penalty * penaltyScore;
        }
    }
}
