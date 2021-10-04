using fesch.Services.Storage;
using Gurobi;

namespace fesch.Services.Scheduler.ExamAttendants
{
    static class Constraints
    {
        static int I = Variables.iGRB.GetLength(0);
        static int S = Variables.sGRB.GetLength(0);
        static int T = Variables.iGRB.GetLength(1);
        static int R = Variables.iGRB.GetLength(2);

        public static void Set(GRBModel model)
        {
            Constraints.NoDuplicateStudents(model);
            Constraints.OneStudentPerTimeSlot(model);
            Constraints.PostsArePresent(model);
            Constraints.NoDuplicateInstructors(model);
        }

        public static void NoDuplicateStudents(GRBModel model)
        {
            GRBLinExpr[] sum = new GRBLinExpr[S];
            for (int s = 0; s < S; s++) { sum[s] = 0; }
            for (int s = 0; s < S; s++)
            {
                for (int t = 0; t < T; t++)
                {
                    for (int r = 0; r < R; r++)
                    {
                        sum[s].AddTerm(1, Variables.sGRB[s, t, r]);
                    }
                }
                model.AddConstr(sum[s] == 1, "NoDuplicateStudents" + s);
            }
        }
        public static void OneStudentPerTimeSlot(GRBModel model)
        {
            GRBLinExpr[] sum = new GRBLinExpr[T];
            for (int t = 0; t < T; t++) { sum[t] = 0; }
            for (int t = 0; t < T; t++)
            {
                for (int r = 0; r < R; r++)
                {
                    for (int s = 0; s < S; s++)
                    {
                        sum[t].AddTerm(1, Variables.sGRB[s, t, r]);
                    }
                    
                }
                model.AddConstr(sum[t] <= 1, "OneStudentPerTimeSlot" + t);
            }
        }
        public static void PostsArePresent(GRBModel model)
        {
            GRBLinExpr[] sumP = new GRBLinExpr[S * T * R];
            GRBLinExpr[] sumS = new GRBLinExpr[S * T * R];
            GRBLinExpr[] sumM = new GRBLinExpr[S * T * R];
            GRBLinExpr[] sumE1 = new GRBLinExpr[S * T * R];
            GRBLinExpr[] sumE2 = new GRBLinExpr[S * T * R];
            for (int str = 0; str < S * T * R; str++)
            {
                sumP[str] = 0;
                sumS[str] = 0;
                sumM[str] = 0;
                sumE1[str] = 0;
                sumE2[str] = 0;
            }
            for (int s = 0; s < S; s++)
            {
                for (int t = 0; t < T; t++)
                {
                    for (int r = 0; r < R; r++)
                    {
                        
                        for (int i = 0; i < I; i++)
                        {
                            if (DataModels.Service.getInstructor(i).President) { sumP[s * T + t].AddTerm(1, Variables.iGRB[i, t, r]); }
                            if (DataModels.Service.getInstructor(i).Secretary) { sumS[s * T + t].AddTerm(1, Variables.iGRB[i, t, r]); }
                            if (DataModels.Service.getInstructor(i).Member) { sumM[s * T + t].AddTerm(1, Variables.iGRB[i, t, r]); }
                            if (DataModels.Service.getInstructor(i).CourseIds.
                                FindAll(x => x == DataModels.Service.getStudent(s).CourseIds[0]).Count > 0) {
                                    sumE1[s * T + t].AddTerm(1, Variables.iGRB[i, t, r]);
                            }
                            if (DataModels.Service.getStudent(s).CourseIds.Count > 1 &&
                                DataModels.Service.getInstructor(i).CourseIds.
                                FindAll(x => x == DataModels.Service.getStudent(s).CourseIds[1]).Count > 0) {
                                    sumE2[s * T + t].AddTerm(1, Variables.iGRB[i, t, r]);
                            }
                        }
                        model.AddConstr(sumP[s * T + t] - Variables.sGRB[s, t, r] >= 0, "PresidentForStudent" + s + "_" + t + "_" + r);
                        model.AddConstr(sumS[s * T + t] - Variables.sGRB[s, t, r] >= 0, "SecretaryForStudent" + s + "_" + t + "_" + r);
                        model.AddConstr(sumM[s * T + t] - Variables.sGRB[s, t, r] >= 0, "MemberForStudent" + s + "_" + t + "_" + r);
                        model.AddConstr(sumE1[s * T + t] - Variables.sGRB[s, t, r] >= 0, "Examiner1ForStudent" + s + "_" + t + "_" + r);
                        if (DataModels.Service.getStudent(s).CourseIds.Count == 2){
                            model.AddConstr(sumE2[s * T + t] - Variables.sGRB[s, t, r] >= 0, "Examiner2ForStudent" + s + "_" + t + "_" + r);
                        }
                        
                    }
                }
            }
        }
        public static void NoDuplicateInstructors(GRBModel model)
        {
            GRBLinExpr[] sum = new GRBLinExpr[I * T];
            for (int it = 0; it < I * T; it++) { sum[it] = 0; }
            for (int i = 0; i < I; i++)
            {
                for (int t = 0; t < T; t++)
                {
                    for (int r = 0; r < R; r++)
                    {
                        sum[i * T + t].AddTerm(1, Variables.iGRB[i, t, r]);
                    }
                    model.AddConstr(sum[i * T + t] <= 1, "NoDuplicateInstructors" + i + "_" + t);
                }
            }
        }
    }
}
