namespace FastFoodSimulator.Models
{
    public class Order : Model
    {
        private static int _orderCounter;

        public Order(int customerId)
        {
            Id = ++_orderCounter;
            CustomerId = customerId;
        }

        public int CustomerId { get; set; }
    }
}
