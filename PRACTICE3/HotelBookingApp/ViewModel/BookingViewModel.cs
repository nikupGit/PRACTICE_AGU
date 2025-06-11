using HotelBookingApp.Command;
using HotelBookingApp.Models;
using HotelBookingApp.Storage;
using HotelBookingApp.Utilities;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

        public ObservableCollection<Booking> Bookings { get; } = new();
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
            BookCommand = new DelegateCommand(Book, CanBook);
            UploadImageCommand = new DelegateCommand(UploadImage);
            EditCommand = new DelegateCommand(Edit, CanEditOrDelete);
            DeleteCommand = new DelegateCommand(Delete, CanEditOrDelete);
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
            if (!CanBook(null)) return;

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
            MessageBox.Show("Бронирование добавлено!", "Успех");
        }

        private void UploadImage(object? parameter)
        {
            OpenFileDialog openFileDialog = new()
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
                    MessageBox.Show($"Не удалось загрузить изображение: {ex.Message}", "Ошибка");
                }
            }
        }

        private void Edit(object? parameter)
        {
            if (SelectedBooking == null || !CanBook(null)) return;

            SelectedBooking.FullName = FullName!;
            SelectedBooking.RoomType = RoomType!;
            SelectedBooking.CheckInDate = CheckInDate ?? DateTime.Now;
            SelectedBooking.CheckOutDate = CheckOutDate ?? DateTime.Now;
            SelectedBooking.Image = ClientImage;
            SelectedBooking.TotalPrice = PriceCalculator.CalculateTotalPrice(RoomType, CheckInDate, CheckOutDate);

            var imagePath = _sourceImagePath != null ? BookingRepository.SaveImage(SelectedBooking, _sourceImagePath) : null;

            BookingRepository.SaveBookings(Bookings);
            ClearInputs();
            MessageBox.Show("Бронирование обновлено!", "Успех");
        }

        private void Delete(object? parameter)
        {
            if (SelectedBooking == null) return;

            if (MessageBox.Show("Удалить это бронирование?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Bookings.Remove(SelectedBooking);
                BookingRepository.SaveBookings(Bookings);
                ClearInputs();
                MessageBox.Show("Бронирование удалено!", "Успех");
            }
        }

        private bool CanBook(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(FullName) &&
                   !string.IsNullOrEmpty(RoomType) &&
                   CheckInDate.HasValue &&
                   CheckOutDate.HasValue &&
                   CheckInDate >= DateTime.Today &&
                   CheckOutDate > CheckInDate;
        }

        private bool CanEditOrDelete(object? parameter)
        {
            return SelectedBooking != null;
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