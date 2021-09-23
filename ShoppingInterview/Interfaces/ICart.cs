using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingInterview.Interfaces
{
  public interface ICart
  {
    ICollection<Item> Items { get; }
    void AddItem(Item item);
    bool RemoveItem(Item item);
    decimal GetTotal();
    void EmptyCart();
  }

}
