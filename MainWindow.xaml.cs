using System.Windows;
using System.Windows.Input;
using WpfApp2.ViewModel;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            CommandManager.RequerySuggested += CommandManager_RequerySuggested;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowViewModel.CurrentStartIndex))
            {
                CardPanel.AnimateToIndex(ViewModel.CurrentStartIndex);
            }
        }

        private void CommandManager_RequerySuggested(object sender, EventArgs e)
        {
            // This ensures that the CanExecute method is called when the command manager suggests it
            CommandManager.InvalidateRequerySuggested();
        }
    }
}