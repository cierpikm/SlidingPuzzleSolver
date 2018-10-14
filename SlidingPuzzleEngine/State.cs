using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlidingPuzzleEngine
{
    public class State
    {
        public byte DimensionX { get; set; }
        public byte DimensionY { get; set; }
        public byte[] Grid { get; set; }
        //public State ParentState { get; set; }
        public DirectionEnum LastMove { get; private set; }
        public int DepthLevel { get; set; }
        public int BlankSpaceIndex { get; set; }
        public List<DirectionEnum> Path { get; set; }

        public State(byte dimensionX, byte dimensionY,  byte[] grid, DirectionEnum lastMove, int depthLevel, List<DirectionEnum> path)
        {
            DimensionX = dimensionX;
            DimensionY = dimensionY;
            Grid = grid;
           // ParentState = parentState;
            LastMove = lastMove;
            DepthLevel = depthLevel;
            FindBlankSpace();
            Path = path;
        }
        public bool IsSolved()
        {
            for (int i = 0; i < DimensionY; i++)
            {
                for (int j = 0; j < DimensionX; j++)
                {
                    if (i == DimensionY - 1 && j == DimensionX - 1)
                    {
                        if (Grid[j + i * DimensionX] != 0)
                            return false;
                    }
                    else
                    {
                        if (Grid[j + i * DimensionX] != j + i * DimensionX + 1)
                            return false;
                    }
                    
                }
            }
            return true;
        }

        public int GetBlankSpaceX()
        {
            return BlankSpaceIndex % DimensionX;
        }
        public int GetBlankSpaceY(int x)
        {
            return (BlankSpaceIndex - x) / DimensionX;
        }

        public List<DirectionEnum> GetAllowedMoves()
        {
            List<DirectionEnum> moves = new List<DirectionEnum>(4);
            int blankSpaceX = GetBlankSpaceX();
            int blankSpaceY = GetBlankSpaceY(blankSpaceX);

            if (blankSpaceY > 0 && LastMove != DirectionEnum.Down)
                moves.Add(DirectionEnum.Up);
            if (blankSpaceY < DimensionY - 1 && LastMove != DirectionEnum.Up)
                moves.Add(DirectionEnum.Down);
            if (blankSpaceX > 0 && LastMove != DirectionEnum.Right)
                moves.Add(DirectionEnum.Left);
            if (blankSpaceX < DimensionX - 1 && LastMove != DirectionEnum.Left)
                moves.Add(DirectionEnum.Right);
            return moves;

        }

        public void FindBlankSpace()
        {
            for (int i = 0; i < Grid.Length; i++)
            {
                if (Grid[i] == 0)
                    BlankSpaceIndex = i;
            }
        }
        public byte[] Move(DirectionEnum direction)
        {
            byte[] grid = (byte[])Grid.Clone();

            if (direction == DirectionEnum.Left)
            {
                Swap(ref grid[BlankSpaceIndex - 1], ref grid[BlankSpaceIndex]);
            }
            if (direction == DirectionEnum.Right)
            {
                Swap(ref grid[BlankSpaceIndex + 1], ref grid[BlankSpaceIndex]);
            }
            if (direction == DirectionEnum.Up)
            {
                Swap(ref grid[BlankSpaceIndex - DimensionX], ref grid[BlankSpaceIndex]);
            }
            if (direction == DirectionEnum.Down)
            {
                Swap(ref grid[BlankSpaceIndex + DimensionX], ref grid[BlankSpaceIndex]);
            }

            return grid;
        }

        public void Swap(ref byte value1, ref byte value2)
        {
            byte temp = value1;
            value1 = value2;
            value2 = temp;
        }
        /*public string GetAllMoves()
        {
            string moves = null;
            if (ParentState != null)
                moves += ParentState.GetAllMoves();
            else
                return null;

            moves += LastMove.ToString()[0];
            return moves;
        }*/

        public static double GetTime(long startTime)
        {
            long endTime = 10000L * Stopwatch.GetTimestamp();
            endTime /= TimeSpan.TicksPerMillisecond;
            endTime *= 100L;
            return (double)(endTime - startTime) / 1000000;
        }


    }
}
