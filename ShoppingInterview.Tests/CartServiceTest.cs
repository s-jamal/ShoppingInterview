using ShoppingInterview.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ShoppingInterview.Tests
{
  public class CartServiceTest
  {
    private ICart _cart { get; set; }
    private ICartManagementBySku _cartManagement { get; set; }
    private IDb _db { get; set; }
    private readonly Item vase = new Item
    {
      Sku = "vase",
      Price = 1.2M
    };
    private readonly Item bigMug = new Item
    {
      Sku = "bigMug",
      Price = 1M
    };
    private readonly Item napkinsPack = new Item
    {
      Sku = "napkinsPack",
      Price = 0.45M
    };
    private readonly Discount mugDiscount = new Discount
    {
      Sku = "bigMug",
      Amount = 2,
      Price = 1.5M
    };
    private readonly Discount napkinPackDiscount = new Discount
    {
      Sku = "napkinsPack",
      Amount = 3,
      Price = 0.9M
    };
    public CartServiceTest()
    {
      _db = new Db(new List<Item> { vase, bigMug, napkinsPack }, new List<Discount> { mugDiscount, napkinPackDiscount });
      _cart = new Cart(_db);
      _cartManagement = new CartManagementBySku(_cart, _db);
    }

    [Fact]
    public void InitialTotalIsZero()
    {
      decimal result = _cart.GetTotal();

      Assert.Equal(0M, result);
    }

    [Fact]
    public void OneItemTotalCorrect()
    {
      _cart.AddItem(vase);

      Assert.Equal(1.2M, _cart.GetTotal());

      _cart.EmptyCart();
      Assert.True(_cart.Items.Count == 0);

      _cart.AddItem(bigMug);
      Assert.Equal(1M, _cart.GetTotal());

      _cart.EmptyCart();
      Assert.True(_cart.Items.Count == 0);

      _cart.AddItem(bigMug);
      Assert.Equal(1M, _cart.GetTotal());
    }

    [Fact]
    public void SeveralItemsTotalCorrect()
    {
      _cart.AddItem(vase);
      _cart.AddItem(vase);
      _cart.AddItem(vase);

      Assert.Equal(3.6M, _cart.GetTotal());


      _cart.AddItem(bigMug);
      Assert.Equal(4.6M, _cart.GetTotal());

      _cart.AddItem(napkinsPack);
      _cart.AddItem(napkinsPack);
      Assert.Equal(5.5M, _cart.GetTotal());
    }



    [Fact]
    public void SeveralItemsWithDiscountsTotalCorrect()
    {
      _cart.AddItem(vase);
      _cart.AddItem(bigMug);
      _cart.AddItem(napkinsPack);
      _cart.AddItem(napkinsPack);
      _cart.AddItem(bigMug);
      _cart.AddItem(bigMug);
      _cart.AddItem(vase);
      _cart.AddItem(napkinsPack);
      _cart.AddItem(napkinsPack);
      _cart.AddItem(vase);

      // 3 vase 3 big mug 4 napkin
      // 3.6 + 2.5 + 0.9 + 0.45

      Assert.Equal(7.45M, _cart.GetTotal());
    }

    [Fact]
    public void SeveralItemsWithDiscountsTotalCorrectSku()
    {
      _cartManagement.AddItemBySku("vase");
      _cartManagement.AddItemBySku("bigMug");
      _cartManagement.AddItemBySku("napkinsPack");
      _cartManagement.AddItemBySku("napkinsPack");
      _cartManagement.AddItemBySku("bigMug");
      _cartManagement.AddItemBySku("bigMug");
      _cartManagement.AddItemBySku("vase");
      _cartManagement.AddItemBySku("napkinsPack");
      _cartManagement.AddItemBySku("napkinsPack");
      _cartManagement.AddItemBySku("vase");


      Assert.Equal(7.45M, _cart.GetTotal());
    }

    [Fact]
    public void SeveralItemsWithDiscountsWithRemovalsTotalCorrectSku()
    {
      _cartManagement.AddItemBySku("vase");
      _cartManagement.AddItemBySku("bigMug");
      _cartManagement.RemoveItemBySku("vase");
      _cartManagement.AddItemBySku("napkinsPack");
      _cartManagement.AddItemBySku("bigMug");
      _cartManagement.AddItemBySku("napkinsPack");
      _cartManagement.RemoveItemBySku("bigMug");
      _cartManagement.AddItemBySku("bigMug");
      _cartManagement.AddItemBySku("bigMug");
      _cartManagement.AddItemBySku("vase");
      _cartManagement.RemoveItemBySku("napkinsPack");
      _cartManagement.AddItemBySku("napkinsPack");
      _cartManagement.AddItemBySku("bigMug");
      _cartManagement.AddItemBySku("napkinsPack");
      _cartManagement.AddItemBySku("vase");

      // 2 vases     4 mugs     3 napkins
      //   2.4    +    3     +     0.9     = 7.3

      Assert.Equal(6.3M, _cart.GetTotal());
    }

    [Fact]
    public void SeveralItemsWithDiscountsWithRemovalsTotalCorrect()
    {
      _cart.AddItem(vase);
      _cart.AddItem(bigMug);
      _cart.RemoveItem(vase);
      _cart.AddItem(napkinsPack);
      _cart.AddItem(bigMug);
      _cart.AddItem(napkinsPack);
      _cart.RemoveItem(bigMug);
      _cart.AddItem(bigMug);
      _cart.AddItem(bigMug);
      _cart.AddItem(vase);
      _cart.RemoveItem(napkinsPack);
      _cart.AddItem(napkinsPack);
      _cart.AddItem(bigMug);
      _cart.AddItem(napkinsPack);
      _cart.AddItem(vase);

      // 2 vases     4 mugs     3 napkins
      //   2.4    +    3     +     0.9     = 7.3

      Assert.Equal(6.3M, _cart.GetTotal());
    }



    [Fact]
    public void CreatingDatabase()
    {
      Assert.Throws<ArgumentNullException>(() => _db = new Db(null, null));
      Assert.Throws<ArgumentNullException>(() => _db = new Db(null, new List<Discount> { new Discount() }));
      _db = new Db(new List<Item> { }, null);
    }

    [Fact]
    public void CreatingCart()
    {
      Assert.Throws<ArgumentNullException>(() => _cart = new Cart(null));
      _cart = new Cart(_db);
    }

    [Fact]
    public void RemoveItemWithoutAdding()
    {
      Assert.False(_cart.RemoveItem(vase));
    }

    [Fact]
    public void RemoveItemAfterAdding()
    {
      _cart.AddItem(vase);
      Assert.True(_cart.RemoveItem(vase));
    }


    [Fact]
    public void AddItemCheck()
    {
      _cart.AddItem(vase);
      Assert.True(_cart.Items.FirstOrDefault() == vase);
    }


    [Fact]
    public void AddItemNullException()
    {
      Assert.Throws<ArgumentNullException>(() => _cart.AddItem(null));
    }

    [Fact]
    public void AddItemNotFoundInDatabaseException()
    {
      Assert.Throws<NullReferenceException>(() => _cartManagement.AddItemBySku("wrong sku"));
    }

    [Fact]
    public void RemoveItemNullException()
    {
      Assert.Throws<ArgumentNullException>(() => _cart.RemoveItem(null));
    }

    [Fact]
    public void RemoveItemNotInCartException()
    {
      Assert.Throws<NullReferenceException>(() => _cartManagement.RemoveItemBySku("wrong sku"));
    }

    [Fact]
    public void EmptyCart()
    {
      _cart.AddItem(vase);
      _cart.AddItem(napkinsPack);
      _cart.AddItem(napkinsPack);
      _cart.AddItem(bigMug);
      _cart.AddItem(vase);

      Assert.True(_cart.Items.Count > 0);

      _cart.EmptyCart();

      Assert.True(_cart.Items.Count == 0);
    }
  }
}
