using SlidingPuzzleEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SlidingPuzzleView
{
    public class PuzzleGridViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }


        #endregion

        public ICommand MoveCommand { get; set; }
        private ObservableCollection<byte> _itemsTable;
        public State StartingState { get; set; }
        public BFSSolver BfsSolver { get; set; }
        public ObservableCollection<byte> ItemsTable
        {
            get => _itemsTable;
            set
            {
                _itemsTable = value;
                OnPropertyChanged(nameof(ItemsTable));
            }
        }

        public PuzzleGridViewModel(ObservableCollection<byte> itemsList)
        {
            ItemsTable = itemsList;
            MoveCommand = new RelayCommand(Move);
            StartingState = new State(4, 4, itemsList.ToArray(), DirectionEnum.None, 0, new List<DirectionEnum>());

        }
        public void Move(int index)
        {

            var list = StartingState.GetAllowedMoves();
            foreach (DirectionEnum directionEnum in list)
            {
                if (directionEnum == DirectionEnum.Left && StartingState.BlankSpaceIndex == index + 1)
                    StartingState.Grid = StartingState.Move(DirectionEnum.Left);
                if (directionEnum == DirectionEnum.Right && StartingState.BlankSpaceIndex == index - 1)
                    StartingState.Grid = StartingState.Move(DirectionEnum.Right);
                if (directionEnum == DirectionEnum.Up && StartingState.BlankSpaceIndex == index + 4)
                    StartingState.Grid = StartingState.Move(DirectionEnum.Up);
                if (directionEnum == DirectionEnum.Down && StartingState.BlankSpaceIndex == index - 4)
                    StartingState.Grid = StartingState.Move(DirectionEnum.Down);

            }
            StartingState.FindBlankSpace();
            ItemsTable = new ObservableCollection<byte>(StartingState.Grid);


        }


        public void SolveBFS()
        {
            BfsSolver = new BFSSolver(StartingState);
            BfsSolver.Solve();
        }
    }
}
