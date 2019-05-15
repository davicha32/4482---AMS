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
    public class AssetController : BaseController
    {

        // GET: Assets
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsView)
            {
                return View(DAL.GetAllAssets());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        //public IActionResult Details() => View();

        // GET: Asset/Create
        public IActionResult Create()
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            //Generate a new view bag of Brands, and if the Current user can add an asset, then that info is displayed.
            ViewBag.BrandID = new SelectList(DAL.GetAllBrands(), "ID", "Name");
            if (CurrentUser.Role.AssetsAdd)
            {
                return View();
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // POST: Asset/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InventoryNumber,DatePurchased,DateWarrantyExpires,IsLoanable,ModelID,StateID,LocationID,ID")] Asset Asset)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsAdd == true)
            {
                if (InputsAreValid(Asset.InventoryNumber, Asset.DatePurchased) == true)
                {
                    if (ModelState.IsValid)
                    {
                        DAL.AddAsset(Asset);
                        return RedirectToAction("Index");
                    }

                }
                return View();
            }

            else
            {
                return PartialView("CannotAccessPage", "Shared");
            }
        }

        // GET: Asset/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (id == null)
            {
                return NotFound();
            }
            else if (CurrentUser.Role.AssetsEdit)
            {
                {
                    LoanedAsset LA = DAL.GetLoanedAssetAndUserByAssetID((int)id);
                    if (LA != null)
                    {
                        ViewBag.LoanedAsset = LA;
                    }

                    return View(DAL.GetAssetByID((int)id));
                }
            }
            return PartialView("CannotAccessPage", "Shared");
        }

        // POST: Asset/Edit/5
        /// <summary>
        /// if the user is logged in, and has the right permissions to edit an asset, and the model is valid,
        /// Next, a Loaned Asset variable is created, if it is selected, then that Asset Becomes loanable.
        /// Finally, the asset is updated appropriately. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Asset"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InventoryNumber,DatePurchased,DateWarrantyExpires,IsLoanable,ModelID,StateID,LocationID,ID")] Asset Asset)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.AssetsEdit)
            {
                if (ModelState.IsValid)
                {
                    LoanedAsset LA = DAL.GetLoanedAssetAndUserByAssetID((int)id);
                    if (LA != null)
                    {
                        ViewBag.LoanedAsset = LA;
                        Asset.IsLoanable = true;
                    }
                    DAL.UpdateAsset(Asset);
                    return View(DAL.GetAssetByID((int)id));
                }

                return View();
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: Asset/Loan/5
        public async Task<IActionResult> Loan(int? id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (id == null)
            {
                return NotFound();
            }
            return View(DAL.GetAssetByID((int)id));
        }

        //Loan out an asset
        /// <summary>
        /// Written by Paul Michael. This allows the user to loan out an asset, and it sets the asset to no
        /// longer be available. 
        /// </summary>
        /// <param name="LA"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Loan([Bind("AssetID","UserID","DateExpectedReturn")] LoanedAsset LA)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }

            int added = DAL.LoanAsset(LA);
            if (added != 0 && added != -1)
            {
                return RedirectToAction("Edit", "Asset", DAL.GetAssetByID((int)LA.AssetID));
            }
            ViewBag.NotAdded = true;
            return View(DAL.GetAssetByID((int)LA.AssetID));
        }

        //Return an asset
        /// <summary>
        /// This method is used to return an asset that has been loaned out.
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Return(int? id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }

            DAL.ReturnAsset((int)id);
            return RedirectToAction("Edit", "Asset", DAL.GetAssetByID((int)id));
        }

        // GET: Asset/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (id == null)
            {
                return NotFound();
            }
            else if (CurrentUser.Role.AssetsArchive)
            {
                DAL.ArchiveAssetByID(id);
                DAL.ArchiveModelByID(DAL.GetAssetByID(id).ModelID);
                return RedirectToAction("Index");
            }
            return PartialView("CannotAccessPage", "Shared");
        }

        // GET: All archived Assets
        public async Task<IActionResult> ArchivedAssets()
        {
            if (LoggedIn == false) { return RedirectToAction(actionName: "Index", controllerName: "Login"); }

            if (CurrentUser.Role.AssetsView)
            {
                return View(model: DAL.GetAllArchivedAssets());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        /// <summary>
        /// Restores the Asset by the passed in ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> RestoreAsset(int id)
        {
            if (LoggedIn == false) { return RedirectToAction(actionName: "Index", controllerName: "Login"); }

            if (CurrentUser.Role.AssetsArchive)
            {
                DAL.RestoreAssetByID(id);
                DAL.RestoreModelByID(DAL.GetAssetByID(id).ModelID);
                return RedirectToAction("Index", "Asset");
            }
            else return PartialView("CannotAccessPage", "Shared");
        }
        /// <summary>
        /// This method validates the inputs on the asset controller.
        /// It also makes sure that the inventory number doesn't exist, and that the purchase date selected is
        /// valid. 
        /// </summary>
        /// <param name="InventoryNumber"></param>
        /// <param name="Date"></param>
        /// <returns></returns>
        private bool InputsAreValid(string InventoryNumber, DateTime Date)
        {
            ViewBag.InventoryNumberError = false;
            ViewBag.PurchaseDateError = false;
            List<Asset> Assets = DAL.GetAllAssets();
            bool InventoryNumberExists = Assets.Any(e => e.InventoryNumber == InventoryNumber);
            bool PurchaseDateValid = Date <= DateTime.Today;

            if (InventoryNumberExists == true && PurchaseDateValid == false)
            {
                ViewBag.InventoryNumberError = true;
                ViewBag.PurchaseDateError = true;
                return false;
            }
            else if (PurchaseDateValid == false)
            {
                ViewBag.PurchaseDateError = true;
                return false;
            }
            else if (InventoryNumberExists == true)
            {
                ViewBag.InventoryNumberError = true;
                return false;
            }
            else
            {
                return true;
            }

        }


    }
}
