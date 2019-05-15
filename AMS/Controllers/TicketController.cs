using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MailKit;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using System.Diagnostics;
using AMS.Data;
using AMS.Models;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace AMS.Controllers
{
    /// <summary>
    /// This is the tickets controller. It contains a lot of methods that are TicketRelated!
    /// </summary>
    public class TicketController : BaseController
    {

        // GET: Ticket
        public async Task<IActionResult> Index()
        {
            //If the user isn't logged in, this redirects them to the login page, otherwise, let's show them some tickets!
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.TicketsView)
            {
                return View(DAL.GetAllTickets());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: Ticket/Details/5
        public async Task<IActionResult> Details(int id)
        {
            //If the user isn't logged in, this redirects them to the login page, otherwise, let's show them a ticket based on a TicketID!
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.TicketsView)
            {
                if (id == null || DAL.GetTicketByID(id) == null)
                {
                    return RedirectToAction("Index");
                }
                //Thius displays the list of Notes that are assigned to the ticket. The reverse sets it so the top note is the newest note. 
                List<Note> NoteList = new List<Note>(DAL.GetNotesByTicketID(id));
                NoteList.Reverse();

                ViewBag.Notes = NoteList;
                ViewBag.Assets = new List<Asset>(DAL.GetAssestsByTicketID(id));
                ViewBag.UserName = CurrentUser.FirstName;
                ViewBag.UTR = DAL.GetUserTicketsRolesByTicketID(id);
                return View(DAL.GetTicketByID(id));
            }
            else return PartialView("CannotAccessPage", "Shared");
        }
        // POST: Ticket/Details
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(string Description, int id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.TicketsView)
            {
                ///If the description field isn't blank after grabbing the ticket details, then a new note has been created, and it needs to be added into the database and assigned to that ticket. 
                if (Description != "")
                {
                    DAL.AddNote(Description, CurrentUser.ID, id);
                }
                List<Note> NoteList = new List<Note>(DAL.GetNotesByTicketID(id));
                NoteList.Reverse();
                ViewBag.Notes = NoteList;
                ViewBag.Assets = new List<Asset>(DAL.GetAssestsByTicketID(id));
                ViewBag.UserName = CurrentUser.FirstName;
                ViewBag.UTR = DAL.GetUserTicketsRolesByTicketID(id);
                //Then we need to return the view with the new note added. 
                return View(DAL.GetTicketByID(id));
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: Ticket/Create
        public IActionResult Create()
        {
            //If the user isn't logged in, this redirects them to the login page.
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            //If the current user can create tickets, then:
            if (CurrentUser.Role.TicketsOpen)
            {
                ///Create some new viewbags in order to get the info you need to later post a new created Asset. 
                ViewBag.Assets = new SelectList(DAL.GetAllAssets(), "ID", "InventoryNumber");
                ViewBag.Categories = new SelectList(DAL.GetAllCategories(), "ID", "Name");
                return View();
            }
            //else: Redirect them to the cannot access page partial view. 
            else return PartialView("CannotAccessPage", "Shared");
        }

        // POST: Ticket/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int UserID, [Bind("StatusID,CategoryID,Number,Subject,Description,DateCreated,DateDue,DateResolved,DateLastUpdated,ID,UserID")] Ticket ticket)
        {
            //If the user is logged in and has the permissions to open a ticket then:
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }

            if (CurrentUser.Role.TicketsOpen)
            {
                var emailUser = DAL.GetUserByID(UserID);   
                //Sometimes clearing the model state was needed to have a valid Model. That doesn't seem to be needed anymore. 
                //ModelState.Clear();
                if (ModelState.IsValid)
                {
                    int ID = DAL.AddTicket(ticket, UserID, 3);

                    //A list of assets to be added to the ticket
                    List<string> al = new List<string>(Request.Form["Asset"].ToList());

                    //If there is atleast one Asset selected add its association to the database
                    if (al.Count != 0)
                    {
                        //pass the ticketID and the list of Asset IDs to the DAL
                        DAL.AddAssetToTicket(ticket.ID, al);
                    }
                    ///This next try catch block is to send an email. 
                    ///First it trys to instantiate the needed variables based off of some properties associated to the specific ticket in question. Then it adds the values of those properties onto the email message which is the body
                    ///of the email. 
                    ///Jack adopted the code from this site: https://dotnetcoretutorials.com/2017/11/02/using-mailkit-send-receive-email-asp-net-core/ in order to get this working. 
                    try {
                        string emailBody = string.Empty;
                        ///bool useEmailTemplate = false;
                        //instantiate a new MimeMessage
                        var message = new MimeMessage();
                        message.To.Add(new MailboxAddress(emailUser.FirstName, emailUser.Email));
                        //Setting the From e-mail address
                        message.From.Add(new MailboxAddress("filler", "filler@filler.com"));
                        //E-mail subject 
                        message.Subject = "#" + ticket.ID + " " + ticket.Subject;
                        //E-mail message body
                        emailBody = ticket.Description;
                        message.Body = new TextPart(TextFormat.Html)
                        {
                            Text = emailBody
                        };
                        //Configure the e-mail client. 
                        using (var emailClient = new SmtpClient())
                        { 
                            emailClient.Connect("smtp.gmail.com", 587, false);
                            emailClient.Authenticate("filler@filler.com", "filler");
                            emailClient.Send(message);
                            emailClient.Disconnect(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.Clear();
                        ViewBag.Exception = $" Oops! Message could not be sent. Error:  {ex.Message}";
                    }
                }
                //The line below is helpful in troubleshooting ModelState errors. #lifeprotip.
                //var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
                return RedirectToAction("Details", "Ticket");
            }
            ViewBag.Assets = new SelectList(DAL.GetAllAssets(), "ID", "InventoryNumber");
            ViewBag.Categories = new SelectList(DAL.GetAllCategories(), "ID", "Name");
            return View(ticket);
        }

        // GET: Resolved Ticket
        /// <summary>
        /// This method queries the resolved tickets and displays them in a view. Useful if you want to look up old tickets. You can then use a search through the resolved tickets to find osmething specific. 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ResolvedTickets()
        {
            if (LoggedIn == false) { return RedirectToAction(actionName: "Index", controllerName: "Login"); }

            if (CurrentUser.Role.TicketsView)
            {
                return View(model: DAL.GetAllResolvedTickets());
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // GET: Ticket/Edit/5
        /// <summary>
        /// This method is used get a ticket situated for editing. It displays all the appropriate viewbags into a view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.TicketsEdit)
            {
                if (id == null || DAL.GetTicketByID((int)id) == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Statuses = new SelectList(DAL.GetAllStatuses(), "ID", "Condition");
                    ViewBag.Assets = new SelectList(DAL.GetAllAssets(), "ID", "InventoryNumber");
                    ViewBag.Categories = new SelectList(DAL.GetAllCategories(), "ID", "Name");
                    ViewBag.TicketAssets = new List<Asset>(DAL.GetAssestsByTicketID((int)id));
                    ViewBag.CurrentUserDeleteAsset = CurrentUser.Role.DeleteAsset;
                    ViewBag.UTR = DAL.GetUserTicketsRolesByTicketID((int)id);
                    return View(DAL.GetTicketByID((int)id));
                }
            }
            else return PartialView("CannotAccessPage", "Shared");
        }

        // POST: Ticket/Edit/5
        /// <summary>
        /// This post method submits the edits of a specific ticket. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="statusid"></param>
        /// <param name="AssetsToRemove"></param>
        /// <param name="AssetsToAdd"></param>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int statusid, string AssetsToRemove, string AssetsToAdd, [Bind("StatusID,CategoryID,Number,Subject,Description,DateResolved,DateCreated,DateDue,DateLastUpdated,ID")] Ticket ticket)
        {

            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            //If the current user can edit tickets, and if the ticket model is valid. 
            if (CurrentUser.Role.TicketsEdit)
            {
                if (ModelState.IsValid)
                {
                    //This grabs the old ticket, which is used for if statement on line 253 regarding  if the status ID has changed. 
                    Ticket oldTicket = DAL.GetTicketByID(ticket.ID);

                    ticket.DateLastUpdated = DateTime.Now;
                    DAL.UpdateTicket(ticket);

                    // Logic statement to decide whether to resolve or open the ticket depending on what is passed in.
                    if (ticket.Status.Condition == "Resolved")
                    {
                        DAL.ResolveTicketByID(id);
                    }
                    else if (ticket.Status.Condition != "Resolved")
                    {
                        DAL.RestoreTicketByID(id);
                    }
                    
                    //If the status ID changed, create a new note that specifies what the Status changed to. 
                    if (oldTicket.StatusID != ticket.StatusID)
                    {
                        string description = "Status Change to " + ticket.Status.Condition;
                        DAL.AddNote(description, CurrentUser.ID, ticket.ID);
                    }
                    //If the current user has the permissions to delete an asset, then remove any assets that have been selected for deletion.
                    if (CurrentUser.Role.DeleteAsset) {
                        Asset.RemoveAssetsForTicket(id, AssetsToRemove);
                    }
                    Asset.UpdateAssetsForTicket(id, AssetsToAdd);

                    //A list of assets to be added to the ticket
                    List<string> al = new List<string>(Request.Form["Asset"].ToList());

                    //If there is atleast one Asset selected add its association to the database
                    if (al.Count != 0)
                    {
                        //pass the ticketID and the list of Asset IDs to the DAL
                        DAL.AddAssetToTicket(ticket.ID, al);
                    }

                    return RedirectToAction("Index", "Ticket");
                }
                ViewBag.Statuses = new SelectList(DAL.GetAllStatuses(), "ID", "Condition");
                ViewBag.Assets = new SelectList(DAL.GetAllAssets(), "ID", "InventoryNumber");
                ViewBag.Categories = new SelectList(DAL.GetAllCategories(), "ID", "Name");
                ViewBag.TicketAssets = new List<Asset>(DAL.GetAssestsByTicketID(id));
                ViewBag.UTR = DAL.GetUserTicketsRolesByTicketID(id);
                return View(DAL.GetTicketByID((int)id));
            }
            else return PartialView("CannotAccessPage", "Shared");
        }


        // GET: Ticket/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (LoggedIn == false) { return RedirectToAction("Index", "Login"); }
            if (CurrentUser.Role.TicketsResolve)
            {
                if (id == null || DAL.GetTicketByID(id) == null)
                {
                    return RedirectToAction("Index");
                }
                DAL.ResolveTicketByID(id);
                return RedirectToAction("Index");
            }
            else return PartialView("CannotAccessPage", "Shared");
        }
    }
}
