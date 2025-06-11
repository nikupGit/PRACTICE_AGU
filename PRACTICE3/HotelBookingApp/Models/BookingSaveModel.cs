namespace HotelBookingApp.Models
{
    public class BookingSaveModel
    {
        public Guid Guid { get; set; }
        public required string FullName { get; set; }
        public required string RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string? ImagePath { get; set; }
        public decimal TotalPrice { get; set; }
    }
}