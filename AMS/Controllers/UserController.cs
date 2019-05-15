using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Data;
using AMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AMS.Controllers
{
    public class UserController : BaseController
    {

        // GET: User
        public async Task<IActionResult> Index()
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.UsersView)
            {
                return View(DAL.GetUsers());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: User/Details/5
        public async Task<IActionResult> Reset(int? id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (id != null)
            {
                if (CurrentUser.Role.UsersEdit || id == CurrentUser.ID)
                {

                    if (id == 1 && CurrentUser.ID != 1)
                    {
                        return PartialView("CannotAccessPage", "Shared");
                    }
                    else
                    {
                        return View(DAL.GetUserByID((int) id));
                    }
                }
            }
            else
            {
                return NotFound();
            }
            return PartialView("CannotAccessPage", "Shared");
        }


        //POST: User/Details
        //This takes in the new password for the user, and user.ID
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reset(int id, [Bind("ID,Password")] User user, string password)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.UsersEdit || id == CurrentUser.ID)
            {
                if (id != user.ID)
                {
                    return NotFound();
                }
                
                ModelState.Remove("Email");
                ModelState.Remove("FirstName");
                ModelState.Remove("LastName");
                ModelState.Remove("RoleID");

                if (ModelState.IsValid)
                {
                    DAL.UpdateUserPassword(id, password);
                    return RedirectToAction("Index", "User");
                }
            }
            return PartialView("CannotAccessPage", "Shared");
        }

        // GET: User/

        public IActionResult Create()
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.UsersAdd)
            {
                ViewBag.RoleID = new SelectList(DAL.GetRoles(), "ID", "Title");
                return View();
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Password,FirstName,LastName,RoleID,ID,Salt")] User user)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.UsersAdd)
            {
                ViewBag.RoleID = new SelectList(DAL.GetRoles(), "ID", "Title");
                if (ModelState.IsValid)
                {
                    DAL.AddUser(user);
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    return View();
                }
            }
            return PartialView("CannotAccessPage", "Shared");
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.UsersEdit || id == CurrentUser.ID)
            {
                if (id == 1 && CurrentUser.ID != 1)
                {
                    return PartialView("CannotAccessPage", "Shared");
                }

                ViewBag.RoleID = new SelectList(DAL.GetRoles(), "ID", "Title");
                if (id == null)
                {
                    return View(DAL.GetUserByID(CurrentUser.ID));
                }

                List<LoanedAsset> LLA = new List<LoanedAsset>(DAL.GetLoanedAssetAndUserByUserID((int)id));
                if (LLA.Count() != 0)
                {
                    ViewBag.LoanedAssets = LLA;
                }
                ViewBag.CurrentUserID = CurrentUser.ID;
                return View(DAL.GetUserByID((int)id));
            }
            return PartialView("CannotAccessPage", "Shared");
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Email,FirstName,LastName,RoleID,ID")] User user)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.UsersEdit || id == CurrentUser.ID)
            {
                if (id != user.ID)
                {
                    return NotFound();
                }

                // researched how to ignore a required bind
                // https://stackoverflow.com/questions/3166579/modelstate-isvalid-does-not-exclude-required-property
                ModelState.Remove("Password");

                if (ModelState.IsValid)
                {
                    if(id == CurrentUser.ID) { user.RoleID = CurrentUser.RoleID; }
                    DAL.UpdateUser(user);
                    List<LoanedAsset> LLA = new List<LoanedAsset>(DAL.GetLoanedAssetAndUserByUserID((int)id));
                    if (LLA.Count() != 0)
                    {
                        ViewBag.LoanedAssets = LLA;
                    }
                    return RedirectToAction("Index", "User");
                }
            }
            return PartialView("CannotAccessPage", "Shared");
        }

        //Return an asset
        public async Task<IActionResult> Return(int? id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }

            DAL.ReturnAsset((int)id);
            return RedirectToAction("Edit", "User", DAL.GetUserByID((int)id));
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (id == null)
            {
                return NotFound();
            }

            if (id != 1)
            {
                if (CurrentUser.Role.UsersDisable && CurrentUser.Email != DAL.GetUserByID(id).Email)
                {
                    DAL.DisableUserByID(id);
                    return RedirectToAction("Index");
                }
            }

            return PartialView("CannotAccessPage", "Shared");
        }

        // GET: All Disabled Users
        public async Task<IActionResult> DisabledUsers()
        {
            if (LoggedIn == false) { return RedirectToAction(actionName: "Index", controllerName: "Login"); }

            if (CurrentUser.Role.UsersView)
            {
                return View(model: DAL.GetAllDisabledUsers());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        /// <summary>
        /// Restores the user by the passed in ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> RestoreUser(int id)
        {
            if (LoggedIn == false) { return RedirectToAction(actionName: "Index", controllerName: "Login"); }

            if (CurrentUser.Role.UsersDisable)
            {
                DAL.RestoreUserByID(id);
                return RedirectToAction("Index", "User");
            }
            else return PartialView("CannotAccessPage", "Shared");
        }
    }
}
