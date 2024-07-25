﻿namespace ShipmentService.Web.Models
{
    public class ShippingAddressDto
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
