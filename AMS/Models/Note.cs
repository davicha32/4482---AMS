using System;
using AMS.Data;

namespace AMS.Models
{

	/// <summary>
	/// Created by: Julia Parsley 2/13/2019
	/// Note: used to track the individual notes on a ticket
	/// </summary>
	public class Note : DatabaseObject
	{
		private string _Description;
        private User _User;
        private DateTime _DateCreated;

        public Note()
        {

        }
        public Note(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

        /// <summary>
        /// A string to hold the content of the note. If the description field is null, then 
        /// the note's string should be set to be blank. 
        /// </summary>
        public string Description
		{
			get { return _Description; }
			set
			{
				if (value == null)
				{
					_Description = "";
				}
				else
				{
					_Description = value;
				}
			}
		}

        /// <summary>
        /// Gets and set a user for the note
        /// </summary>
        public User User
        {
            get {
                if (_User == null)
                {
                    _User = DAL.GetUserByNoteID(ID);
                }
                return _User;
            }
            set
            {
                    _User = DAL.GetUserByNoteID(ID);
            }
        }
        /// <summary>
        /// Gets and sets a DateTime for when the note was created. 
        /// </summary>
        public DateTime DateCreated
        {
            get { return _DateCreated; }
            set
            {
                    _DateCreated = value;
                
            }
        }

        /// <summary>
        /// Fills the object from the datareader
        /// </summary>
        /// <param name="dr"></param>
        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            ID = dr.GetInt32("NoteID");
            Description = dr.GetString("Description");
            DateCreated = dr.GetDateTime("DateCreated");
        }
    }

}