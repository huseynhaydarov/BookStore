﻿namespace OrderService.Domain.Entities;

public record Address
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EmailAddress { get; set; }
    public string? Country { get; set; }
    public string? State { get; set; }
    public string? Street { get; set; }
}