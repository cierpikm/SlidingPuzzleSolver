using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlidingPuzzleEngine
{
    public class BFSSolver
    {
        public PuzzleCore StartingState { get; set; }
        public PuzzleCore CurrentState { get; set; }
        public Queue<PuzzleCore> States { get; set; }

        public BFSSolver(PuzzleCore startingState)
        {
            States=new Queue<PuzzleCore>();
            StartingState = startingState;
            CurrentState = startingState;
            States.Enqueue(StartingState);
        }

        public void AppendQueueWithChildrens()
        {
            List<int> allowedMoves = CurrentState.GetAllowedMoves();
            for (int i = 0; i < allowedMoves.Count; i++)
            {
                int move = allowedMoves[i];
                if (move != CurrentState.LastMove)
                {
                    PuzzleCore newPuzzle = new PuzzleCore(CurrentState.Dimension,new List<byte>(CurrentState.PuzzleGrid)){Path = new List<Direction>(CurrentState.Path)};
                    Direction direction = newPuzzle.Move(move);
                    newPuzzle.Path.Add(direction);
                    States.Enqueue(newPuzzle);
                }
            }
        }

        public List<Direction> Solve()
        {
            while (States.Count > 0)
            {
                CurrentState = States.Dequeue();
                if (CurrentState.Check())
                {
                    Console.WriteLine("Done");
                    foreach (Direction direction in CurrentState.Path)
                    {
                        Console.WriteLine(direction.ToString());
                    }

                    return CurrentState.Path; 
                }

                AppendQueueWithChildrens();
            }

            return null;
        }
    }
}
