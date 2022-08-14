using Avalonia.Controls;
using ExerciseApp.Pages;

namespace ExerciseApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StaticSetter.MainWindow = this;
            StaticSetter.MainWindow.Content = new MainPage();
        }
    }
}