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
    /// This controller is used to CRUD Devices and their corresponding logic.
    /// </summary>
    public class DeviceController : BaseController
    {

        // GET: Device
        /// <summary>
        /// If there is a logged in user that can view assets, then display AllDevices. 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsView)
            {
                ViewBag.Brands = new SelectList(DAL.GetAllBrands(), "ID", "Name");
                return View(DAL.GetAllDevices());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: Device/Create
        public IActionResult Create()
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsAdd)
            {
                ViewBag.Brands = new SelectList(DAL.GetAllBrands(), "ID", "Name");
                return View();
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // POST: Device/Create
        /// <summary>
        /// If the logged in uder can add assets, then call the DAL.AddDevice. 
        /// </summary>
        /// <param name="DeviceModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Create([Bind("BrandID,Name,ID")] Device DeviceModel /*int brandID, string deviceName*/)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsAdd == true)
            {
                ViewBag.Brands = new SelectList(DAL.GetAllBrands(), "ID", "Name");
                if (ModelState.IsValid)
                {
                    
                    DAL.AddDevice(DeviceModel);
                    return RedirectToAction("Index", "Device");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return PartialView("CannotAccessPage", "Shared");
            }
        }

        // GET: Device/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsView)
            {
                if (id == null)
                {
                    return NotFound();
                }
                ViewBag.Brands = new SelectList(DAL.GetAllBrands(), "ID", "Name");
                return View(DAL.GetDeviceByID((int)id));
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // POST: Device/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrandID,Name,ID")] Device device)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsEdit)
            {
                if (ModelState.IsValid)
                {
                    ViewBag.Brands = new SelectList(DAL.GetAllBrands(), "ID", "Name");
                    DAL.UpdateDevice(device);
                    return View(DAL.GetDeviceByID((int)id));
                }
                return View();
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: Device/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            //If the user has the right permissions to archive assets they can also delete devices. 
            if (CurrentUser.Role.AssetsArchive)
            {
                List<Model> deviceCheck = DAL.GetAllModels();
                List<Model> modelWithActiveDevice = new List<Model>();
                bool devicesFound = false;
                foreach(Model model in deviceCheck)
                {
                    if (model.DeviceID == id)
                    {
                        modelWithActiveDevice.Add(model);
                        devicesFound = true;
                    }
                }
                if (devicesFound == false)
                {
                    DAL.ArchiveDeviceByID(id);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.DeletionError = DAL.GetDeviceByID(id).Name;
                    ViewBag.DeletionModel = "device";
                    ViewBag.ModelAssociation = "a model";
                    ViewBag.ActiveModels = modelWithActiveDevice;
                    return PartialView("DeletionError", "Shared");
                }
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: All archived Devices
        public async Task<IActionResult> ArchivedDevices()
        {
            if (LoggedIn == false) { return RedirectToAction(actionName: "Index", controllerName: "Login"); }

            if (CurrentUser.Role.AssetsView)
            {
                return View(model: DAL.GetAllArchivedDevices());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        /// <summary>
        /// Restores the Device by the passed in ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> RestoreDevice(int id)
        {
            if (LoggedIn == false) { return RedirectToAction(actionName: "Index", controllerName: "Login"); }

            if (CurrentUser.Role.AssetsArchive)
            {
                DAL.RestoreDeviceByID(id);
                return RedirectToAction("Index", "Device");
            }
            else return PartialView("CannotAccessPage", "Shared");
        }
    }
}
