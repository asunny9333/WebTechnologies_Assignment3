namespace Assignment_3.Models
{
    public class Cart
    {
        public int CartId { get; set; } // Primary key
        public int UserId { get; set; } // Foreign key

        public User? User { get; set; }
        public ICollection<CartItem>? CartItems { get; set; } // Navigation property
    }

    public class CartItem
{
    public int CartItemId { get; set; } // Primary key
    public int CartId { get; set; } // Foreign key
    public int ProductId { get; set; } // Foreign key
    public int Quantity { get; set; }

    public Cart? Cart { get; set; }
    public Product? Product { get; set; }
}

}
