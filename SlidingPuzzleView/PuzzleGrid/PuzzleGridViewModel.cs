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
        public PuzzleCore StartingState { get; set; }
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
            StartingState=new PuzzleCore(4,itemsList.ToList());
            
        }
        public void Move(int index)
        {
            SlidingPuzzleEngine.Direction move = StartingState.GetMove(index);
            if (move != SlidingPuzzleEngine.Direction.None)
            {
                StartingState.Swap(index, StartingState.PointToIndex(StartingState.BlankSpace));
                StartingState.BlankSpace = StartingState.FindBlankSpace();
                ItemsTable=new ObservableCollection<byte>(StartingState.PuzzleGrid);

            }
        }


        public List<SlidingPuzzleEngine.Direction> SolveBFS()
        {
            BfsSolver = new BFSSolver(StartingState);
            return BfsSolver.Solve();
        }
    }
}
