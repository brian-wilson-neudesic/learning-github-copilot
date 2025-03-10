using System.ComponentModel.DataAnnotations;

namespace eShop.Ordering.Domain.AggregatesModel.OrderAggregate
{
    /// <summary>
    /// Represents an order aggregate.
    /// </summary>
    public class Order : Entity, IAggregateRoot
    {
        /// <summary>
        /// Gets the date and time when the order was placed.
        /// </summary>
        public DateTime OrderDate { get; private set; }

        /// <summary>
        /// Gets the address associated with the order.
        /// </summary>
        [Required]
        public Address Address { get; private set; }

        /// <summary>
        /// Gets the buyer identifier.
        /// </summary>
        public int? BuyerId { get; private set; }

        /// <summary>
        /// Gets the buyer associated with the order.
        /// </summary>
        public Buyer Buyer { get; }

        /// <summary>
        /// Gets the current status of the order.
        /// </summary>
        public OrderStatus OrderStatus { get; private set; }
    
        /// <summary>
        /// Gets the order description.
        /// </summary>
        public string Description { get; private set; }

#pragma warning disable CS0414 // The field 'Order._isDraft' is assigned but its value is never used
        private bool _isDraft;
#pragma warning restore CS0414

        // DDD Patterns comment
        // Using a private collection field, better for DDD Aggregate's encapsulation.
        private readonly List<OrderItem> _orderItems;
   
        /// <summary>
        /// Gets the list of order items as a read-only collection.
        /// </summary>
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        /// <summary>
        /// Gets the payment identifier.
        /// </summary>
        public int? PaymentId { get; private set; }

        /// <summary>
        /// Gets the taxes applied to the order.
        /// </summary>
        public decimal Taxes { get; private set; }

        /// <summary>
        /// Creates a new draft order.
        /// </summary>
        /// <returns>A new <see cref="Order"/> instance in draft mode.</returns>
        public static Order NewDraft()
        {
            var order = new Order
            {
                _isDraft = true
            };
            return order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        protected Order()
        {
            _orderItems = new List<OrderItem>();
            _isDraft = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class with the specified details.
        /// </summary>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="userName">The name of the user.</param>
        /// <param name="address">The address for the order.</param>
        /// <param name="cardTypeId">The card type identifier.</param>
        /// <param name="cardNumber">The card number.</param>
        /// <param name="cardSecurityNumber">The card's security number.</param>
        /// <param name="cardHolderName">The name of the card holder.</param>
        /// <param name="cardExpiration">The expiration date of the card.</param>
        /// <param name="buyerId">The buyer identifier (optional).</param>
        /// <param name="paymentMethodId">The payment method identifier (optional).</param>
        public Order(string userId, string userName, Address address, int cardTypeId, string cardNumber, string cardSecurityNumber,
            string cardHolderName, DateTime cardExpiration, int? buyerId = null, int? paymentMethodId = null) : this()
        {
            BuyerId = buyerId;
            PaymentId = paymentMethodId;
            OrderStatus = OrderStatus.Submitted;
            OrderDate = DateTime.UtcNow;
            Address = address;

            // Add the OrderStarterDomainEvent to the domain events collection 
            // to be raised/dispatched when committing changes into the Database [ After DbContext.SaveChanges() ]
            AddOrderStartedDomainEvent(userId, userName, cardTypeId, cardNumber,
                                        cardSecurityNumber, cardHolderName, cardExpiration);
        }

        /// <summary>
        /// Adds an order item to the order.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="productName">The product name.</param>
        /// <param name="unitPrice">The price per unit.</param>
        /// <param name="discount">The discount to apply.</param>
        /// <param name="pictureUrl">The URL of the product picture.</param>
        /// <param name="units">The number of units (default is 1).</param>
        public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
        {
            var existingOrderForProduct = _orderItems.SingleOrDefault(o => o.ProductId == productId);

            if (existingOrderForProduct != null)
            {
                // If a previous order item exists, update it with a higher discount and increase the number of units.
                if (discount > existingOrderForProduct.Discount)
                {
                    existingOrderForProduct.SetNewDiscount(discount);
                }

                existingOrderForProduct.AddUnits(units);
            }
            else
            {
                // Add validated new order item.
                var orderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);
                _orderItems.Add(orderItem);
            }
        }

        /// <summary>
        /// Sets the payment method as verified.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="paymentId">The payment identifier.</param>
        public void SetPaymentMethodVerified(int buyerId, int paymentId)
        {
            BuyerId = buyerId;
            PaymentId = paymentId;
        }

        /// <summary>
        /// Sets the order status to AwaitingValidation if the current status is Submitted.
        /// </summary>
        public void SetAwaitingValidationStatus()
        {
            if (OrderStatus == OrderStatus.Submitted)
            {
                AddDomainEvent(new OrderStatusChangedToAwaitingValidationDomainEvent(Id, _orderItems));
                OrderStatus = OrderStatus.AwaitingValidation;
            }
        }

        /// <summary>
        /// Sets the order status to StockConfirmed if the current status is AwaitingValidation.
        /// </summary>
        public void SetStockConfirmedStatus()
        {
            if (OrderStatus == OrderStatus.AwaitingValidation)
            {
                AddDomainEvent(new OrderStatusChangedToStockConfirmedDomainEvent(Id));

                OrderStatus = OrderStatus.StockConfirmed;
                Description = "All the items were confirmed with available stock.";
            }
        }

        /// <summary>
        /// Sets the order status to Paid if the current status is StockConfirmed.
        /// </summary>
        public void SetPaidStatus()
        {
            if (OrderStatus == OrderStatus.StockConfirmed)
            {
                AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(Id, OrderItems));

                OrderStatus = OrderStatus.Paid;
                Description = "The payment was performed at a simulated \"American Bank checking bank account ending on XX35071\"";
            }
        }

