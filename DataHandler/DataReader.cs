using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler
{
    public static class DataReader
    {

        public static StateDataPack LoadStartingState(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {

                // Read the stream to a string, and write the string to the console.
                String[] line = sr.ReadLine()?.Split(' ','\r','\n');
                byte x = byte.Parse(line[0]);
                byte y = byte.Parse(line[1]);
                String[] gridString = sr.ReadLine()?.Split(' ');
                byte[] grid = new Byte[x * y];
                for (int i = 0; i < y; i++)
                {
                    for (int j = 0; j < x; j++)
                    {
                        grid[j + i * x] = byte.Parse(gridString[j + i * x ]);
                    }
                }
                return new StateDataPack() { DimensionX = x, DimensionY = y, Grid = grid };
            }

        }
    }
}
