#region

using OrderApi.Domain.ValueObjects;

#endregion

namespace OrderApi.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public decimal TotalAmount { get; private set; }

    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public Order()
    {
    }

    public Order(Guid customerId, IEnumerable<OrderItem> items)
    {
        CustomerId = customerId;
        _items.AddRange(items);
        TotalAmount = _items.Sum(x => x.Quantity * x.UnitPrice);
    }
}