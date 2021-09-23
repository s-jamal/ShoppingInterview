using ShoppingInterview.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingInterview
{
  class Program
  {
    private static IDb _db { get; set; }
    private static ICartManagementBySku _cartManagement { get; set; }
    private static ICart _cart{ get; set; }
    static void Main(string[] args)
    {
      var items = new List<Item>()
      {
        new Item()
        {
          Sku = "Vase",
          Price = 1.2M
        },
        new Item()
        {
          Sku = "Mug",
          Price = 1M
        },
        new Item()
        {
          Sku = "Napkin",
          Price = 0.45M
        },
      };

      var discounts = new List<Discount>()
      {
        new Discount()
          {
            Sku = "Mug",
            Amount = 2,
            Price = 1.5M
          },
        new Discount()
          {
            Sku = "Napkin",
            Amount = 3,
            Price = 0.9M
          }
      };

      _db = new Db(items, discounts);
      _cart = new Cart(_db);
      _cartManagement = new CartManagementBySku(_cart, _db);

      MainMenu();



      Console.ReadLine();
    }

    private static void MainMenu()
    {
      Console.Clear();

      Console.WriteLine("1. View Cart");
      Console.WriteLine("2. Add Item");
      Console.WriteLine("3. Remove Item");
      Console.WriteLine("4. Get total");
      Console.WriteLine("0. Return to main menu");


      byte key = ReadSelection();
      MainMenuSelected(key);
    }

    private static void MainMenuSelected(byte key)
    {
      switch (key)
      {
        case 1:
          ViewCart();
          MainMenuReturn();
          break;
        case 2:
          {
            ShowItems();
            string sku = Console.ReadLine();
            AddItem(sku);
            MainMenu();
            break;
          }
        case 3:
          {
            ViewCart();
            string sku = Console.ReadLine();
            RemoveItem(sku);
            MainMenu();
            break;
          }
        case 4:
          ShowTotal();
          MainMenuReturn();
          break;
        case 5:
          _cart.RemoveItem(new Item() { Price = 5, Sku = "asd" });
          break;
        case 0:
          MainMenu();
          return;
        default:
          Console.WriteLine("Wrong selection. Try again!");
          ReadSelection();
          break;
      }
    }

    private static void AddItem(string sku)
    {
      try
      {
        _cartManagement.AddItemBySku(sku);
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
        MainMenuReturn();
      }
    }

    private static void RemoveItem(string sku)
    {
      try
      {
        _cartManagement.RemoveItemBySku(sku);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        MainMenuReturn();
      }
    }

    private static void MainMenuReturn()
    {
      Console.WriteLine("\n Press Enter to go to main menu");
      Console.ReadLine();
      MainMenu();
    }

    private static void ShowTotal()
    {
      Console.Clear();
      Console.WriteLine($"\nTotal: {_cart.GetTotal()}");
    }

    private static void ViewCart()
    {
      Console.Clear();
      var groupedItems = _cart.Items.GroupBy(c => c.Sku);

      if (groupedItems.Count() == 0)
      {
        Console.WriteLine("\nCart is empty");
        return;
      }

      for (int i = 0; i < groupedItems.Count(); i++)
      {
        Console.WriteLine($"{i + 1}. {groupedItems.ElementAt(i).Key} - {groupedItems.ElementAt(i).Count()}");
      }
    }

    private static void ShowItems()
    {
      Console.Clear();
      for (int i = 1; i <= _db.Items.Count(); i++)
      {
        Console.WriteLine($"{i}. {_db.Items.ElementAt(i - 1).Sku}");
      }

      Console.WriteLine("0. Return to main menu");
    }

    private static byte ReadSelection()
    {
      string read = Console.ReadLine();
      byte selectedKey;

      if (!byte.TryParse(read, out selectedKey))
      {
        Console.WriteLine("Wrong selection. Try again!");
        selectedKey = ReadSelection();
      }

      return selectedKey;
    }
  }
}
