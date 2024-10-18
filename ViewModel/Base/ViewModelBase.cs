using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp2.ViewModel.Base
{
    public interface IViewModelBase
    {
        IViewModelBase? ParentViewModel { get; set; }

        event PropertyChangedEventHandler? PropertyChanged;
    }

    public abstract class ViewModelBase : INotifyPropertyChanged, IViewModelBase
    {
        public IViewModelBase? ParentViewModel { get; set; } = null;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
