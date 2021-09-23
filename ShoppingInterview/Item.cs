using ShoppingInterview.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingInterview
{
  public class Item : IItem
  {
    public string Sku { get; set; }
    public decimal Price { get; set; }
  }
}
