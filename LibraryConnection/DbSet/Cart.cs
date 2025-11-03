using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryConnection.DbSet
{
    public class Cart
    {
        [Key]
        public Guid id { get; set; }

        // Usuario autenticado (opcional)
        public long? user_id { get; set; }

        // Invitado (opcional)
        public string? session_token { get; set; }

        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        public ICollection<CartItem> items { get; set; }
    }
}
