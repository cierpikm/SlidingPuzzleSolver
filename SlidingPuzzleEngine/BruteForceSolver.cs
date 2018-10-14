using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataHandler;

namespace SlidingPuzzleEngine
{
    public class BruteForceSolver
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
        public Queue<State> StatesBfs { get; set; }

        /// <summary>
        /// Stack for dfs states
        /// </summary>
        public Stack<State> StatesDfs { get; set; }

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
        /// Order for searching the neighborhood
        /// </summary>
        public List<DirectionEnum> Order { get; set; }

        /// <summary>
        /// Selected algorithm (bfs or dfs)
        /// </summary>
        public SolverAlgorithmEnum Algorithm { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for View?? 
        /// </summary>
        /// <param name="startingState"></param>
        public BruteForceSolver(State startingState)
        {
            InfoPath = @"\..\..\test.txt";
            SolutionPath = @"\..\..\test1.txt";
            DimensionX = startingState.DimensionX;
            DimensionY = startingState.DimensionY;

            StatesDfs = new Stack<State>();
            StatesBfs = new Queue<State>();
            StartingState = startingState;
            CurrentState = startingState;
            StatesDfs.Push(StartingState);
            StatesBfs.Enqueue(StartingState);
        }

        /// <summary>
        /// Constructor for Command Line with paths and order params 
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="order"></param>
        /// <param name="startingStatePath"></param>
        /// <param name="solutionPath"></param>
        /// <param name="infoPath"></param>
        public BruteForceSolver(SolverAlgorithmEnum algorithm, string order, string startingStatePath, string solutionPath, string infoPath)
        {
            Algorithm = algorithm;
            Order = State.StringToDirectionEnums(order);
            StateDataPack data = DataReader.LoadStartingState(startingStatePath);
            SolutionPath = solutionPath;
            InfoPath = infoPath;
            
            DimensionX = data.DimensionX;
            DimensionY = data.DimensionY;
            StartingState = new State(DimensionX, DimensionY, data.Grid, DirectionEnum.None, 0, new List<DirectionEnum>());
            CurrentState = StartingState;
            
            //if algorithm is dfs, reverse order
            if (Algorithm == SolverAlgorithmEnum.Dfs)
                Order.Reverse();

            StatesDfs = new Stack<State>();
            StatesBfs = new Queue<State>();
            
            StatesDfs.Push(StartingState);
            StatesBfs.Enqueue(StartingState);
        }

        #endregion

        #region Method

        /// <summary>
        /// Method that appends queue or stack with new states from allowed moves for current state.
        /// </summary>
        /// <param name="visited"></param>
        public void AppendWithChildren(ref int visited)
        {
            //if algorithm is dfs, depth level cannot be over 20
            if (CurrentState.DepthLevel >= 20 && Algorithm == SolverAlgorithmEnum.Dfs)
                return;

            List<DirectionEnum> allowedMoves = CurrentState.GetAllowedMoves(Order);
            for (int i = 0; i < allowedMoves.Count; i++)
            {
                visited++;

                State newPuzzle = new State(DimensionX, DimensionY, CurrentState.Move(allowedMoves[i]), allowedMoves[i], CurrentState.DepthLevel + 1, CurrentState.Path.Append(allowedMoves[i]).ToList());
               
                //if algorithm is dfs, Add state to the Stack, else Add state to the queue
                if (Algorithm == SolverAlgorithmEnum.Dfs)
                    StatesDfs.Push(newPuzzle);
                else
                    StatesBfs.Enqueue(newPuzzle);
            }
        }

        /// <summary>
        /// Solve puzzle with selected algorithm
        /// </summary>
        public void Solve()
        {
            StartTime = DateTime.Now.Ticks / (TimeSpan.TicksPerMillisecond / 1000);

            //States processed
            int processed = 0;
           
            //States visited
            int visited = 1;

            while ((StatesDfs.Count > 0 && Algorithm == SolverAlgorithmEnum.Dfs) || (StatesBfs.Count > 0 && Algorithm == SolverAlgorithmEnum.Bfs))
            {
                //if algorithm is dfs, remove last state from the Stack, else remove first state from the queue
                if (Algorithm == SolverAlgorithmEnum.Dfs)
                    CurrentState = StatesDfs.Pop();
                else
                    CurrentState = StatesBfs.Dequeue();

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

        #endregion

    }
}
