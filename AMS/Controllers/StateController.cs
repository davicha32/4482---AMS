using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Data;
using AMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AMS.Controllers
{
    /// <summary>
    /// This controller is used to edit and track the states of an Asset.
    /// </summary>
    public class StateController : BaseController
    {

        // GET: State
        public async Task<IActionResult> Index()
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            ///If the current user can view Assets, then return the States of the Asset. 
            if (CurrentUser.Role.AssetsView)
            {
                return View(DAL.GetAllStates());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

       
        /// <summary>
        /// We didn't see the need to create additional states, so we elected to leave this commented out. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: State/Create
        public IActionResult Create()
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsAdd)
            {
                return View();
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // POST: State/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ID")] State state)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            //If the user can add assets then they can also add brands
            if (CurrentUser.Role.AssetsAdd)
            {
                if (ModelState.IsValid)
                {
                    DAL.AddState(state);
                    return RedirectToAction("Index", "State");
                }
                else
                {
                    return View();
                }
            }
            return PartialView("CannotAccessPage", "Shared");
        }

        /// This isn't utilized at this point. 

        // GET: State/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsView)
            {
                if (id == null)
                {
                    return NotFound();
                }

                return View(DAL.GetStateByID((int)id));
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        /// <summary>
        /// This Method has the ability to edit states. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>

        // POST: State/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,ID")] State state)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsEdit)
            {
                if (ModelState.IsValid)
                {
                    DAL.UpdateState(state);
                    return View(DAL.GetStateByID((int)id));
                }
                return View();
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: State/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            //If the user has the right permissions to archive assets they can also delete brands. 
            if (CurrentUser.Role.AssetsArchive)
            {
                List<Asset> assetCheck = DAL.GetAllAssets();
                List<Asset> assetsWithActiveState = new List<Asset>();
                bool statesFound = false;
                foreach (Asset asset in assetCheck)
                {
                    if (asset.StateID == id)
                    {
                        assetsWithActiveState.Add(asset);
                        statesFound = true;
                    }
                }
                List<Location> locationCheck = DAL.GetAllLocations();
                List<Location> locationsWithActiveState = new List<Location>();
                foreach (Location location in locationCheck)
                {
                    if (location.StateID == id)
                    {
                        locationsWithActiveState.Add(location);
                        statesFound = true;
                    }
                }
                if (statesFound == false)
                {
                    DAL.ArchiveStateByID(id);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.DeletionError = DAL.GetStateByID(id).Name;
                    ViewBag.DeletionModel = "state";
                    ViewBag.ModelAssociation = "an asset or location";
                    ViewBag.ActiveAssets = assetsWithActiveState;
                    ViewBag.ActiveLocations = locationsWithActiveState;
                    return PartialView("DeletionError", "Shared");
                }
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: All archived States
        public async Task<IActionResult> ArchivedStates()
        {
            if (LoggedIn == false) { return RedirectToAction(actionName: "Index", controllerName: "Login"); }

            if (CurrentUser.Role.AssetsView)
            {
                return View(model: DAL.GetAllArchivedStates());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        /// <summary>
        /// Restores the State by the passed in ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> RestoreState(int id)
        {
            if (LoggedIn == false) { return RedirectToAction(actionName: "Index", controllerName: "Login"); }

            if (CurrentUser.Role.AssetsArchive)
            {
                DAL.RestoreStateByID(id);
                return RedirectToAction("Index", "State");
            }
            else return PartialView("CannotAccessPage", "Shared");
        }
    }
}
