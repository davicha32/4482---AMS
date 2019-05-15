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
    /// This is the controller used to track Brands. It inherits from the Base Controller. 
    /// </summary>
    public class BrandController : BaseController
    {

        // GET: Brand
        /// <summary>
        /// If there is a logged in user who can view Assets, then:
        /// Else, return the partial view "CannotAccessPage" 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsView)
            {
                return View(DAL.GetAllBrands());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: Brand/Create
        public IActionResult Create()
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsAdd)
            {
                return View();
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // POST: Brand/Create
        /// <summary>
        /// This method posts a new brand that is created. 
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ID")] Brand brand)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            //If the user can add assets then they can also add brands
            if (CurrentUser.Role.AssetsAdd)
            {
                if (ModelState.IsValid)
                {
                    DAL.AddBrand(brand);
                    return RedirectToAction("Index", "Brand");
                }
                else
                {
                    return View();
                }
            }
            return PartialView("CannotAccessPage", "Shared");
        }

        // GET: Brand/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsView)
            {
                if (id == null)
                {
                    return NotFound();
                }

                return View(DAL.GetBrandByID((int)id));
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // POST: Brand/Edit/5
        // If the current user can edit assets, and the model state is valid, then call the UpdateBrand DAL call. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,ID")] Brand brand)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsEdit)
            {
                if (ModelState.IsValid)
                {
                    DAL.UpdateBrand(brand);
                    return View(DAL.GetBrandByID((int)id));
                }
                return View();
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: Brand/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            //If the user has the right permissions to archive assets they can also delete brands. 
            if (CurrentUser.Role.AssetsArchive)
            {
                List<Device> deviceCheck = DAL.GetAllDevices();
                List<Device> deviceWithActiveBrands = new List<Device>();
                bool brandsFound = false;
                foreach(Device device in deviceCheck)
                {
                    if (device.BrandID == id)
                    {
                        deviceWithActiveBrands.Add(device);
                        brandsFound = true;
                    }
                }
                if (brandsFound == false)
                {
                    DAL.ArchiveBrandByID(id);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.DeletionError = DAL.GetBrandByID(id).Name;
                    ViewBag.DeletionModel = "brand";
                    ViewBag.ModelAssociation = "a device type";
                    ViewBag.ActiveDevices = deviceWithActiveBrands;
                    return PartialView("DeletionError", "Shared");
                }
                
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: All archived Brands
        public async Task<IActionResult> ArchivedBrands()
        {
            if (LoggedIn == false) { return RedirectToAction(actionName: "Index", controllerName: "Login"); }

            if (CurrentUser.Role.AssetsView)
            {
                return View(model: DAL.GetAllArchivedBrands());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        /// <summary>
        /// Restores the Brand by the passed in ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> RestoreBrand(int id)
        {
            if (LoggedIn == false) { return RedirectToAction(actionName: "Index", controllerName: "Login"); }

            if (CurrentUser.Role.AssetsArchive)
            {
                DAL.RestoreBrandByID(id);
                return RedirectToAction("Index", "Brand");
            }
            else return PartialView("CannotAccessPage", "Shared");
        }
    }
}
