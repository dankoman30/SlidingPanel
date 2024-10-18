using MahApps.Metro.IconPacks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfApp2.Commands.Base;
using WpfApp2.Models;
using WpfApp2.ViewModel.Base;
using System.Diagnostics;

namespace WpfApp2.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<CardViewModel> _cards;
        private int _currentStartIndex;

        public ObservableCollection<CardViewModel> Cards
        {
            get => _cards;
            set
            {
                _cards = value;
                OnPropertyChanged();
            }
        }

        public int CurrentStartIndex
        {
            get => _currentStartIndex;
            set
            {
                if (_currentStartIndex != value)
                {
                    _currentStartIndex = value;
                    OnPropertyChanged();
                    Debug.WriteLine($"CurrentStartIndex changed to {value}");
                }
            }
        }

        public ICommand PreviousCommand { get; }
        public ICommand NextCommand { get; }

        public MainWindowViewModel()
        {
            Cards = new ObservableCollection<CardViewModel>
            {
                CreateCardViewModel("Input", "Form inputs", PackIconMaterialKind.TextBox),
                CreateCardViewModel("Controls", "Fluent & Modern", PackIconMaterialKind.Widgets),
                CreateCardViewModel("Colors", "Themes & Palette", PackIconMaterialKind.Palette),
                CreateCardViewModel("Icons", "Fluent System Icons", PackIconMaterialKind.IdCard),
                CreateCardViewModel("Layouts", "Responsive Grids", PackIconMaterialKind.ViewGrid),
                CreateCardViewModel("Typography", "Text Styles", PackIconMaterialKind.FormatFont)
            };

            PreviousCommand = new CommandsBase(PreviousCommandExecute);
            NextCommand = new CommandsBase(NextCommandExecute);
        }

        private CardViewModel CreateCardViewModel(string title, string description, PackIconMaterialKind iconKind)
        {
            var cardModel = new CardModel
            {
                Title = title,
                Description = description,
                IconKind = iconKind
            };
            return new CardViewModel(cardModel, new CommandsBase(NavigateToCard));
        }

        private void PreviousCommandExecute(object? parameter)
        {
            Debug.WriteLine("PreviousCommand executed");
            if (CurrentStartIndex > 0)
            {
                CurrentStartIndex--;
            }
        }

        private void NextCommandExecute(object? parameter)
        {
            Debug.WriteLine("NextCommand executed");
            if (CurrentStartIndex < Cards.Count - 4)
            {
                CurrentStartIndex++;
            }
        }

        private void NavigateToCard(object? parameter)
        {
            Debug.WriteLine($"Navigating to card: {parameter}");
        }
    }
}