using System;
using System.Data;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using ShoppingSite.Models;
using ShoppingSite.Validations;

namespace ShoppingSite.Controllers
{
    public class UsersController : Controller
    {
        private UserDBContext _context = new UserDBContext();

        public ActionResult ProductList()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            var alreadyInCart = _context.Carts.Where(q => q.UserId == UserId).Select(s => s.ProductId).ToList();
            ViewBag.alreadyInCart = alreadyInCart;
            Session["alreadyInCartCount"] = alreadyInCart.Count();
            return View(_context.Products.ToList().Where(visible => visible.IsVisible == "True"));
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public ActionResult Register([Bind(Include = "UserId,UserName,Email,Password,ConfirmPassword")] User user)
        {
           user.UserType = "User";
            if (ModelState.IsValid)
            {
                            //user.Password = AESCryptography.Encrypt(user.Password);
                            //user.ConfirmPassword = AESCryptography.Encrypt(user.ConfirmPassword);
                            user.Password = Crypto.Hash(user.Password, "MD5");
                            user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword, "MD5");
                            _context.Users.Add(user);
                            _context.SaveChanges();
                            ViewBag.Message = user.UserName + " Registered Succesfully";                           
                            return View();
            }

            else
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                ViewBag.message = errors;
            }
            return View();
        }

        [ImportModelState]
        public ActionResult Login()
        {
            
            return View();
        }

        [ExportModelState]
        [HttpPost]
        [Obsolete]
        public ActionResult Login(User user)
        {
            using (UserDBContext db = new UserDBContext())
            {
                var get_user = db.Users.FirstOrDefault(username => username.UserName == user.UserName);
                if (get_user != null)
                {
                        if (get_user.Password ==  Crypto.Hash(user.Password, "MD5"))
                    {
                        FormsAuthentication.SetAuthCookie(user.UserName, false);
                        Session["UserId"] = get_user.UserId.ToString();
                        Session["UserType"] = get_user.UserType.ToString();
                        Session["UserName"] = get_user.UserName.ToString();
                        if (get_user.UserType.Equals("Admin"))
                        {
                            TempData["Message"] = Session["UserName"]+ " Logged In Succesfully";
                            return RedirectToAction("ProductList","Admin");
                        }
                        else
                        {
                            TempData["Message"] = Session["UserName"]+ " Logged In Succesfully";
                            return RedirectToAction("ProductList");
                        }                                                    
                    }
                    else
                    {
                        ModelState.AddModelError("", "UserName or Password does not match.");
                        //ViewBag.Message= "UserName or Password does not match";
                    }
                }
                else
                {
                    ModelState.AddModelError("", "UserName does not Exists");
                    //ViewBag.Message= "UserName does not Exists";
                }
            }          
           return View();
            
        }
 

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session["Message"] = Session["UserName"] + " Logged Out Succesfully";
            return RedirectToAction("Login");
        }
   
        public ActionResult AddCart(int? id)
        {       
            Product product = _context.Products.Find(id);
            //object obj = Session["UserId"];
            //if (obj != null)
            //{
                _context.Carts.Add(
                new Cart
                {
                    ProductId = product.ProductId,
                    UserId = Convert.ToInt32(Session["UserId"]),
                });
                _context.SaveChanges();
            TempData["Message"] = product.ProductName+ "Added To Cart ";
            return RedirectToAction("ProductList");
            //}
            //else
            //{
            //    Session["Error"] = "You need to login first  ";
            //    return RedirectToAction("Login");
            //}

        }
      
        public ActionResult MyCart()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            var query = from carts in _context.Carts join products in _context.Products on carts.ProductId equals products.ProductId where carts.UserId== UserId
                        select products;          
            return View(query);
        }
           
        public ActionResult RemoveFromCart(int id)
        {
            var query = (from carts in _context.Carts where carts.ProductId == id select carts).FirstOrDefault();
            _context.Carts.Remove(query);
            _context.SaveChanges();
            TempData["Message"] = "Product Removed";
            int UserId = Convert.ToInt32(Session["UserId"]);
            var alreadyInCart = _context.Carts.Where(q => q.UserId == UserId).Select(s => s.ProductId).ToList();
            Session["alreadyInCartCount"] = alreadyInCart.Count();
            return RedirectToAction("MyCart");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
