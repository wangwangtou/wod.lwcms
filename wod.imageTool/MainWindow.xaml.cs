using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wod.imageTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel(main);
        }
    }

    public class MainViewModel
    {
        private Frame frame;
        public MainViewModel(Frame frame)
        {
            this.frame = frame;

            ResizeCommand = new NavCommand("resizeParameter.xaml", frame);
            OpacityCommand = new NavCommand("opacityParameter.xaml", frame);

        }
        public ICommand ResizeCommand { get; set; }
        public ICommand OpacityCommand { get; set; }

    }

    public class NavCommand : ICommand
    {
        private string path;
        private Frame frame;

        public NavCommand(string path, Frame frame)
        {
            this.frame = frame;
            this.path = path;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            frame.Navigate(new Uri("parameterView/" + path, UriKind.RelativeOrAbsolute));
        }
    }
}
