using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Data;
using AMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AMS;
using MySql.Data.MySqlClient;

namespace AMS.Controllers
{
    public class LoginController : BaseController
    {
		// GET: Login
		public IActionResult Index()
		{
			return View();
		}

        // GET: Login/Create
        public IActionResult Create()
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Password,FirstName,LastName,RoleID,ID")] User user)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (ModelState.IsValid)
            {
            }
            return View(user);
        }

        // GET: Login/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (id == null)
            {
                return NotFound();
            }

            return View();
        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Email,Password,FirstName,LastName,RoleID,ID")] User user)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

		/// <summary>
		/// Login Logic from the Controller
		/// </summary>
		/// <returns>This returns a successful login/ unsuccessful login</returns>
		[HttpPost]
		public RedirectToActionResult Login(string Email, string Password)
		{
			User e = DAL.GetUser(Email, Password);
            CurrentUser = e;
            DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
            if (e != null && e.DateDisabled == maxDate)
			{
                LoggedIn = true;
				return RedirectToAction("Index", "Ticket");
			} else
			{
				return RedirectToAction("Index", "Login");
			}
		}

        [HttpGet]
        public RedirectToActionResult Logout()
        {
            CurrentUser = null;
            LoggedIn = false;
            return RedirectToAction("Index", "Login");
        }
    }
}
