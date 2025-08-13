using System;
using Volo.Abp.Domain.Entities;

namespace ECommerce.Orders
{
    public class OrderItem : Entity<int>
    {
        #region Properties
        public int OrderId { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Price { get; private set; }

        #endregion

        #region Navigation Property
        public Order Order { get; private set; }

        #endregion 

        private OrderItem() { }

        public OrderItem(int id, int orderId, int productId, int quantity, decimal unitPrice) : base(id) 
        {
            //OrderId = orderId;
            //ProductId = productId;
            //Quantity = SetQuantity(quantity);
            //UnitPrice = SetUnitPrice(unitPrice);
            //Price = SetPrice(quantity, unitPrice);
            Set(orderId, productId, quantity, unitPrice);

        }

        //private int SetQuantity(int quantity) 
        //{
        //    if (quantity <= 0) 
        //        throw new System.ArgumentException("Quantity must be > 0", nameof(quantity));

        //    return quantity;
        //}
        //private decimal SetUnitPrice(decimal unitPrice)
        //{
        //    if (unitPrice < 0) 
        //        throw new System.ArgumentException("Unit price must be >= 0", nameof(unitPrice));
        //    return unitPrice;

        //}

        //private decimal SetPrice(decimal quantity , decimal unitPrice)
        //{
        //    decimal price = (decimal)(unitPrice * quantity);
        //    return price;
        //}
        private void Set(int orderId, int productId, int quantity, decimal unitPrice)
        {
            if (quantity <= 0) throw new System.ArgumentException("Quantity must be > 0", nameof(quantity));
            if (unitPrice < 0) throw new System.ArgumentException("Unit price must be >= 0", nameof(unitPrice));
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Price = quantity * unitPrice;
        }
    }
}