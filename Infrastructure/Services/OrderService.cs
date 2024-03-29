﻿using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;

    public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IPaymentService paymentService)
    {
        _paymentService = paymentService;
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Order?> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId,
        Address shippingAddress)
    {
        // get basket from repo 
        var basket = await _basketRepository.GetBasketAsync(basketId);
        // get items from product repo
        var items = new List<OrderItem>();
        basket!.Items.ForEach(async item =>
        {
            var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        });
        // get dm from repo
        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
        // calc subtotal
        var subtotal = items.Sum(item => item.Price * item.Quantity);
        //check if order exists
        var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
        var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        if (existingOrder != null)
        {
            _unitOfWork.Repository<Order>().Delete(existingOrder);
            await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
        }
        // create order
        var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal, basket.PaymentIntentId);
        _unitOfWork.Repository<Order>().Add(order);
        //save to db
        var result = await _unitOfWork.Complete();
        if (result <= 0)
            return null;
        // return order
        return order;
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
        return await _unitOfWork.Repository<Order>().ListAsyncWithSpec(spec);
    }

    public async Task<Order?> GetOrderByIdAsync(int id, string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
        return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
    }
}