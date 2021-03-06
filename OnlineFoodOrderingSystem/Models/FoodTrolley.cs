﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using OnlineFoodOrderingSystem.DAL;
using OnlineFoodOrderingSystem.Models;

namespace OnlineFoodOrderingSystem.Models
{
    public partial class FoodBasket //basket because punny
    {
        Food_Ordering db = new Food_Ordering(); 

        public string FoodCartID { get; set; }

        public const string CartSessionKey = "cartId"; //session 

        public static FoodBasket GetCart(HttpContextBase context) //base class for HTTP requests  
        {
            var cart = new FoodBasket();
            cart.FoodCartID = cart.GetCartId(context);

            return cart;
        }

        public static FoodBasket GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(MenuItem menuItemID)
        {
            var cartItem = db.Carts.SingleOrDefault(c=>c.CartId == FoodCartID && c.MenuItemID == menuItemID.Id);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    MenuItemId = menuItemID.Id,
                    CartId = FoodCartID,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }

            db.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            var cartItem = db.Carts.SingleOrDefault(cart => cart.CartId == FoodCartID && cart.MenuItemID == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    db.Carts.Remove(cartItem);
                }

                db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(cart => cart.CartId == FoodCartID);

            foreach (var cartItem in cartItems)
            {
                db.Cart.Remove(cartItem);
            }
            db.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(cart => cart.CartId == FoodCartID).ToList();
        }

        public int GetCount()
        {
            int? count =
                (from cartItems in db.Carts where cartItems.CartId == FoodCartID select (int?) cartItems.Count).Sum();

            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartItems in db.Carts
                where cartItems.CartId == FoodCartID
                              select (cartItems.Count * cartItems.Menu.Price)).Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(EmployeeOrder EmployeeOrder)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();

            foreach (var item in cartItems)
            {
                var orderedFoood = new OrderedFoood
                {
                    MenuItemId = item.MenuItemID, EmployeeOrderId = EmployeeOrder.Id, Quantity = item.Count
                };

                orderTotal += (item.Count*item.Menu.Price);

                db.OrderedFoood.Add(orderedFoood);
            }

            EmployeeOrder.Amount = orderTotal;

            db.SaveChanges();

            EmptyCart();

            return EmployeeOrder.Id;
        }

        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }

                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        public void MigrateCart(string userName)
        {
            var fooodCart = db.Carts.Where(c => c.CartId == FoodCartID);
            foreach (Cart item in fooodCart)
            {
                item.CartId = userName;
            }

            db.SaveChanges();
        }

    }
}