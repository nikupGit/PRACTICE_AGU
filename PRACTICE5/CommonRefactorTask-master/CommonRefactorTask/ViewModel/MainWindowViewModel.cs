using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommonRefactorTask.Command;
using CommonRefactorTask.Model;
using CommonRefactorTask.Service;


namespace CommonRefactorTask.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<Good> OrderList { get; } = new();
        public ObservableCollection<Good> GoodList { get; } = new();

        private Good? _selectedGood;
        public Good? SelectedGood
        {
            get => _selectedGood;
            set
            {
                _selectedGood = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private Good? _selectedOrderItem;
        public Good? SelectedOrderItem
        {
            get => _selectedOrderItem;
            set
            {
                _selectedOrderItem = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal TotalPrice => OrderList.Sum(g => g.Price);

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand BuyCommand { get; }

        public MainWindowViewModel()
        {
            AddCommand = new DelegateCommand(Add, _ => SelectedGood != null);
            DeleteCommand = new DelegateCommand(Delete, _ => SelectedOrderItem != null);
            BuyCommand = new DelegateCommand(Buy);

            LoadData();
        }

        private void LoadData()
        {
            var goods = new[]
            {
                new Good { Name = "Банан", Price = 90 },
                new Good { Name = "Шоколадка", Price = 100 },
                new Good { Name = "Кола", Price = 75 }
            };

            foreach (var good in goods)
                GoodList.Add(good);

            SelectedGood = goods.FirstOrDefault();
            SelectedOrderItem = null;
        }

        private void Add(object? parameter)
        {
            if (parameter is Good selectedGood)
            {
                OrderList.Add(selectedGood);
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        private void Delete(object? parameter)
        {
            if (parameter is Good selectedItem)
            {
                OrderList.Remove(selectedItem);
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        private void Buy(object? parameter)
        {
            var discount = DiscountService.CalculateDiscountPercentage(TotalPrice);
            var finalPrice = DiscountService.CalculateDiscountedPrice(TotalPrice);

            MessageBox.Show($"Скидка составила - {discount}%, итоговая цена - {finalPrice}");

            OrderList.Clear();
            OnPropertyChanged(nameof(TotalPrice));
        }
    }
}