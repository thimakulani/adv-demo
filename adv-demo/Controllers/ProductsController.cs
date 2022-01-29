using System;
using adv_demo.Models.Database;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace adv_demo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DemoEntities _db = new DemoEntities();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            var products = _db.Products;

            return View(await products.ToListAsync());
        }
        
        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.ProductCategoryID = new SelectList(_db.ProductCategories, "ProductCategoryID", "Name");
            ViewBag.ProductModelID = new SelectList(_db.ProductModels, "ProductModelID", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductID,Name,ProductNumber,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryID,ProductModelID,SellStartDate,SellEndDate,DiscontinuedDate,ModifiedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.rowguid = new Guid();
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProductCategoryID = new SelectList(_db.ProductCategories, "ProductCategoryID", "Name", product.ProductCategoryID);
            ViewBag.ProductModelID = new SelectList(_db.ProductModels, "ProductModelID", "Name", product.ProductModelID);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductCategoryID = new SelectList(_db.ProductCategories, "ProductCategoryID", "Name", product.ProductCategoryID);
            ViewBag.ProductModelID = new SelectList(_db.ProductModels, "ProductModelID", "Name", product.ProductModelID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductID,Name,ProductNumber,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryID,ProductModelID,SellStartDate,SellEndDate,DiscontinuedDate,ThumbNailPhoto,ThumbnailPhotoFileName,rowguid,ModifiedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(product).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategoryID = new SelectList(_db.ProductCategories, "ProductCategoryID", "Name", product.ProductCategoryID);
            ViewBag.ProductModelID = new SelectList(_db.ProductModels, "ProductModelID", "Name", product.ProductModelID);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var product = await _db.Products.FindAsync(id);
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
