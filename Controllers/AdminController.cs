
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Linq.Dynamic;
using ShoppingSite.Models;
using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Web.Security;
using System.Threading.Tasks;
using System.Text;

namespace ShoppingSite.Controllers
{
    [AccessDeniedAuthorization(Roles = "Admin")]
    public class AdminController : Controller
    {
        private UserDBContext _context = new UserDBContext();

        public ActionResult ProductList()
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            var alreadyInCart = _context.Carts.Where(q => q.UserId == UserId).Select(s => s.ProductId).ToList();
            ViewBag.alreadyInCart = alreadyInCart;
            Session["alreadyInCartCount"] = alreadyInCart.Count();
            return View(_context.Products.ToList());
        }
        public ActionResult UserDataTable()
        {         
                return View(_context.Users.ToList());
        }

        [HttpPost]
        public ActionResult LoadData()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            //find search columns info
            var getvalue = Request.Form.GetValues("search[value]").FirstOrDefault();       

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            using (UserDBContext _context = new UserDBContext())
            {
                // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
                var getUser = (from users in _context.Users select users);
              
                if (!string.IsNullOrEmpty(getvalue))
                {
                    getUser = getUser.Where(user => user.UserName.Contains(getvalue) || user.Email.Contains(getvalue) 
                               || user.UserType.Contains(getvalue) || user.UserId.ToString().Contains(getvalue) );
                }
                //SORT
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    getUser = getUser.OrderBy(sortColumn + " " + sortColumnDir);
                }

                recordsTotal = getUser.Count();
                var data = getUser.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]       
        public ActionResult AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    ViewBag.Message = product.ProductName+" Product Added";
                    return View();
             }                          
            return View();
        }
      

     
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {     
            if (ModelState.IsValid)
            {
                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();
                ViewBag.Message = product.ProductName+" product Edited";
                return View();
            }
            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }         
            Product product = _context.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProduct(int id)
        {         
            Product product = _context.Products.Find(id);
            _context.Products.Remove(product);
            TempData["MessageRemove"] = product.ProductName + " product Deleted";
            _context.SaveChanges();
            return RedirectToAction("ProductList");
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
            TempData["Message"] = product.ProductName + " Added To Cart";
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
            var query = from carts in _context.Carts
                        join products in _context.Products on carts.ProductId equals products.ProductId
                        where carts.UserId == UserId
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
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session["Message"] = Session["UserName"] + " Logged Out Succesfully";
            return RedirectToAction("Login","Users");
        }

        public ActionResult UploadData()
        {
            return View();
        }


        [HttpPost]
        public ActionResult UploadData(HttpPostedFileBase UploadedFile)
        {
            string filePath = string.Empty;
           if (UploadedFile != null)
           {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    filePath = path + Path.GetFileName(UploadedFile.FileName);
                    string extension = Path.GetExtension(UploadedFile.FileName);
                    UploadedFile.SaveAs(filePath);                    
                    if (extension == ".csv")
                    {
                        try
                        {
                            Task<Product> task = Task.Run(async () => await UploadProducts(UploadedFile));
                            ViewBag.Message = "File uploaded successfully";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = "ERROR:" + ex.Message.ToString();
                        }
                    }
                    else
                    {
                        ViewBag.MessageError = "Please upload csv file only";
                    }
            }
            else
            {
                ViewBag.MessageError = "Please choose a file to upload";
            }
            return View();

        }

        private static async Task<Product> UploadProducts(HttpPostedFileBase UploadedFile)
        {
            Product pro = null;
            await Task.Run(() =>
            {
                using (var context = new UserDBContext())
                {
                    string csvData;
                    var fileStream = UploadedFile.InputStream;
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        csvData = streamReader.ReadToEnd();
                        IList<string> data = new List<string>();
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            string[] itemParts = row.Split(',');
                            var checkProduct = itemParts[0];
                            var getProduct = context.Products.FirstOrDefault(product => product.ProductName == checkProduct);
                            var check = data.Contains(checkProduct);
                            if (getProduct == null && check == false)
                            {
                                data.Add(itemParts[0].ToString());
                                context.Products.Add(new Product { ProductName = itemParts[0].ToString(), ProductPrice = float.Parse(itemParts[1]), DiscountPrice = float.Parse(itemParts[2]), AdditionalDiscount = float.Parse(itemParts[3]), IsVisible = itemParts[4] });
                            }
                        }
                    }
                    context.SaveChanges();
                    }
                }
            });
            return pro;
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

