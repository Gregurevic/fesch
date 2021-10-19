using Gurobi;
using System;

namespace fesch.Services.Scheduler
{
    static class Visualizer
    {
        /// Visualizer for Structure Scheduler
        /// Can be used after the model.Optimize() call in Scheduler.cs
        public static void VisualizeStructures()
        {
            int I = Structure.Variables.iGRB.GetLength(0);
            int D = Structure.Variables.iGRB.GetLength(1);
            int C = Structure.Variables.iGRB.GetLength(2);
            GRBLinExpr temp = 0;
            for (int i = 0; i < I; i++)
            {
                for (int d = 0; d < D; d++)
                {
                    for (int c = 0; c < C; c++)
                    {
                        Console.Write(Structure.Variables.iGRB[i, d, c].X);
                        temp.AddTerm(1, Structure.Variables.iGRB[i, d, c]);
                    }
                    Console.Write("  ");
                }
                Console.Write("\n");
            }
            Console.WriteLine(temp.Value);
        }

        /// Visualizer for Attendant Scheduling
        /// Can be used after the model.Optimize() call in Scheduler.cs
        public static void VisualizeAttendants()
        {
            int S = Attendant.Variables.S;
            int F = Attendant.Variables.F;
            Console.WriteLine("sfx");
            for (int f = 0; f < F; f++)
            {
                for (int s = 0; s < S; s++)
                {
                    Console.Write(Attendant.Variables.sfx[s, f].X);
                }
                Console.Write("\n");
            }
            Console.Write("\n");
            Console.WriteLine("sor");
            int[] temp = new int[Attendant.Variables.OS];
            for (int o = 0; o < Attendant.Variables.OS; o++) { temp[o] = 0; }
            for (int s = 0; s < S; s++)
            {
                for (int o = 0; o < Attendant.Variables.sor[s].Length; o++)
                {
                    Console.Write(Attendant.Variables.sor[s][o].X);
                    temp[o] += (int)Attendant.Variables.sor[s][o].X;
                }
                Console.Write("\n");
            }
            for (int o = 0; o < Attendant.Variables.OL; o++) { Console.Write(temp[o] + " "); }
            Console.Write("\n");
            Console.WriteLine("sme");
            for (int s = 0; s < S; s++)
            {
                for (int me = 0; me < Attendant.Variables.sme[s].Length; me++)
                {
                    Console.Write(Attendant.Variables.sme[s][me].X);
                }
                Console.Write("\n");
            }
        }
    }
}
