using HotelBookingApp.Models;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Imaging;


namespace HotelBookingApp.Storage
{
    public class BookingRepository
    {
        private const string DataFilePath = "bookings.json";
        private const string ImagesFolder = "BookingImages";

        public BookingRepository()
        {
            Directory.CreateDirectory(ImagesFolder);
        }

        public List<Booking> LoadBookings()
        {
            var bookings = new List<Booking>();
            try
            {
                if (File.Exists(DataFilePath))
                {
                    var json = File.ReadAllText(DataFilePath);
                    var savedBookings = JsonSerializer.Deserialize<List<BookingSaveModel>>(json)
                        ?? new List<BookingSaveModel>();

                    foreach (var savedBooking in savedBookings)
                    {
                        var booking = new Booking
                        {
                            Guid = savedBooking.Guid,
                            FullName = savedBooking.FullName,
                            RoomType = savedBooking.RoomType,
                            CheckInDate = savedBooking.CheckInDate,
                            CheckOutDate = savedBooking.CheckOutDate,
                            TotalPrice = savedBooking.TotalPrice
                        };

                        if (!string.IsNullOrEmpty(savedBooking.ImagePath) && File.Exists(savedBooking.ImagePath))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.UriSource = new Uri(Path.GetFullPath(savedBooking.ImagePath), UriKind.Absolute);
                            bitmap.EndInit();
                            bitmap.Freeze();
                            booking.Image = bitmap;
                        }

                        bookings.Add(booking);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить бронирования: {ex.Message}", "Ошибка");
            }
            return bookings;
        }

        public static void SaveBookings(IEnumerable<Booking> bookings)
        {
            try
            {
                var bookingsToSave = new List<BookingSaveModel>();
                foreach (var booking in bookings)
                {
                    bookingsToSave.Add(new BookingSaveModel
                    {
                        Guid = booking.Guid,
                        FullName = booking.FullName,
                        RoomType = booking.RoomType,
                        CheckInDate = booking.CheckInDate,
                        CheckOutDate = booking.CheckOutDate,
                        TotalPrice = booking.TotalPrice,
                        ImagePath = (booking.Image as BitmapImage)?.UriSource?.LocalPath
                    });
                }

                var json = JsonSerializer.Serialize(bookingsToSave, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(DataFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить бронирования: {ex.Message}", "Ошибка");
            }
        }

        public static string? SaveImage(Booking booking, string sourcePath)
        {
            try
            {
                if (string.IsNullOrEmpty(sourcePath) || !File.Exists(sourcePath))
                {
                    return null;
                }

                string imagePath = Path.Combine(ImagesFolder, $"{booking.Guid}.png");
                File.Copy(sourcePath, imagePath, true);

                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(Path.GetFullPath(imagePath), UriKind.Absolute);
                    bitmap.EndInit();
                    bitmap.Freeze();
                    booking.Image = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Изображение скопировано, но не удалось загрузить для отображения: {ex.Message}", "Предупреждение");
                }

                return Path.GetFullPath(imagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить изображение: {ex.Message}", "Ошибка");
                return null;
            }
        }
    }
}