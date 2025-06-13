namespace CommonRefactorTask.Service
{
    public class DiscountService
    {
        public static int CalculateDiscountPercentage(decimal totalPrice)
        {
            return totalPrice switch
            {
                < 1000 => 0,
                >= 1000 and < 5000 => 5,
                >= 5000 => 10
            };
        }

        public static decimal CalculateDiscountedPrice(decimal totalPrice)
        {
            var discount = CalculateDiscountPercentage(totalPrice);
            return totalPrice - (totalPrice * discount / 100);
        }
    }
}