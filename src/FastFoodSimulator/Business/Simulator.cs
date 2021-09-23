using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FastFoodSimulator.Models;

namespace FastFoodSimulator.Business
{
    public class Simulator : INotifyPropertyChanged
    {
        private readonly Queue<Customer> _queueToOrderTaker;

        private readonly Queue<Customer> _queueToServer;

        private readonly Queue<Order> _ordersToCook;

        private readonly Queue<Order> _cookReadyOrders;

        private readonly Queue<Order> _serverReadyOrders;

        private readonly OrderTaker _orderTaker;

        private readonly Cook _cook;

        private readonly Server _server;

        private uint _customerArrivalTime;

        private uint _customerOrderPickUpTime;

        private uint _cookLeadTime;

        public Simulator(uint cookLeadTime, uint orderTakerOrderingTime, uint customerArrivalTime, uint serverDeliveryTime, uint customerOrderPickUpTime)
        {
            _queueToOrderTaker = new();
            _queueToServer = new();
            _ordersToCook = new();
            _cookReadyOrders = new();
            _serverReadyOrders = new();

            _orderTaker = new OrderTaker(_ordersToCook, orderTakerOrderingTime);
            _cook = new Cook(_ordersToCook, _cookReadyOrders, cookLeadTime);
            _server = new Server(_serverReadyOrders, serverDeliveryTime);

            CookLeadTime = cookLeadTime;
            CustomerArrivalTime = customerArrivalTime;
            OrderTakerOrderingTime = orderTakerOrderingTime;
            ServerDeliveryTime = serverDeliveryTime;
            CustomerOrderPickUpTime = customerOrderPickUpTime;
        }

        public uint CustomerArrivalTime
        {
            get => _customerArrivalTime;
            set
            {
                _customerArrivalTime = value;
                OnPropertyChanged(nameof(_customerArrivalTime));
            }
        }

        public uint CookLeadTime
        {
            get => _cookLeadTime;
            set
            {
                _cookLeadTime = value;
                OnPropertyChanged(nameof(CookLeadTime));
            }
        }

        public uint OrderTakerOrderingTime
        {
            get => _orderTaker.TimeOfOrdering;
            set
            {
                _orderTaker.TimeOfOrdering = value;
                OnPropertyChanged(nameof(OrderTakerOrderingTime));
            }
        }

        public uint ServerDeliveryTime
        {
            get => _server.DeliveryTime;
            set
            {
                _server.DeliveryTime = value;
                OnPropertyChanged(nameof(ServerDeliveryTime));
            }
        }

        public uint CustomerOrderPickUpTime
        {
            get => _customerOrderPickUpTime;
            set
            {
                _customerOrderPickUpTime = value;
                OnPropertyChanged(nameof(CustomerOrderPickUpTime));
            }
        }

        public bool IsSimulatorWorking { get; set; } = true;

        public int NumberOfCustomersWaitingToPlaceOrder => _queueToOrderTaker.Count;

        public string CurrentOrder => _cook.CurrentOrder == null ? "null" : _cook.CurrentOrder.Id.ToString();

        public int CountOfWaitingOrders => _ordersToCook.Count;

        public int ContOfCurrentlyAvailableOrders => _serverReadyOrders.Count;

        public int CountOfWaitingCustomers => _queueToServer.Count;

        public string OrdersReadyToCook
        {
            get
            {
                string answer = "";
                foreach (var order in _ordersToCook)
                {
                    answer += order.Id + " ";
                }

                return answer;
            }
        }

        public string CurrentOrderTakerOrder =>
            _orderTaker.CurrentOrder == null ? "null" : _orderTaker.CurrentOrder.Id.ToString();

        public void StartFastFoodSimulator()
        {
            IsSimulatorWorking = true;
            Task.Run(GenerateCustomers);
            Task.Run(OrderTakerWork);
            Task.Run(CookWork);
            Task.Run(ServerWork);
            Task.Run(QueueToServerWork);
        }

        private void GenerateCustomers()
        {
            while (IsSimulatorWorking)
            {
                Thread.Sleep((int)CustomerArrivalTime);
                _queueToOrderTaker.Enqueue(new Customer());
                PropertyChange();
            }
        }

        private void OrderTakerWork()
        {
            while (IsSimulatorWorking)
            {
                if (_queueToOrderTaker.Count > 0 )
                {
                    var customer = _queueToOrderTaker.Dequeue();

                    _orderTaker.ReceiveAnOrder(customer);

                    _queueToServer.Enqueue(customer);

                    PropertyChange();
                }
            }
        }

        private void CookWork()
        {
            while (IsSimulatorWorking)
            {
                if (_ordersToCook.Count > 0)
                {
                    _cook.CookOrder();
                    PropertyChange();
                }
            }
        }

        private void ServerWork()
        {
            while (IsSimulatorWorking)
            {
                if (_cookReadyOrders.Count > 0)
                {
                    _server.DeliverOrder(_cookReadyOrders.Dequeue());
                    PropertyChange();
                }
            }
        }

        private void QueueToServerWork()
        {
            while (IsSimulatorWorking)
            {
                if (_queueToServer.Count > 0 && _serverReadyOrders.Count > 0)
                {
                    Thread.Sleep((int)CustomerOrderPickUpTime);
                    _serverReadyOrders.Dequeue();
                    _queueToServer.Dequeue();
                    PropertyChange();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PropertyChange()
        {
            OnPropertyChanged(nameof(NumberOfCustomersWaitingToPlaceOrder));
            OnPropertyChanged(nameof(CurrentOrder));
            OnPropertyChanged(nameof(CountOfWaitingOrders));
            OnPropertyChanged(nameof(ContOfCurrentlyAvailableOrders));
            OnPropertyChanged(nameof(CountOfWaitingCustomers));
            OnPropertyChanged(nameof(OrdersReadyToCook));
            OnPropertyChanged(nameof(CurrentOrderTakerOrder));
        }
    }
}
