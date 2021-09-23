using ShoppingInterview.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingInterview
{
  public class Discount : IDiscount
  {
    public string Sku { get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }
  }
}
