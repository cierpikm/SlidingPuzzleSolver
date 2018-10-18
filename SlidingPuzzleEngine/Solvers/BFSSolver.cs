using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlidingPuzzleEngine
{
    public class BFSSolver : PuzzleSolver
    {
        public Queue<State> States { get; set; }
        public List<DirectionEnum> Order { get; set; }
        public BFSSolver(State startingState) : base(startingState)
        {
        }

        public BFSSolver(string order,string startingStatePath, string solutionPath, string infoPath) : base(startingStatePath, solutionPath, infoPath)
        {
            Order = State.StringToDirectionEnums(order);
            States = new Queue<State>();
            States.Enqueue(StartingState);
        }

        protected override bool CanMove()
        {
            return true;
        }

        protected override List<DirectionEnum> GetAllMoves()
        {
            return CurrentState.GetAllowedMoves(Order);
        }

        protected override void AddToStates(State newPuzzle)
        {
            States.Enqueue(newPuzzle);
        }

        protected override State GetFromStates()
        {
            return States.Dequeue();
        }

        protected override int StatesCount()
        {
            return States.Count;
        }

    }
}
