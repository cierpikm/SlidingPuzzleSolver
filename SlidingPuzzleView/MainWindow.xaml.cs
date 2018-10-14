using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SlidingPuzzleView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private ObservableCollection<byte> StartingPosition { get; set; } = new ObservableCollection<byte>
        {
            1,2,3,4,5,6,7,8,9,10,11,12,13,14,0,15
        };
        public PuzzleGridViewModel PuzzleGridVM { get; set; } = new PuzzleGridViewModel(new ObservableCollection<byte>
        {
            1,2,3,4,5,6,7,8,9,10,11,12,13,14,0,15
        });
        public MainWindow()
        {

            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private void RestartButton_OnClick(object sender, RoutedEventArgs e)
        {
            PuzzleGridVM.ItemsTable = new ObservableCollection<byte>(StartingPosition);
        }

        private void Solve_BFS_OnClick(object sender, RoutedEventArgs e)
        {
            PuzzleGridVM.SolveBFS();

        }
    }
}
