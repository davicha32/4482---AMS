///
/// Coded by Jack Bradley 2/13/19
///

/// This is a model for Tickets. They have various properties for each ticket. Some of the email fields have been commented out and are no longer being utilized. This was so we could assign a specific user to a ticket that has an email address.
using System;
using System.ComponentModel.DataAnnotations;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Pop3;
using System.Collections.Generic;
using System.Linq;
using AMS.Data;

namespace AMS.Models
{
	public class Ticket : DatabaseObject
	{
		private int _Number;
		private string _Description;
		private DateTime _DateCreated;
		private DateTime _DateLastUpdated;
        private DateTime _DateDue;
        private DateTime _DateResolved;
        private int _StatusID;
        //private string _Subject;
        private Status _Status;
		private int _CategoryID;
        private Category _Category;
        //private Email _Email;
        //private string _Message;
        private int _UserID;
        //private string _Name;
        //[Required]
        //[StringLength(60, MinimumLength = 5)]
        //public string Name { get; set; }

        [Required]
        public string Subject { get; set; }
        
        public string Message { get; set; }
        public string Email { get; set; }
        
        public Ticket()
        {

        }

        public Ticket(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

        /// <summary>
        /// Foreign ID for status Table
        /// </summary>
        [Required]
        [Display(Name = "Status")]
        public int StatusID
		{
			get { return _StatusID; }
			set { _StatusID = value; }
		}

        /// <summary>
        /// Foreign key for category Table
        /// </summary>
        [Required]
        [Display(Name = "Category")]
        public int CategoryID
		{
			get { return _CategoryID; }
			set { _CategoryID = value; }
		}

//        [Required]
        [Display(Name = "Category")]
        /// <summary>
        /// Gets and sets the Category Object
        /// </summary>
        public Category Category
        {
            get
            {
                if (_Category == null)
                {
                    _Category = DAL.GetCategoryByID(_CategoryID);
                }
                return _Category;
            }
            set
            {
                _Category = value;
                if (value == null)
                {
                    _CategoryID = -1;
                }
                else
                {
                    _CategoryID = value.ID;
                }
            }
        }

        [Display(Name = "Status")]
        /// <summary>
        /// Gets and sets the Status Object
        /// </summary>
        public Status Status
        {
            get
            {
                if(_Status == null)
                {
                    _Status = DAL.GetStatusByID(_StatusID);
                }
                return _Status;
            }
            set
            {
                _Status = value;
                if(value == null)
                {
                    _StatusID = -1;
                }
                else
                {
                    _StatusID = value.ID;
                }
            }
        }

        /// <summary>
        /// The TicketNumber
        /// </summary>
//        [Required]
        [Display(Name = "Ticket Number")]
		public int Number
		{
			get { return _Number; }
			set { _Number = value; }
		}

        /// <summary>
        /// Short Description for the ticket
        /// </summary>
        [Required]
        public string Description
		{
			get { return _Description; }
			set { _Description = value; }
		}
   
        /// <summary>
        /// Date the ticket was created
        /// </summary>
//        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date Created")]
        public DateTime DateCreated
		{
			get { return _DateCreated; }
			set { _DateCreated = value; }
		}

        /// <summary>
        /// Date the ticket is due to be complete
        /// set to 9999-12-31 23:59:59 by default in database
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date Due")]
        public DateTime DateDue
		{
			get { return _DateDue; }
			set { _DateDue = value; }
		}

        /// <summary>
        /// Date the ticket was resolved
        /// set to 9999-12-31 23:59:59 by defaul
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date Resolved")]
        public DateTime DateResolved
        {
            get { return _DateResolved; }
            set { _DateResolved = value; }
        }

        /// <summary>
        /// Date the ticket was last updated
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date Last Updated")]
        public DateTime DateLastUpdated
		{
			get { return _DateLastUpdated; }
			set { _DateLastUpdated = value; }
		}

        [Required]
        [Display(Name = "Requestor")]
        public int UserID {
            get { return _UserID; }
            set { _UserID = value; }
        }

        /// <summary>
        /// Fills the object using the datareader
        /// </summary>
        /// <param name="dr"></param>
        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            ID = dr.GetInt32("TicketID");
            Number = dr.GetInt32("Number");
            Subject = dr.GetString("Subject");
            Description = dr.GetString("Description");
            DateCreated = dr.GetDateTime("DateCreated");
            DateLastUpdated = dr.GetDateTime("DateLastUpdated");
            DateDue = dr.GetDateTime("DateDue");
            DateResolved = dr.GetDateTime("DateResolved");
            StatusID = dr.GetInt32("StatusID");
            CategoryID = dr.GetInt32("CategoryID");

        }

    }
}

