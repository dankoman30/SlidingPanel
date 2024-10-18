using MahApps.Metro.IconPacks;

using System.Windows.Input;
using WpfApp2.Models;
using WpfApp2.ViewModel.Base;

namespace WpfApp2.ViewModel
{
    public class CardViewModel : ViewModelBase
    {
        public CardModel CardModel { get; set; }
        public ICommand NavigateCommand { get; }

        public CardViewModel(CardModel cardModel, ICommand navigateCommand)
        {
            CardModel = cardModel;
            NavigateCommand = navigateCommand;
        }
    }
}
