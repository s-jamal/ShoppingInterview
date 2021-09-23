using ShoppingInterview.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingInterview
{
  public class Db : IDb
  {
    public ICollection<Item> Items { get; set; }
    public ICollection<Discount> Discounts { get; set; }

    public Db(ICollection<Item> items, ICollection<Discount> discounts)
    {
      if (items == null)
      {
        throw new ArgumentNullException("Database must have items");
      }
      Items = items;
      Discounts = discounts;
    }
  }
}