        /// <summary>
        /// Sets the order status to Shipped if the order is paid; otherwise, throws an exception.
        /// </summary>
        public void SetShippedStatus()
        {
            if (OrderStatus != OrderStatus.Paid)
            {
                StatusChangeException(OrderStatus.Shipped);
            }

            OrderStatus = OrderStatus.Shipped;
            Description = "The order was shipped.";
            AddDomainEvent(new OrderShippedDomainEvent(this));
        }

        /// <summary>
        /// Sets the order status to Cancelled if allowed; otherwise, throws an exception.
        /// </summary>
        public void SetCancelledStatus()
        {
            if (OrderStatus == OrderStatus.Paid ||
                OrderStatus == OrderStatus.Shipped)
            {
                StatusChangeException(OrderStatus.Cancelled);
            }

            OrderStatus = OrderStatus.Cancelled;
            Description = "The order was cancelled.";
            AddDomainEvent(new OrderCancelledDomainEvent(this));
        }

        /// <summary>
        /// Sets the order status to Cancelled and updates the description when stock is rejected.
        /// </summary>
        /// <param name="orderStockRejectedItems">The product identifiers that were rejected due to lack of stock.</param>
        public void SetCancelledStatusWhenStockIsRejected(IEnumerable<int> orderStockRejectedItems)
        {
            if (OrderStatus == OrderStatus.AwaitingValidation)
            {
                OrderStatus = OrderStatus.Cancelled;

                var itemsStockRejectedProductNames = OrderItems
                    .Where(c => orderStockRejectedItems.Contains(c.ProductId))
                    .Select(c => c.ProductName);

                var itemsStockRejectedDescription = string.join(", ", itemsStockRejectedProductNames);
                Description = $"The product items don't have stock: ({itemsStockRejectedDescription}).";
            }
        }

        /// <summary>
        /// Calculates the total price of the order including taxes and subtracting discounts.
        /// </summary>
        /// <returns>The total price of the order.</returns>
        public decimal GetTotal()
        {
            var total = _orderItems.Sum(o => o.Units * o.UnitPrice);
            var totalDiscount = _orderItems.Sum(o => o.Discount);
            return total + Taxes - totalDiscount;
        }

        // Private helper methods are not publicly documented.
        private void AddOrderStartedDomainEvent(string userId, string userName, int cardTypeId, string cardNumber,
            string cardSecurityNumber, string cardHolderName, DateTime cardExpiration)
        {
            var orderStartedDomainEvent = new OrderStartedDomainEvent(this, userId, userName, cardTypeId,
                                                                        cardNumber, cardSecurityNumber,
                                                                        cardHolderName, cardExpiration);

            this.AddDomainEvent(orderStartedDomainEvent);
        }

        private void StatusChangeException(OrderStatus orderStatusToChange)
        {
            throw new OrderingDomainException($"Is not possible to change the order status from {OrderStatus} to {orderStatusToChange}.");
        }
    }
}
