using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SlidingPuzzleView
{
    public class RelayCommand : ICommand
    {
        private Action<int> mAction;

        public RelayCommand(Action<int> mAction)
        {
            this.mAction = mAction;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mAction(int.Parse((string) parameter));
        }

        public event EventHandler CanExecuteChanged;
    }
}
