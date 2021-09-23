using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace FastFoodSimulator.Models
{
    public class OrderTaker : INotifyPropertyChanged
    {
        private readonly Queue<Order> _ordersToCook;
        private Order _currentOrder;

        public OrderTaker(Queue<Order> ordersToCook, uint timeOfOrdering)
        {
            _ordersToCook = ordersToCook;
            TimeOfOrdering = timeOfOrdering;
        }

        public uint TimeOfOrdering { get; set; }

        public Order CurrentOrder
        {
            get => _currentOrder;
            set
            {
                _currentOrder = value;
                OnPropertyChanged(nameof(CurrentOrder));
            }
        }

        public void ReceiveAnOrder(Customer customer)
        {
            CurrentOrder = customer.MakeAnOrder();
            Thread.Sleep((int)TimeOfOrdering);
            PutAnOrderInTheOrderQueue();
        }

        private  void PutAnOrderInTheOrderQueue()
        {
            _ordersToCook.Enqueue(CurrentOrder);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
