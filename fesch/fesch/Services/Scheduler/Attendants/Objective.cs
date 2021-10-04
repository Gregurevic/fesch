using fesch.Services.Storage;
using Gurobi;

namespace fesch.Services.Scheduler.ExamAttendants
{
    static class Objective
    {
        public static void Set(GRBModel model)
        {
            // set model objective
            model.SetObjective(
                //InstructorsAreAvailable() + PresidentsAreAvailable() + 
                //SupervisorsAreAvailable() + Overtime() +
                OverallNumberOfInstructors(), 
                GRB.MINIMIZE);
        }

        private static GRBLinExpr InstructorsAreAvailable()
        {
            int penaltyScore = 1000;
            GRBLinExpr penalty = 0;
            int I = Variables.iGRB.GetLength(0);
            int T = Variables.iGRB.GetLength(1);
            int R = Variables.iGRB.GetLength(2);
            for (int i = 0; i < I; i++)
            {
                for (int t = 0; t < T; t++)
                {
                    for (int r = 0; r < R; r++)
                    {
                        if(!DataModels.Service.getInstructors()[i].Availability[t])
                        {
                            penalty.AddTerm(penaltyScore, Variables.iGRB[i, t, r]);
                        }
                    }
                }
            }
            return penalty;
        }
        private static GRBLinExpr PresidentsAreAvailable()
        {
            int penaltyScore = 9000;
            GRBLinExpr penalty = 0;
            int I = Variables.iGRB.GetLength(0);
            int T = Variables.iGRB.GetLength(1);
            int R = Variables.iGRB.GetLength(2);
            for (int i = 0; i < I; i++)
            {
                for (int t = 0; t < T; t++)
                {
                    for (int r = 0; r < R; r++)
                    {
                        if (!DataModels.Service.getInstructors()[i].Availability[t] && DataModels.Service.getInstructors()[i].President)
                        {
                            penalty.AddTerm(penaltyScore, Variables.iGRB[i, t, r]);
                        }
                    }
                }
            }
            return penalty;
        }
        private static GRBLinExpr SupervisorsAreAvailable()
        {
            int penaltyScore = 100;
            GRBLinExpr penalty = 0;
            int S = Variables.sGRB.GetLength(0);
            int T = Variables.sGRB.GetLength(1);
            int R = Variables.sGRB.GetLength(2);
            for (int s = 0; s < S; s++)
            {
                for (int t = 0; t < T; t++)
                {
                    for (int r = 0; r < R; r++)
                    {
                        if (!DataModels.Service.getInstructor(DataModels.Service.getStudents()[s].SupervisorId).Availability[t])
                        {
                            penalty.AddTerm(penaltyScore, Variables.sGRB[s, t, r]);
                        }
                    }
                }
            }
            return penalty;
        }
        private static GRBLinExpr Overtime()
        {
            int penaltyScore = 10000;
            GRBLinExpr penalty = 0;
            int S = Variables.sGRB.GetLength(0);
            int T = Variables.sGRB.GetLength(1);
            int R = Variables.sGRB.GetLength(2);
            bool timeslotOveruse;
            for (int t = S; t < T; t++)
            {
                timeslotOveruse = false;
                for (int s = 0; s < S; s++)
                {
                    for (int r = 0; r < R; r++)
                    {
                        if (!timeslotOveruse)
                        {
                            penalty.AddTerm(penaltyScore, Variables.sGRB[s, t, r]);
                        }
                    }
                }
            }
            return penalty;
        }
        private static GRBLinExpr OverallNumberOfInstructors()
        {
            int penaltyScore = 10;
            GRBLinExpr penalty = 0;
            int I = Variables.iGRB.GetLength(0);
            int T = Variables.iGRB.GetLength(1);
            int R = Variables.iGRB.GetLength(2);
            for (int i = 0; i < I; i++)
            {
                for (int t = 0; t < T; t++)
                {
                    for (int r = 0; r < R; r++)
                    {
                        penalty.AddTerm(penaltyScore, Variables.iGRB[i, t, r]);
                    }
                }
            }
            return penalty;
        }
    }
}
