namespace Microsoft.eShopWeb.Web.Pages.Basket;

public class BasketViewModel
{
    public int Id { get; set; }
    public List<BasketItemViewModel> Items { get; set; } = new List<BasketItemViewModel>();
    public string? BuyerId { get; set; }

    public List<BasketItemViewModel> RecommendedItems { get; set; } = new List<BasketItemViewModel>();

    public decimal Total()
    {
        decimal total = 0m;
        foreach (var item in Items)
        {
            if (item.Discount > 0)
            {
                total += ((item.UnitPrice * Convert.ToDecimal(item.Discount)) / 100m) * item.Quantity;
            }
            else
            {
                total += item.UnitPrice * item.Quantity;
            }
        }

        return Math.Round(total, 2);
    }
}