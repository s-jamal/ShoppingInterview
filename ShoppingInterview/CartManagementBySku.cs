using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShoppingInterview.Interfaces;

namespace ShoppingInterview
{
  public class CartManagementBySku : ICartManagementBySku
  {
    private readonly ICart _cart;
    private readonly IDb _db;

    public CartManagementBySku(ICart cart, IDb db)
    {
      _cart = cart;
      _db = db;
    }

    public void AddItemBySku(string sku)
    {
      Item item = _db.Items.Where(c => c.Sku == sku).FirstOrDefault();

      if (item == null)
      {
        throw new NullReferenceException("Item not found in database");
      }

      _cart.AddItem(item);
    }

    public void RemoveItemBySku(string sku)
    {
      Item item = _cart.Items.Where(c => c.Sku == sku).FirstOrDefault();

      if (item == null)
      {
        throw new NullReferenceException("Item not found in cart");
      }

      _cart.RemoveItem(item);
    }
  }
}
