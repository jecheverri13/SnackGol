using System;
using System.Collections.Generic;

namespace LibraryConnection.Dtos
{
    public class CartDto
    {
        public Guid id { get; set; }
        public List<CartItemDto> items { get; set; } = new();
        public int count { get; set; }
        public double total { get; set; }
    }
}
