namespace FastFoodSimulator.Models
{
    public class Customer : Model
    {
        private static int _customerCounter;

        public Customer()
        {
            Id = ++_customerCounter;
        }

        public string Name { get; set; }

        public Order MakeAnOrder()
        {
            return new(Id);
        }
    }
}
