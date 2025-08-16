using ECommerce.Orders.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ECommerce.Orders
{
    public class Order : FullAuditedAggregateRoot<int>
    {
        #region Properties
        public Guid CustomerId { get; private set; }

        public OrderStatus Status { get; private set; }

        public decimal OrderTotal { get; private set; }

        public DateTime CreatedOnUtc { get; private set; }

        public DateTime UpdatedOnUtc { get; private set; }

        #endregion

        #region Navigation Property

        public ICollection<OrderItem> OrderItems { get; private set; }

        #endregion

        #region Ctor
        private Order() { OrderItems = new List<OrderItem>(); }


        public Order(int id, Guid customerId, ICollection<OrderItem> orderItems) : base(id)
        {
            CustomerId = customerId;
            Status = OrderStatus.Pending;
            CreatedOnUtc = DateTime.UtcNow;
            OrderTotal = 0m;
            OrderItems = orderItems;
        }
        #endregion

        #region Helper Mithods

        public void SetStatus(OrderStatus status)
        {
            Status = status;
            UpdatedOnUtc = DateTime.UtcNow;
        }

        public void RecalculateTotal()
        {
            decimal total = 0m;
            foreach (var item in OrderItems)
                total += item.Price;
            OrderTotal = total;
            UpdatedOnUtc = DateTime.UtcNow;
        }

        #endregion

    }
}
