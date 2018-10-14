using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SlidingPuzzleEngine;

namespace SlidingPuzzleCommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 5)
                Console.WriteLine("Too few arguments");
            else
            {
                switch (args[0])
                {
                    case "bfs":
                        {
                            string path = @"..\..\DataHandler\Data\";
                            BFSSolver bfsSolver = new BFSSolver(path + args[2], path + args[3], path + args[4]);
                            bfsSolver.Solve();
                            break;

                        }
                    case "dfs":
                        {
                            break;

                        }
                    case "astr":
                        {
                            break;

                        }
                }
            }

        }
    }
}
