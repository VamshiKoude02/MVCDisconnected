using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVCDisconnected.Models;

namespace MVCDisconnected.Controllers
{
    public class EmployeeController : Controller
    {
        CSDBDataContext cdb=new CSDBDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            List<Employee> emp = cdb.GetEmployees();
            return View(emp);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            cdb.InsertEmployee(emp);
            TempData["Message"] = "User created Sucessfully..!";
            return RedirectToAction("Details" , new {id=emp.Eno});
        }

        public ActionResult Details(int id)
        {
            List<Employee> emps = cdb.GetEmployees();
            var emp = emps.FirstOrDefault(x => x.Eno == id);
            if (TempData["Message"] == null)
            {
                TempData["Message"] = "Employee Information";
            }
            return View(emp);
        }        
        public ActionResult Update(int id)
        {
            List<Employee> emps = cdb.GetEmployees();
            var emp = emps.FirstOrDefault(x => x.Eno == id);
   
            return View(emp);
        }
        [HttpPost]
        public ActionResult Update(Employee employee)
        {
            List<Employee> emps = cdb.GetEmployees();
            var emp = emps.FirstOrDefault(x => x.Eno == employee.Eno);
            if(emp.Ename != employee.Ename || emp.Dname != employee.Dname || emp.Job != employee.Job || emp.Salary != employee.Salary )
            {
                cdb.UpdateEmployee(employee);
                TempData["Message"]="Updated Information";
                return RedirectToAction("Details", new { id = emp.Eno });                
            }
            else
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            cdb.DeleteEmployee(id);
            return RedirectToAction("List");
        }

    }
}