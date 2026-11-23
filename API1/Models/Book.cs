using System;
using System.Collections.Generic;

namespace API1.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public int AuthorId { get; set; }

    public string? Isbn { get; set; }

    public decimal Price { get; set; }

    public int? Pages { get; set; }

    public int? StockQuantity { get; set; }

    public bool? IsAvailable { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
