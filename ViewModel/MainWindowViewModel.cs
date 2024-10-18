using MahApps.Metro.IconPacks;

using System.Collections.ObjectModel;

using System.Windows.Input;
using WpfApp2.Commands.Base;
using WpfApp2.Models;
using WpfApp2.ViewModel.Base;
using static System.Net.Mime.MediaTypeNames;

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
                OnPropertyChanged(nameof(Cards));
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
                    OnPropertyChanged(nameof(CurrentStartIndex));
                    CommandManager.InvalidateRequerySuggested();
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

            PreviousCommand = new CommandsBase(Previous, CanGoToPrevious);
            NextCommand = new CommandsBase(Next, CanGoToNext);
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

        private void Previous(object? parameter)
        {
            if (CurrentStartIndex > 0)
            {
                CurrentStartIndex--;
            }
        }

        private bool CanGoToPrevious(object? parameter) => CurrentStartIndex > 0;

        private void Next(object? parameter)
        {
            if (CurrentStartIndex < Cards.Count - 4)
            {
                CurrentStartIndex++;
            }
        }

        private bool CanGoToNext(object? parameter) => CurrentStartIndex < Cards.Count - 4;

        private void NavigateToCard(object? parameter)
        {
            // Implement navigation logic here
            System.Diagnostics.Debug.WriteLine($"Navigating to card: {parameter}");
        }
    }
}
