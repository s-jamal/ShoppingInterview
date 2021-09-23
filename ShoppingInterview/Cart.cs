using ShoppingInterview.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingInterview
{
  public class Cart : ICart
  {
    private readonly IDb _db;

    public ICollection<Item> Items { get; private set; } = new List<Item>();


    public Cart(IDb db)
    {
      if (db == null)
      {
        throw new ArgumentNullException("Must have a database");
      }

      _db = db;
    }


    public void AddItem(Item item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("Item not found");
      }

      Items.Add(item);
    }

    public bool RemoveItem(Item item)
    {
      if (item == null)
      {
        throw new ArgumentNullException("Item not found");
      }

      return Items.Remove(item);
    }


    public decimal GetTotal()
    {
      decimal total = 0;
      total = Items.GroupBy(c => c.Sku).Sum(c => CalculateItemGroupPrice(c));

      return total;
    }

    public void EmptyCart()
    {
      Items = new List<Item>();
    }

    private decimal CalculateItemGroupPrice(IGrouping<string, Item> items)
    {
      int count = items.Count();
      string sku = items.Key;

      var discount = _db.Discounts.SingleOrDefault(c => c.Sku == sku);

      if (discount == null)
        return items.Sum(c => c.Price);

      int remaining = count % discount.Amount;
      decimal discountedItemsPrice = (count / discount.Amount) * discount.Price;

      decimal total = remaining * items.Select(c => c.Price).First() + discountedItemsPrice;

      return total;
    }
  }
}
