using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataHandler;

namespace SlidingPuzzleEngine
{
    public class DFSSolver
    {
        public long StartTime { get; set; }
        public State StartingState { get; set; }
        public State CurrentState { get; set; }
        public Stack<State> States { get; set; }
        public byte DimensionX { get; set; }
        public byte DimensionY { get; set; }
        public string InfoPath { get; set; }
        public string SolutionPath { get; set; }
        public List<DirectionEnum> Order { get; set; }

        public DFSSolver(State startingState)
        {
            InfoPath = @"\..\..\test.txt";
            SolutionPath = @"\..\..\test1.txt";
            DimensionX = startingState.DimensionX;
            DimensionY = startingState.DimensionY;

            States = new Stack<State>();
            StartingState = startingState;
            CurrentState = startingState;
            States.Push(StartingState);
        }

        public DFSSolver(string order, string startingStatePath, string infoPath, string solutionPath)
        {
            InfoPath = infoPath;
            SolutionPath = solutionPath;
            StateDataPack data = DataReader.LoadStartingState(startingStatePath);
            DimensionX = data.DimensionX;
            DimensionY = data.DimensionY;
            Order = State.StringToDirectionEnums(order);
            States = new Stack<State>();
            StartingState = new State(DimensionX, DimensionY, data.Grid, DirectionEnum.None, 0, new List<DirectionEnum>());
            CurrentState = StartingState;
            States.Push(StartingState);
        }

        public void AppendQueueWithChildrens()
        {
            if (CurrentState.DepthLevel > 20)
                return;

            List<DirectionEnum> allowedMoves = CurrentState.GetAllowedMoves(Order);
            for (int i = 0; i < allowedMoves.Count; i++)
            {
                State newPuzzle = new State(DimensionX, DimensionY, CurrentState.Move(allowedMoves[i]), allowedMoves[i], CurrentState.DepthLevel + 1, CurrentState.Path.Append(allowedMoves[i]).ToList());
                States.Push(newPuzzle);
            }
        }

        public void Solve()
        {
            StartTime = 10000L * Stopwatch.GetTimestamp();
            StartTime /= TimeSpan.TicksPerMillisecond;
            StartTime *= 100L;
            while (States.Count > 0)
            {
                CurrentState = States.Pop();
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
                        DepthSize = CurrentState.DepthLevel,
                        SizeOfSolvedPuzzle = CurrentState.Path.Count,
                        StatesVisited = 0,
                        StatesProcessed = 0,
                        Time = State.GetTime(StartTime)
                    }, InfoPath);
                    Console.WriteLine("Done!");
                    return;
                }

                AppendQueueWithChildrens();
            }
        }
    }
}
