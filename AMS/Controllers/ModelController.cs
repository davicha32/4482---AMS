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
    public class ModelController : BaseController
    {

        // GET: Model
        public async Task<IActionResult> Index()
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsView)
            {
                return View(DAL.GetAllModels());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: Model/Create
        public IActionResult Create()
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsAdd)
            {
                ViewBag.Devices = new SelectList(DAL.GetAllDevices(), "ID", "Name");
                return View();
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // POST: Model/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeviceID,Name,ID")] Model model)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsAdd == true)
            {
                ViewBag.Devices = new SelectList(DAL.GetAllDevices(), "ID", "Name");
                if (ModelState.IsValid)
                {
                    DAL.AddModel(model);
                    return RedirectToAction("Index", "Model");
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

        // GET: Model/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsView)
            {
                if (id == null)
                {
                    return NotFound();
                }
                ViewBag.Devices = new SelectList(DAL.GetAllDevices(), "ID", "Name");
                return View(DAL.GetModelByID((int)id));
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // POST: Model/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeviceID,Name,ID")] Model model)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsEdit)
            {
                ViewBag.Devices = new SelectList(DAL.GetAllDevices(), "ID", "Name");
                if (ModelState.IsValid)
                {
                    DAL.UpdateModel(model);
                    return View(DAL.GetModelByID((int)id));
                }
                return View();
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: Model/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            //If the user has the right permissions to archive assets they can also delete Models. 
            if (CurrentUser.Role.AssetsArchive)
            {
                List<Asset> assetCheck = DAL.GetAllAssets();
                List<Asset> assetsWithActiveModels = new List<Asset>();
                bool modelsFound = false;
                foreach (Asset asset in assetCheck)
                {
                    if (asset.ModelID == id)
                    {
                        assetsWithActiveModels.Add(asset);
                        modelsFound = true;
                    }
                }
                if (modelsFound == false)
                {
                    DAL.ArchiveModelByID(id);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.DeletionError = DAL.GetModelByID(id).Name;
                    ViewBag.DeletionModel = "model";
                    ViewBag.ModelAssociation = "an asset";
                    ViewBag.ActiveAssets = assetsWithActiveModels;
                    return PartialView("DeletionError", "Shared");
                }
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: All archived Models
        public async Task<IActionResult> ArchivedModels()
        {
            if (LoggedIn == false) { return RedirectToAction(actionName: "Index", controllerName: "Login"); }

            if (CurrentUser.Role.AssetsView)
            {
                return View(model: DAL.GetAllArchivedModels());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        /// <summary>
        /// Restores the Model by the passed in ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> RestoreModel(int id)
        {
            if (LoggedIn == false) { return RedirectToAction(actionName: "Index", controllerName: "Login"); }

            if (CurrentUser.Role.AssetsArchive)
            {
                DAL.RestoreModelByID(id);
                return RedirectToAction("Index", "Model");
            }
            else return PartialView("CannotAccessPage", "Shared");
        }
    }
}
