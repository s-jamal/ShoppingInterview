using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingInterview.Interfaces
{
  public interface ICartManagementBySku
  {
    void AddItemBySku(string sku);
    void RemoveItemBySku(string sku);
  }
}
