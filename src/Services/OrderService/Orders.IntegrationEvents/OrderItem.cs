﻿namespace Orders.IntegrationEvents;

public class OrderItem
{
    public Guid BookId { get; set; }
    public int Quantity {  get; set; }
    public decimal Price { get; set; }
}
