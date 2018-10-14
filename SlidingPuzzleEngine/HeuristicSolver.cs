using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataHandler;

namespace SlidingPuzzleEngine
{
    public class HeuristicSolver
    {
        #region Property

        /// <summary>
        /// The time that solve algorithm starts
        /// </summary>
        public long StartTime { get; set; }

        /// <summary>
        /// Starting state loaded from file
        /// </summary>
        public State StartingState { get; set; }

        /// <summary>
        /// State that is currently processing
        /// </summary>
        public State CurrentState { get; set; }

        /// <summary>
        /// Queue for bfs states
        /// </summary>
        public C5.IntervalHeap<Tuple<State, int>> States { get; set; }

        /// <summary>
        /// Dimension X loaded from file
        /// </summary>
        public byte DimensionX { get; set; }

        /// <summary>
        /// Dimension Y loaded from file
        /// </summary>
        public byte DimensionY { get; set; }

        /// <summary>
        /// Max depth achieved by solver
        /// </summary>
        public int MaxDepth { get; set; }

        /// <summary>
        /// Path to save Info file
        /// </summary>
        public string InfoPath { get; set; }

        /// <summary>
        /// Path to save soultion file
        /// </summary>
        public string SolutionPath { get; set; }

        /// <summary>
        /// Selected function (hamming or Manhattan)
        /// </summary>
        public HeuristicFunctionEnum Function { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for View?? 
        /// </summary>
        /// <param name="startingState"></param>
        public HeuristicSolver(State startingState)
        {
            InfoPath = @"\..\..\test.txt";
            SolutionPath = @"\..\..\test1.txt";
            DimensionX = startingState.DimensionX;
            DimensionY = startingState.DimensionY;

            StartingState = startingState;
            CurrentState = startingState;
            States = new C5.IntervalHeap<Tuple<State, int>>(
                Comparer<Tuple<State, int>>.Create((t1, t2) =>
                    t1.Item2 > t2.Item2 ? 1 : t1.Item2 < t2.Item2 ? -1 : 0)) { new Tuple<State, int>(StartingState, 0) };
        }

        /// <summary>
        /// Constructor for Command Line with paths and function params 
        /// </summary>
        /// <param name="function"></param>
        /// <param name="startingStatePath"></param>
        /// <param name="solutionPath"></param>
        /// <param name="infoPath"></param>
        public HeuristicSolver(HeuristicFunctionEnum function, string startingStatePath, string solutionPath, string infoPath)
        {
            Function = function;
            StateDataPack data = DataReader.LoadStartingState(startingStatePath);
            SolutionPath = solutionPath;
            InfoPath = infoPath;

            DimensionX = data.DimensionX;
            DimensionY = data.DimensionY;
            StartingState = new State(DimensionX, DimensionY, data.Grid, DirectionEnum.None, 0, new List<DirectionEnum>());
            CurrentState = StartingState;
            States = new C5.IntervalHeap<Tuple<State, int>>(
                Comparer<Tuple<State, int>>.Create((t1, t2) =>
                    t1.Item2 > t2.Item2 ? 1 : t1.Item2 < t2.Item2 ? -1 : 0)) { new Tuple<State, int>(StartingState, 0) };

        }

        #endregion

        #region Method

        /// <summary>
        /// Method that appends priority queue with new states from allowed moves for current state.
        /// </summary>
        /// <param name="visited"></param>
        private void AppendWithChildren(ref int visited)
        {

            List<DirectionEnum> allowedMoves = CurrentState.GetAllowedMoves();
            for (int i = 0; i < allowedMoves.Count; i++)
            {
                visited++;

                State newPuzzle = new State(DimensionX, DimensionY, CurrentState.Move(allowedMoves[i]), allowedMoves[i], CurrentState.DepthLevel + 1, CurrentState.Path.Append(allowedMoves[i]).ToList());
                States.Add(new Tuple<State, int>(newPuzzle, HeuristicFunction(newPuzzle)));
            }
        }

        /// <summary>
        /// Solve puzzle with selected function
        /// </summary>
        public void Solve()
        {
            StartTime = DateTime.Now.Ticks / (TimeSpan.TicksPerMillisecond / 1000);

            //States processed
            int processed = 0;

            //States visited
            int visited = 1;

            while (States.Count > 0)
            {

                CurrentState = States.DeleteMin().Item1;

                MaxDepth = CurrentState.DepthLevel > MaxDepth ? CurrentState.DepthLevel : MaxDepth;
                processed++;

                //Check if state is solved, if its solved Write info to the file
                if (CurrentState.IsSolved())
                {
                    string path = null;
                    foreach (DirectionEnum directionEnum in CurrentState.Path)
                    {
                        path += directionEnum.ToString()[0];
                    }

                    DataWriter.WriteSolutionToFile(new InformationDataPack()
                    {
                        SizeOfSolvedPuzzle = CurrentState.Path.Count,
                        Solution = path,
                    }, SolutionPath);

                    DataWriter.WriteInfoToFile(new InformationDataPack()
                    {
                        DepthSize = MaxDepth,
                        SizeOfSolvedPuzzle = CurrentState.Path.Count,
                        StatesVisited = visited,
                        StatesProcessed = processed,
                        Time = (double)(DateTime.Now.Ticks / (TimeSpan.TicksPerMillisecond / 1000) - StartTime) / 1000
                    }, InfoPath);
                    Console.WriteLine("Done!");
                    return;
                }

                AppendWithChildren(ref visited);
            }
        }

        /// <summary>
        /// Distance function
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private int HeuristicFunction(State state)
        {
            byte[] board = state.Grid;
            int distance = state.Path.Count;

            if (Function == HeuristicFunctionEnum.Manhattan)
                distance += Manhattan(board);
            if (Function == HeuristicFunctionEnum.Hamming)
                distance += Hamming(board);

            return distance;
        }

        /// <summary>
        /// Manhattan heuristic function
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private int Manhattan(byte[] board)
        {
            int distance = 0;
            for (int i = 0; i < DimensionY; i++)
            {
                for (int j = 0; j < DimensionX; j++)
                {
                    int value = board[j + i * DimensionX];
                    if (value == 0)
                    {
                        int x = DimensionX - 1;
                        int y = DimensionY - 1;
                        distance += Math.Abs(j - x) + Math.Abs(i - y);
                    }
                    else
                    {
                        int x = value - 1 % DimensionX;
                        int y = (value - 1 - x) / DimensionX;
                        distance += Math.Abs(j - x) + Math.Abs(i - y);
                    }

                }
            }
            return distance;
        }
        /// <summary>
        /// Hamming Heuristic Function
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private int Hamming(byte[] board)
        {
            int distance = 0;
            for (int i = 0; i < DimensionY; i++)
            {
                for (int j = 0; j < DimensionX; j++)
                {
                    if (i == DimensionY - 1 && j == DimensionX - 1)
                    {
                        if (board[j + i * DimensionX] != 0)
                            distance++;
                    }
                    else
                    {
                        if (board[j + i * DimensionX] != j + i * DimensionX + 1)
                            distance++;
                    }

                }
            }
            return distance;
        }
        #endregion
    }
}
