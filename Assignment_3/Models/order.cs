public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string OrderDetails { get; set; } 

    public User User { get; set; }
}
