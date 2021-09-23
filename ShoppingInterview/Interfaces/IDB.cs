using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingInterview.Interfaces
{
  public interface IDb
  {
    ICollection<Item> Items { get; }
    ICollection<Discount> Discounts { get; }
  }
}
