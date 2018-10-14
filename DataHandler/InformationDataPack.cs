using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler
{
    public class InformationDataPack
    {
        public string Solution { get; set; }
        public int SizeOfSolvedPuzzle { get; set; }
        public int StatesVisited { get; set; }
        public int StatesProcessed { get; set; }
        public int DepthSize { get; set; }
        public double Time { get; set; }
    }
}
