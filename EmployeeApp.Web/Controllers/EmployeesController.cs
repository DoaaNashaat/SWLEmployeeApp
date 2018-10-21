using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeApp.Models.EmployeeDAL;
using EmployeeApp.Models.ViewModel;

namespace EmployeeApp.Controllers
{
    public class EmployeesController : Controller
    {
        private EmployeeDBEntities db = new EmployeeDBEntities();


        // GET: Employees
        public ActionResult Index()
        {
            
            List<VMEmployee> VMEmployees = new List<VMEmployee>();

            VMEmployees = db.Employees.Include(e => e.City).AsEnumerable().Select(dataRow => new VMEmployee
            {
                EmployeeID = dataRow.Id,
                Name = dataRow.Name,
                Salary = dataRow.Salary,
                Email = dataRow.Email,
                City = dataRow.City.Name,
                Country = dataRow.City.Country.Name,
               

            }).ToList();

            return View(VMEmployees.ToList());
        }


        #region Add Employee
        // GET: Employees/Create
        public ActionResult Create()
        {
            
            return View(new VMEmployee{ CountryList = db.Countries.ToList()});
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,CityID,Name,Email,Salary")] VMEmployee vmemployee)
        {
            if (ModelState.IsValid)
            {
                
                Employee employee = new Employee();
                employee.Name = vmemployee.Name;
                employee.Email = vmemployee.Email;
                employee.Salary = vmemployee.Salary;
                employee.CityID = vmemployee.CityID;

                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

           
            return View(vmemployee);
        }
        #endregion

        #region Edit Employee
        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(new VMEmployee { CountryList = db.Countries.ToList(),
                CityList = db.Cities.ToList(),

                EmployeeID = id.Value,
                Name = employee.Name,
                Salary = employee.Salary,
                Email  = employee.Email,
                CountryID = employee.City.CountryID.Value,
                CityID  = employee.CityID.Value
            });
           
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,CityID,Name,Email,Salary")] VMEmployee vmemployee)
        {
            if (ModelState.IsValid)
            {
                Employee employee = db.Employees.Find(vmemployee.EmployeeID);


                employee.Name = vmemployee.Name;
                employee.Email = vmemployee.Email;
                employee.Salary = vmemployee.Salary;
                employee.CityID = vmemployee.CityID;

                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(vmemployee);
        }
        #endregion

        #region Delete One Employee
        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion


        #region Delete Multiple Employee
        [HttpPost]
        public ActionResult MultipleDelete(int[] deleteIDs)
        {
           // array of employees id's to be deleted
            if (deleteIDs != null && deleteIDs.Length > 0)
            {
                foreach (int id in deleteIDs)
                {
                    Employee employee = db.Employees.Find(id);
                    db.Employees.Remove(employee);
                    
                }
                db.SaveChanges();
            }
            // And finally, redirect to the action that lists the books
            // (let's assume it's Index)
            return RedirectToAction("Index");
        }
        #endregion


        [HttpPost]
        public ActionResult GetCityByCountryId(int CountryID)
        {
            try
            {
                List<City> objcity = new List<City>();
                objcity = db.Cities.ToList().Where(m => m.CountryID == CountryID).ToList();
                SelectList obgcity = new SelectList(objcity, "Id", "Name", 0);
                return Json(obgcity);
            }
            catch (Exception)
            {
                
            }
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
