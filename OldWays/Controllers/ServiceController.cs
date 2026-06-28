using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OldWays.Data;
using OldWays.Models;

namespace OldWays.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ServiceController(ApplicationDbContext db)
        {
            _db = db;
        }

        //grouped by category in controller
        //search bar
        public IActionResult Index(string search)
        {
            var services = _db.Services.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                services = services.Where(s => s.Name.ToLower().Contains(search.ToLower()));
            }

            var grouped = services.ToList().GroupBy(s => s.Category);
            return View(grouped);
        }


        //create
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if (ModelState.IsValid)
            {
                _db.Services.Add(service);
                await _db.SaveChangesAsync();
                TempData["success"] = "Service created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        //edit view
        public IActionResult Edit(int? id)
        {

            //finds one and returns error if more then one
            Service service = _db.Services.SingleOrDefault(s => s.Id == id);
            //Service service1 = _db.Services.Where(u=>u.Id == id).FirstOrDefault();  at 3:09 time
            //Service? service = _db.Services.Find(id);
            //Service service1 = _db.Services.FirstOrDefault(s => s.Id == id); finds one and returns it
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        //edit post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Service obj)
        {
            if (id != obj.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _db.Services.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Service update successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        //delete
        [HttpPost]
        public IActionResult Delete(Service obj)
        {
            _db.Services.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Service deleted successfully";
            return RedirectToAction("Index");
        }
    }

}

