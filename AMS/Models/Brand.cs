using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models
    /// This is a simple model to track Brands of assets. 
{   
    public class Brand : DatabaseNamedObject
    {
        private DateTime _DateArchived;
        public Brand()
        {

        }

        /// <summary>
        /// The date the brand was archived
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

        /// <summary>
        /// Constructor:
        /// </summary>
        /// <param name="dr"></param>
        public Brand(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

        /// <summary>
        /// This is a fill method that adds a new Brand into the database once it is called within a constructor. 
        /// </summary>
        /// <param name="dr"></param>
        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            ID = dr.GetInt32("BrandID");
            Name = dr.GetString("Name");
            DateArchived = dr.GetDateTime("DateArchived");
        }

    }
}