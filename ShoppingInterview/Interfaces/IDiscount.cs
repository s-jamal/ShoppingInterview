using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingInterview.Interfaces
{
  public interface IDiscount
  {
    int Amount { get; set; }
    decimal Price { get; set; }
  }
}
