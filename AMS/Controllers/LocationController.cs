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
    public class LocationController : BaseController
    {

        // GET: Location
        public async Task<IActionResult> Index()
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsView)
            {
                ViewBag.States = new SelectList(DAL.GetAllStates(), "ID", "Name");
                return View(DAL.GetAllLocations());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: Location/Create
        public IActionResult Create()
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsAdd)
            {
                ViewBag.States = new SelectList(DAL.GetAllStates(), "ID", "Name");
                return View();
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // POST: Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StateID,Name,ID")] Location location)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            //If the user can add assets then they can also add locations
            if (CurrentUser.Role.AssetsAdd)
            {
                ViewBag.States = new SelectList(DAL.GetAllStates(), "ID", "Name");
                if (ModelState.IsValid)
                {
                    DAL.AddLocation(location);
                    return RedirectToAction("Index", "Location");
                }
                else
                {
                    return View();
                }
            }
            return PartialView("CannotAccessPage", "Shared");
        }

        // GET: Location/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsView)
            {
                if (id == null)
                {
                    return NotFound();
                }
                ViewBag.States = new SelectList(DAL.GetAllStates(), "ID", "Name");
                return View(DAL.GetLocationByID((int)id));
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // POST: Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StateID,Name,ID")] Location location)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsEdit)
            {
                if (ModelState.IsValid)
                {

                    ViewBag.States = new SelectList(DAL.GetAllStates(), "ID", "Name");
                    DAL.UpdateLocation(location);
                    return View(DAL.GetLocationByID((int)id));
                }
                return View();
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: User/Delete/5
        /// <summary>
        /// This method deletes a Location. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            //If the user has the right permissions to archive assets they can also delete locations. 
            if (CurrentUser.Role.AssetsArchive)
            {
                List<Asset> assetCheck = DAL.GetAllAssets();
                List<Asset> assetWithActiveLocation = new List<Asset>();
                bool locationsFound = false;
                foreach (Asset asset in assetCheck)
                {
                    if (asset.LocationID == id)
                    {
                        assetWithActiveLocation.Add(asset);
                        locationsFound = true;
                    }
                }
                if (locationsFound == false)
                {
                    DAL.ArchiveLocationByID(id);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.DeletionError = DAL.GetLocationByID(id).Name;
                    ViewBag.DeletionModel = "location";
                    ViewBag.ModelAssociation = "an asset";
                    ViewBag.ActiveAssets = assetWithActiveLocation;
                    return PartialView("DeletionError", "Shared");
                }

            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: All archived Locations
        public async Task<IActionResult> ArchivedLocations()
        {
            if (LoggedIn == false) { return RedirectToAction(actionName: "Index", controllerName: "Login"); }

            if (CurrentUser.Role.AssetsView)
            {
                return View(model: DAL.GetAllArchivedLocations());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        /// <summary>
        /// Restores the Location by the passed in ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> RestoreLocation(int id)
        {
            if (LoggedIn == false) { return RedirectToAction(actionName: "Index", controllerName: "Login"); }

            if (CurrentUser.Role.AssetsArchive)
            {
                DAL.RestoreLocationByID(id);
                return RedirectToAction("Index", "Location");
            }
            else return PartialView("CannotAccessPage", "Shared");
        }
    }
}
