using HotelBookingApp.Command;
using HotelBookingApp.Models;
using HotelBookingApp.Storage;
using HotelBookingApp.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HotelBookingApp.ViewModel
{
    public class BookingViewModel : INotifyPropertyChanged
    {
        private readonly BookingRepository _repository;
        private string? _fullName;
        private string? _roomType;
        private DateTime? _checkInDate;
        private DateTime? _checkOutDate;
        private ImageSource? _clientImage;
        private Booking? _selectedBooking;
        private string? _sourceImagePath;

        public ObservableCollection<Booking> Bookings { get; } = new ObservableCollection<Booking>();
        public ICommand BookCommand { get; }
        public ICommand UploadImageCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public string? FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(nameof(FullName)); }
        }

        public string? RoomType
        {
            get => _roomType;
            set { _roomType = value; OnPropertyChanged(nameof(RoomType)); }
        }

        public DateTime? CheckInDate
        {
            get => _checkInDate;
            set { _checkInDate = value; OnPropertyChanged(nameof(CheckInDate)); }
        }

        public DateTime? CheckOutDate
        {
            get => _checkOutDate;
            set { _checkOutDate = value; OnPropertyChanged(nameof(CheckOutDate)); }
        }

        public ImageSource? ClientImage
        {
            get => _clientImage;
            set { _clientImage = value; OnPropertyChanged(nameof(ClientImage)); }
        }

        public Booking? SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                _selectedBooking = value;
                OnPropertyChanged(nameof(SelectedBooking));
                if (_selectedBooking != null)
                {
                    FullName = _selectedBooking.FullName;
                    RoomType = _selectedBooking.RoomType;
                    CheckInDate = _selectedBooking.CheckInDate;
                    CheckOutDate = _selectedBooking.CheckOutDate;
                    ClientImage = _selectedBooking.Image;
                    _sourceImagePath = null;
                }
            }
        }

        public BookingViewModel()
        {
            _repository = new BookingRepository();
            BookCommand = new DelegateCommand(Book);
            UploadImageCommand = new DelegateCommand(UploadImage);
            EditCommand = new DelegateCommand(Edit);
            DeleteCommand = new DelegateCommand(Delete);
            LoadBookings();
        }

        private void LoadBookings()
        {
            var bookings = _repository.LoadBookings();
            foreach (var booking in bookings)
            {
                Bookings.Add(booking);
            }
        }

        private void Book(object? parameter)
        {
            string validationError = ValidateBookingInput();
            if (!string.IsNullOrEmpty(validationError))
            {
                MessageBox.Show($"Невозможно добавить бронирование:\n{validationError}", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var booking = new Booking
            {
                Guid = Guid.NewGuid(),
                FullName = FullName!,
                RoomType = RoomType!,
                CheckInDate = CheckInDate ?? DateTime.Now,
                CheckOutDate = CheckOutDate ?? DateTime.Now,
                Image = ClientImage,
                TotalPrice = PriceCalculator.CalculateTotalPrice(RoomType, CheckInDate, CheckOutDate)
            };

            var imagePath = _sourceImagePath != null ? BookingRepository.SaveImage(booking, _sourceImagePath) : null;

            Bookings.Add(booking);
            BookingRepository.SaveBookings(Bookings);
            ClearInputs();
            MessageBox.Show("Бронирование успешно добавлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UploadImage(object? parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var bitmap = new BitmapImage();
                    using (var stream = File.OpenRead(openFileDialog.FileName))
                    {
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();
                    }
                    bitmap.Freeze();
                    ClientImage = bitmap;
                    _sourceImagePath = openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось загрузить изображение: {ex.Message}.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Edit(object? parameter)
        {
            if (SelectedBooking == null)
            {
                MessageBox.Show("Выберите бронирование для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string validationError = ValidateBookingInput();
            if (!string.IsNullOrEmpty(validationError))
            {
                MessageBox.Show($"Невозможно обновить бронирование:\n{validationError}.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SelectedBooking.FullName = FullName!;
            SelectedBooking.RoomType = RoomType!;
            SelectedBooking.CheckInDate = CheckInDate ?? DateTime.Now;
            SelectedBooking.CheckOutDate = CheckOutDate ?? DateTime.Now;
            SelectedBooking.Image = ClientImage;
            SelectedBooking.TotalPrice = PriceCalculator.CalculateTotalPrice(RoomType, CheckInDate, CheckOutDate);

            var imagePath = _sourceImagePath != null ? BookingRepository.SaveImage(SelectedBooking, _sourceImagePath) : null;

            BookingRepository.SaveBookings(Bookings);
            ClearInputs();
            MessageBox.Show("Бронирование успешно обновлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Delete(object? parameter)
        {
            if (SelectedBooking == null)
            {
                MessageBox.Show("Выберите бронирование для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить это бронирование?", "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Bookings.Remove(SelectedBooking);
                BookingRepository.SaveBookings(Bookings);
                ClearInputs();
                MessageBox.Show("Бронирование успешно удалено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private string ValidateBookingInput()
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(FullName))
                errors.AppendLine("- Укажите ФИО клиента.");

            if (string.IsNullOrWhiteSpace(RoomType))
                errors.AppendLine("- Выберите тип номера.");

            if (!CheckInDate.HasValue)
                errors.AppendLine("- Укажите дату заезда.");
            else if (CheckInDate < DateTime.Today)
                errors.AppendLine("- Дата заезда не может быть раньше текущей даты.");

            if (!CheckOutDate.HasValue)
                errors.AppendLine("- Укажите дату выезда");
            else if (CheckInDate.HasValue && CheckOutDate <= CheckInDate)
                errors.AppendLine("- Дата выезда должна быть позже даты заезда.");

            return errors.ToString();
        }

        private void ClearInputs()
        {
            FullName = string.Empty;
            RoomType = string.Empty;
            CheckInDate = null;
            CheckOutDate = null;
            ClientImage = null;
            _sourceImagePath = null;
            SelectedBooking = null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}