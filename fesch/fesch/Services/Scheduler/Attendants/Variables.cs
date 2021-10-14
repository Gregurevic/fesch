using fesch.Services.Exceptions;
using fesch.Services.Storage.Scheduler;
using Gurobi;

namespace fesch.Services.Scheduler.ExamAttendants
{
    static class Variables
    {
        ///dimension
        public static int S = Attendants.Service.DimensionS;
        public static int I = Attendants.Service.DimensionI;
        public static int D = Attendants.Service.DimensionD;
        public static int C = Attendants.Service.DimensionC;
        public static int OS = Attendants.Service.DimensionOS;
        public static int OL = Attendants.Service.DimensionOL;
        /// variables
        public static GRBVar[,,] iGRB;
        public static GRBVar[,,] sGRB;
        public static GRBVar[][] ordinal;
        /// objective helper variables
        public static GRBVar[,,] SAM; /// short-exam (instructors')availability matrix
        public static GRBVar[,,] LAM; /// long-exam (instructors')availability matrix
        public static void Set(GRBModel model)
        {
            /// variables
            iGRB = new GRBVar[I, D, C];
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
            sGRB = new GRBVar[S, D, C];
            for (int s = 0; s < S; s++)
            {
                for (int d = 0; d < D; d++)
                {
                    for (int c = 0; c < C; c++)
                    {
                        sGRB[s, d, c] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "s_" + s + "_" + d + "_" + c);
                    }
                }
            }
            ordinal = new GRBVar[S][];
            for (int s = 0; s < S; s++)
            {
                int length = Attendants.Service.Students[s].Short ? OS : OL;
                ordinal[s] = new GRBVar[length];
                for(int l = 0; l < length; l++)
                {
                    ordinal[s][l] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "ordinal_" + s + "_" + l);
                }
            }
            /// objective helper variables
            SAM = new GRBVar[I, D, OS];
            int time;
            int fromIdx;
            int toIdx;
            for (int i = 0; i < I; i++)
            {
                for (int d = 0; d < D; d++)
                {
                    for (int os = 0; os < OS; os++)
                    {
                        time = 60; /// 9:00 -> 60;
                        time += os * 40; /// previous exams' length
                        time += ((os + 1) / 6) * 50; /// add lunch duration (50) from 6th daily exam onward
                        fromIdx = time / 60; /// convert to instructor availability index from the above calculated 'time'
                        toIdx = (time + 40) / 60; /// current exam's finishing availability index
                        bool available = Attendants.Service.Instructors[i].Availability[fromIdx] && Attendants.Service.Instructors[i].Availability[toIdx];
                        double mtxValue = available ? 1.0 : 0.0;
                        SAM[i, d, os] = model.AddVar(mtxValue, mtxValue, 0.0, GRB.BINARY, "SAM_" + i + "_" + d + "_" + os);
                    }
                }
            }
            LAM = new GRBVar[I, D, OL];
            for (int i = 0; i < I; i++)
            {
                for (int d = 0; d < D; d++)
                {
                    for (int ol = 0; ol < OL; ol++)
                    {
                        time = 50; /// 8:50 -> 50;
                        time += ol * 50; /// previous exams' length
                        time += ((ol + 1) / 5) * 50; /// add lunch duration (50) from 5th daily exam onward
                        fromIdx = time / 60; /// convert to instructor availability index from the above calculated 'time'
                        toIdx = (time + 50) / 60; /// current exam's finishing availability index
                        bool available = Attendants.Service.Instructors[i].Availability[fromIdx] && Attendants.Service.Instructors[i].Availability[toIdx];
                        double mtxValue = available ? 1.0 : 0.0;
                        LAM[i, d, ol] = model.AddVar(mtxValue, mtxValue, 0.0, GRB.BINARY, "LAM_" + i + "_" + d + "_" + ol);
                    }
                }
            }
        }

        public static GRBQuadExpr MultiplyAt(GRBVar[,,] AM, GRBVar[]ordinal, int i, int d)
        {
            if (AM.GetLength(3) != ordinal.Length) throw new AttendantSchedulerException("Ordinal length mismatch!");
            GRBQuadExpr product = 0;
            for (int o = 0; o < ordinal.Length; o++)
            {
                product.Add(AM[i, d, o] * ordinal[o]);
            }
            return product;
        }
    }
}
