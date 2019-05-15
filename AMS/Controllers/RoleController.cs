using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using AMS.Data;
using AMS.Models;

//This code is from Jon Holmes. Initially. Then Jack Bradley wrote all sorts of code all over the place with the help of Chas!
/// <summary>
/// This Controller is used to track and utilize Roles. 
/// </summary>
namespace AMS.Controllers
{
    public class RoleController : BaseController
    {


        // GET: Role
        public async Task<IActionResult> Index()
        {
            ///If there isn't a logged in user, then redirect to the login page. 
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.RolesView)
            {
                return View(DAL.GetRoles());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: Role/Create
        public IActionResult Create()
        {
            ///If there isn't a logged in user, then redirect to the login page. 
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.RolesView)
            {
                return View();
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // POST: Role/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,TicketsView,TicketsComment,TicketsResolve,TicketsOpen,TicketsEdit,AssetsView,AssetsAdd,AssetsArchive,UsersView,UsersAdd,UsersDisable,RolesView")] Role role)
        {
            ///If there isn't a logged in user, then redirect to the login page. 
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            ///If the current user can view roles, then create a new role! This is an appropriate role check, because Administrators are the only users who can view roles, and they are the only ones who will be able to create them as a result.
            if (CurrentUser.Role.RolesView)
            {
                if (ModelState.IsValid)
                {
                    DAL.AddRoles(role);
                }
                else
                {
                    return View();
                }
                return RedirectToAction("Index", "Role");
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ///If there isn't a logged in user, then redirect to the login page. 
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            /// If the current user can view roles, and the RoleID isn't null, then display the role by calling the GetRoleByID DAL Call. 
            if (CurrentUser.Role.RolesView)
            {
                if (id == null)
                {
                    return NotFound();
                }

                return View(DAL.GetRoleByID((int)id));
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,TicketsView,TicketsComment,TicketsResolve,TicketsOpen,TicketsEdit,AssetsView,AssetsAdd,AssetsEdit,AssetsArchive,UsersView,UsersAdd,UsersEdit,UsersDisable,RolesView")] Role role)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            ///If the current user can View roles, and the role they have selected isn't their own role, then:
            if (CurrentUser.Role.RolesView && CurrentUser.RoleID != id)
            {
                if (id != role.ID)
                {
                    return NotFound();
                }
                ///If the model state is valid, then Update the Role using the appropriate DAL call, and then redirect to the edit roles page with the updated edits. 
                if (ModelState.IsValid)
                {
                    DAL.UpdateRole(role);
                    return RedirectToAction("Edit", "Role");
                }
                return View();
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: User/Delete/5
        /// <summary>
        /// This method deletes a Role. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            //If the user has the right permissions to View roles, and the current role isn't the role they are trying to delete. 
            if (CurrentUser.Role.RolesView && CurrentUser.RoleID != id)
            {
                /// if the DeleteRole ID equals -1, or the CurrentUser's role ID, then redirect them to the partial view that says they can't access this page.                     
                if (CurrentUser.RoleID == id)
                {
                    return PartialView("CannotAccessPage", "Shared");
                }
                /// Else, delete the Role based on the select ID using a DAL call, and then redirect to the index page. 
                else
                {
                    List<User> userCheck = DAL.GetUsers();
                    List<User> usersWithActiveRole = new List<User>();
                    bool rolesFound = false;
                    foreach (User user in userCheck)
                    {
                        if (user.RoleID == id)
                        {
                            usersWithActiveRole.Add(user);
                            rolesFound = true;
                        }
                    }
                    if (rolesFound == false)
                    {
                        DAL.DeleteRoleByID(id);
                        return RedirectToAction("Index", "Role");
                    }
                    else
                    { 
                        ViewBag.DeletionError = DAL.GetRoleByID(id).Title;
                        ViewBag.DeletionModel = "role";
                        ViewBag.ModelAssociation = "user";
                        ViewBag.ActiveUsers = usersWithActiveRole;
                        return PartialView("DeletionError", "Shared");
                    }
                }

            }
            else return PartialView("CannotAccessPage", "Shared");
        }
    }
}
