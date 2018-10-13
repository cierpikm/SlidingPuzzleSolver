using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlidingPuzzleEngine
{
    /// <summary>
    /// Directions that piece can be moved
    /// </summary>
    public enum Direction
    {
        Left, Right, Up, Down, None
    }

    public class PuzzleCore
    {

        #region Properties

        /// <summary>
        /// The List that contains Puzzle Grid 
        /// </summary>
        public List<byte> PuzzleGrid { get; set; }

        /// <summary>
        /// The Dimension of the Puzzle
        /// </summary>
        public int Dimension { get; set; } = 4;

        /// <summary>
        /// The Value of Last Move piece 
        /// </summary>
        public int LastMove { get; set; }

        /// <summary>
        /// The coordinates of the Blank Space (x,y)
        /// </summary>
        public Point BlankSpace { get; set; }

        /// <summary>
        /// The Path of the final solution
        /// </summary>
        public List<Direction> Path { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor with parameters ( dimensions and filled Puzzle Grid )
        /// </summary>
        /// <param name="dimensions"></param>
        /// <param name="puzzleGrid"></param>
        public PuzzleCore(int dimensions, List<byte> puzzleGrid)
        {
            PuzzleGrid = new List<byte>(dimensions * dimensions);
            PuzzleGrid = puzzleGrid;
            BlankSpace = FindBlankSpace();
            Path= new List<Direction>();
        }


        #endregion

        #region Methods

        /// <summary>
        /// Gets allowed move for specified piece, if piece can't be moved returns Direction.None
        /// </summary>
        /// <param name="index">index of specified piece</param>
        /// <returns></returns>
        public Direction GetMove(int index)
        {
            int blankColumn = BlankSpace.X;
            int blankRow = BlankSpace.Y;
            int column = IndexToPoint(index).X;
            int row = IndexToPoint(index).Y;
            if (blankRow < 3 && row == blankRow + 1 && column == blankColumn)
            {
                return Direction.Down;
            }
            else if (blankRow > 0 && row == blankRow - 1 && column == blankColumn)
            {
                return Direction.Up;
            }
            else if (blankColumn < 3 && row == blankRow && column == blankColumn + 1)
            {
                return Direction.Right;
            }
            else if (blankColumn > 0 && row == blankRow && column == blankColumn - 1)
            {
                return Direction.Left;
            }
            else
                return Direction.None;
        }

        /// <summary>
        /// Converts index in 'Dimension' x 'Dimension' Grid to XY Point
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Point IndexToPoint(int index)
        {
            return new Point(index % Dimension, (index - (index % Dimension)) / Dimension);
        }
        /// <summary>
        /// Converts XY Point to index in 'Dimension' x 'Dimension' Grid 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public int PointToIndex(Point point)
        {
            return point.Y * Dimension + point.X;
        }

        /// <summary>
        /// Returns Blank Space position as XY Point
        /// </summary>
        /// <returns></returns>
        public Point FindBlankSpace()
        {
            for (int i = 0; i < PuzzleGrid.Count; i++)
            {
                if (PuzzleGrid[i] == 0)
                {
                    return new Point(i % Dimension, (i - (i % Dimension)) / Dimension);
                }
            }

            throw new ArgumentException("Blank Spave can not be found");
        }

        /// <summary>
        /// Returns indexes of Pieces that can be moved in specified direction (not Direction.None)
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllowedMoves()
        {
            List<int> allowedMoves = new List<int>();
            for (int i = 0; i < PuzzleGrid.Count; i++)
            {
                if (GetMove(i) != Direction.None)
                    allowedMoves.Add(i);
            }
            return allowedMoves;
        }

        /// <summary>
        /// Swaps two pieces in the Puzzle Grid list
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        public void Swap(int index1, int index2)
        {
            byte temp = PuzzleGrid[index1];
            PuzzleGrid[index1] = PuzzleGrid[index2];
            PuzzleGrid[index2] = temp;
        }

        /// <summary>
        /// Checks if Sliding Puzzle is solved
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            int maxElementCount = Dimension * Dimension - 1;
            for (int i = 0; i < maxElementCount; i++)
                if (PuzzleGrid[i] != i + 1)
                    return false;
            if (PuzzleGrid[maxElementCount] != 0)
                return false;
            return true;
        }
        
        /// <summary>
        /// Moves piece if its able to move
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Direction Move(int index)
        {
            Direction move = GetMove(index);
            if (move != Direction.None)
            {
                LastMove = PointToIndex(BlankSpace);
                Swap(index,PointToIndex(BlankSpace));
                BlankSpace = FindBlankSpace();

            }
            return move;
        }

        /// <summary>
        /// Returns Copy of the current Puzzle Core 
        /// </summary>
        /// <returns></returns>
        public PuzzleCore GetCopy()
        {
            return new PuzzleCore(Dimension,PuzzleGrid)
            {
                Path = this.Path,
                LastMove = this.LastMove
            };
        }

        #endregion

    }
}
