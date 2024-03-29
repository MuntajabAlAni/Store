﻿namespace Core.Entities.OrderAggregate;

public class Order : BaseEntity
{
    public Order(IReadOnlyList<OrderItem> orderItems, string buyerEmail, Address shipToAddress,
        DeliveryMethod deliveryMethod, decimal subtotal, string paymentIntentId)
    {
        BuyerEmail = buyerEmail;
        ShipToAddress = shipToAddress;
        DeliveryMethod = deliveryMethod;
        OrderItems = orderItems;
        Subtotal = subtotal;
        PaymentIntentId = paymentIntentId;
    }

    public Order()
    {
    }

    public string BuyerEmail { get; set; }
    public string OrderDate { get; set; } = DateTime.Now.ToString();
    public Address ShipToAddress { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public IReadOnlyList<OrderItem> OrderItems { get; set; }
    public decimal Subtotal { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string PaymentIntentId { get; set; } = "";

    // for AutoMapper !!
    public decimal GetTotal()
    {
        return Subtotal + DeliveryMethod.Price;
    }
}