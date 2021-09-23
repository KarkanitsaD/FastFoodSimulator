using System.Collections.Generic;
using System.Threading;

namespace FastFoodSimulator.Models
{
    public class Server
    {
        private readonly Queue<Order> _readyOrders;

        public Server(Queue<Order> readyOrders, uint deliveryTime)
        {
            _readyOrders = readyOrders;
            DeliveryTime = deliveryTime;
        }

        public uint DeliveryTime { get; set; }

        public void DeliverOrder(Order order)
        {
            Thread.Sleep((int)DeliveryTime);
            _readyOrders.Enqueue(order);
        }
    }
}
