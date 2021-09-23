using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace FastFoodSimulator.Models
{
    public class Cook : INotifyPropertyChanged
    {
        private readonly Queue<Order> _ordersToCook;
        private readonly Queue<Order> _cookReadyOrders;

        private Order _currentOrder;

        public Cook(Queue<Order> ordersToCook, Queue<Order> cookReadyOrders, uint leadTime)
        {
            _ordersToCook = ordersToCook;
            _cookReadyOrders = cookReadyOrders;
            LeadTime = leadTime;
        }


        public uint LeadTime { get; set; }

        public Order CurrentOrder => _currentOrder;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void CookOrder()
        {
            _currentOrder = _ordersToCook.Dequeue();

            OnPropertyChanged($"CurrentOrder");

            Thread.Sleep((int)LeadTime);

            _cookReadyOrders.Enqueue(_currentOrder);
        }
    }
}
