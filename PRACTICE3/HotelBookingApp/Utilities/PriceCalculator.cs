namespace HotelBookingApp.Utilities
{
    public static class PriceCalculator
    {
        private static readonly Dictionary<string, decimal> roomPrices = new()
        {
            { "Одноместный", 100m },
            { "Двухместный", 180m },
            { "Люкс", 300m }
        };

        public static decimal CalculateTotalPrice(string? roomType, DateTime? checkInDate, DateTime? checkOutDate)
        {
            if (string.IsNullOrEmpty(roomType) || !checkInDate.HasValue || !checkOutDate.HasValue)
                return 0m;

            var days = (checkOutDate.Value - checkInDate.Value).Days;
            return days * roomPrices[roomType];
        }
    }
}