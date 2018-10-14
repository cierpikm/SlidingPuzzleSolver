using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler
{
    public class StateDataPack
    {
        public byte[] Grid { get; set; }
        public byte DimensionX { get; set; }
        public byte DimensionY { get; set; }
    }
}
