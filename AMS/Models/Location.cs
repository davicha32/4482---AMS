
//Created by Tyson Baker
//2/12/2019

using System;
using System.ComponentModel.DataAnnotations;
using AMS.Data;

/// <summary>
/// Places where devices can be stored, or where user have some sort of residence with a specific area
/// </summary>
namespace AMS.Models
{

	public class Location : DatabaseNamedObject
	{
		private int _StateID;
		private int _ParentLocationID;
        private State _State;
        private DateTime _DateArchived;

        //This is not currently in the database - Paul
        //private int _TemporaryParentLocationID;

        public Location()
        {

        }
        public Location(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

        /// <summary>
        /// Gets and sets the StateID
        /// </summary>
        [Required]
		public int StateID
		{
			get { return _StateID; }
			set { _StateID = value; }
		}

        /// <summary>
        /// Gets and sets the ParentLocationID
        /// </summary>
		public int ParentLocationID
		{
			get { return _ParentLocationID; }
			set { _ParentLocationID = value; }
		}

        /// <summary>
        /// <summary>
        /// Gets and sets the state Object
        /// </summary>
//        [Required]
        public State State
        {
            get
            {
                if (_State == null)
                {
                    _State = DAL.GetStateByID(_StateID);
                }
                return _State;
            }
            set
            {
                _State = value;
                if (value == null)
                {
                    _StateID = -1;
                }
                else
                {
                    _StateID = value.ID;
                }
            }
        }

        /// <summary>
        /// The date the location was archived
        /// set to 9999-12-31 23:59:59 by default
        /// </summary>
//        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date Archived")]
        public DateTime DateArchived
        {
            get { return _DateArchived; }
            set { _DateArchived = value; }
        }

        //This is not currently in the database - Paul
        //public int TemporaryParentLocationID
        //{
        //	get { return _TemporaryParentLocationID; }
        //	set { _TemporaryParentLocationID = value; }
        //}

        /// <summary>
        /// Fills the object out with data from the datareader.
        /// </summary>
        /// <param name="dr"></param>
        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            ID = dr.GetInt32("LocationID");
            Name = dr.GetString("Name");
            StateID = dr.GetInt32("StateID");
            ParentLocationID = dr.GetInt32("ParentLocationID");
            DateArchived = dr.GetDateTime("DateArchived");
        }
    }
}