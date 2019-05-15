



using System;
using System.ComponentModel.DataAnnotations;
///
/// Coded by Aaron Harvey
///
/// <summary>
/// This model is used to track the state/condition of an asset. An example could be "broken".
/// </summary>
namespace AMS.Models
{
	public class State : DatabaseObject
	{
        private DateTime _DateArchived;
        private string _Name;

        public State()
        {

        }

        /// <summary>
        /// Name property for all models that inherit from this model
        /// </summary>
        [Required]
        [Display(Name = "State")]
        public string Name
        {
            get { return _Name; }
            set
            {
                if (value == null)
                {
                    _Name = "";
                }
                else
                {
                    _Name = value;
                }
            }
        }

        /// <summary>
        /// The date the state was archived
        /// set to 9999-12-31 23:59:59 by default
        /// </summary>
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date Archived")]
        public DateTime DateArchived
        {
            get { return _DateArchived; }
            set { _DateArchived = value; }
        }

        public State(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

        /// <summary>
        /// Fills the object using the datareader
        /// </summary>
        /// <param name="dr"></param>
        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            ID = dr.GetInt32("StateID");
            Name = dr.GetString("Name");
            DateArchived = dr.GetDateTime("DateArchived");
        }
    }
}