


using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AMS.Data;

///
/// Coded by Aaron Harvey 2/13/19
///
/// <summary>
/// This model is to represent the User that will be utilizing the system. It contains all the required properties.
/// </summary>
namespace AMS.Models
{

	public class User : DatabaseObject
	{

		private string _Email;
		private string _Password;
		private string _FirstName;
		private string _LastName;
		private int _RoleID;
		private string _Salt;
        private DateTime _DateDisabled;
        private Role _Role;
        private User _CurrentUser;

        public User()
        {

        }
        public User(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

        /// <summary>
        /// A getter and setter for an Email. This is important, because it ties an email address eto a user. 
        /// </summary>
        [Required]
        public string Email
		{
			get { return _Email; }
			set { _Email = value; }
		}
        /// <summary>
        /// The users's password. Don't worry, we hash it!
        /// </summary>
        [Required]
		public string Password
		{
			get { return _Password; }
			set { _Password = value; }
		}
        /// <summary>
        /// The getter and setter for the user's first name.
        /// </summary>
        [Required]
        [Display(Name = "First Name")]
        public string FirstName
		{
			get { return _FirstName; }
			set
			{
				if (_FirstName == null)
				{
					_FirstName = value;
				}
			}
		}
        /// <summary>
        /// And their last name!
        /// </summary>
        [Required]
        [Display(Name = "Last Name")]
        public string LastName
		{
			get { return _LastName; }
			set
			{
                if (_LastName == null)
                {
                    _LastName = value;
                }
			}
		}

        //Foreign key joining the Role table. 
        [Display(Name = "Role")]
        [Required]
        public int RoleID
		{
			get { return _RoleID; }
			set { _RoleID = value; }
		}

        /// <summary>
        /// Gets and sets the Role Object
        /// </summary>
//        [Required]
        public Role Role
        {
            get
            {
                if (_Role == null)
                {
                    _Role = DAL.GetRoleByID(_RoleID);
                }
                return _Role;
            }
            set
            {
                _Role = value;
                if (value == null)
                {
                    _Role = null;
                }
                else
                {
                    _Role = value;
                }
            }
        }

        /// <summary>
        /// This holds a salt for the user in the database
        /// </summary>
        public string Salt
		{
			get
			{
				return _Salt;
			}
			set
			{
				_Salt = value.Trim();
			}
		}

        /// <summary>
        /// Date the user was deleted
        /// set to 9999-12-31 23:59:59 by default
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date Disabled")]
        public DateTime DateDisabled
        {
            get { return _DateDisabled; }
            set { _DateDisabled = value; }
        }

        /// <summary>
        /// Fills the object using the datareader
        /// </summary>
        /// <param name="dr">Data Reader to Retrieve the User information</param>
        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            //This fills an arry with the column names so we can check for the password column in this fill()
            //Found Here: https://stackoverflow.com/questions/373230/check-for-column-name-in-a-sqldatareader-object
            var fieldNames = Enumerable.Range(0, dr.FieldCount).Select(i => dr.GetName(i)).ToArray();

            ID = dr.GetInt32("UserID");
            Email = dr.GetString("Email");
            FirstName = dr.GetString("FirstName");
            LastName = dr.GetString("LastName");
            RoleID = dr.GetInt32("RoleID");
            DateDisabled = dr.GetDateTime("DateDisabled");
            if (fieldNames.Contains("Password"))
            {
                Password = dr.GetString("Password");
                Salt = dr.GetString("Salt");
            }
        }
    }
}