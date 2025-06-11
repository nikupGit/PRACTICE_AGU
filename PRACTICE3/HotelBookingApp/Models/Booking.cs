using System.Windows.Media;


namespace HotelBookingApp.Models
{
    public class Booking
    {
        public Guid Guid { get; set; }
        public required string FullName { get; set; }
        public required string RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public ImageSource? Image { get; set; }
        public decimal TotalPrice { get; set; }
    }
}