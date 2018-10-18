using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlidingPuzzleEngine
{
    public class DFSSolver : PuzzleSolver
    {
        public Stack<State> States { get; set; }
        public List<DirectionEnum> Order { get; set; }
        public DFSSolver(State startingState) : base(startingState)
        {
        }

        public DFSSolver(string order, string startingStatePath, string solutionPath, string infoPath) : base(startingStatePath, solutionPath, infoPath)
        {
            Order = State.StringToDirectionEnums(order);
            Order.Reverse();
            States = new Stack<State>();
            States.Push(StartingState);
        }

        protected override bool CanMove()
        {
            return CurrentState.DepthLevel < 20;
        }

        protected override List<DirectionEnum> GetAllMoves()
        {
            return CurrentState.GetAllowedMoves(Order);
        }

        protected override void AddToStates(State newPuzzle)
        {
            States.Push(newPuzzle);
        }

        protected override State GetFromStates()
        {
            return States.Pop();
        }

        protected override int StatesCount()
        {
            return States.Count;
        }
    }
}
