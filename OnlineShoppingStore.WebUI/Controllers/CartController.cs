using OnlineShoppingStore.Domain.Abstract;
using OnlineShoppingStore.Domain.Entities;
using OnlineShoppingStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShoppingStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor processor;

        public CartController(IProductRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            processor = proc;

        }

        public ViewResult Index(Cart cart, string ReturnUrl)
        {
            return View(new CartIndexViewModel { Cart = cart, ReturnUrl = ReturnUrl } ) ;
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }
        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                processor.ProcessOrder(cart, shippingDetails);
                cart.clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

        public RedirectToRouteResult AddToCart(Cart cart, int ProductID, string ReturnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == ProductID);

            if(product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { ReturnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int ProductID, string ReturnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == ProductID);

            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { ReturnUrl });
        }

        //private Cart GetCart()
        //{
        //    Cart cart = (Cart)Session["Cart"];
        //    if(cart == null)
        //    {
        //        cart = new Cart();
        //        Session["Cart"] = cart;
        //    }
        //    return cart;
        //}
    }
}