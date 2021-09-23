using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingInterview.Interfaces
{
  public interface IItem
  {
    string Sku { get; set; }
    decimal Price { get; set; }
  }
}
