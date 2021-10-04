using fesch.Services.Storage.Scheduler;
using Gurobi;
using System;

namespace fesch.Services.Scheduler.ExamStructure
{
    static class Objective
    {
        public static void Set(GRBModel model)
        {
            model.SetObjective(
                EvenLoadDeviation!!!() +
                MinimizeChamnbers(), 
                GRB.MINIMIZE);
        }

        private static GRBLinExpr MinimizeChamnbers()
        {
            int penaltyScore = 1000;
            GRBLinExpr penalty = 0;
            int I = Variables.iGRB.GetLength(0);
            int D = Variables.iGRB.GetLength(1);
            int C = Variables.iGRB.GetLength(2);
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
            int penaltyScore = 10;
            int I = Variables.iGRB.GetLength(0);
            int D = Variables.iGRB.GetLength(1);
            int C = Variables.iGRB.GetLength(2);
            /// minden elnökre (és titkárra): SUM(X) - opt, ahol opt = "mennyi kell" / "összes", ahol "mennyi kell" = "napok száma"
            /// Lineáris kifejezésben viszont ki lehet emelni az "opt" konstanst, így a kifejezés az lesz, hogy ->
            /// minden_elnökre(SUM(X)) - D
            GRBLinExpr PresidentPenalty = -1 * D;
            GRBLinExpr SecretaryPenalty = -1 * D;
            for (int d = 0; d < D; d++)
            {
                for (int c = 0; c < C; c++)
                {
                    for (int i = 0; i < I; i++)
                    {
                        if (Structure.Service.Instructors[i].President)
                        {
                            PresidentPenalty.AddTerm(1, Variables.iGRB[i, d, c]);
                        }
                        if (Structure.Service.Instructors[i].President)
                        {
                            PresidentPenalty.AddTerm(1, Variables.iGRB[i, d, c]);
                        }
                    }
                }
            }
            return (PresidentPenalty + SecretaryPenalty )* penaltyScore;
        }
    }
}
