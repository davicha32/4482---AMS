using System;
using System.ComponentModel.DataAnnotations;
using AMS.Data;
using AMS.Models;
///
/// Coded by Jack Bradley 2/13/19
///
/// <summary>
/// This is used to distinguish the brand name of devices. 
/// </summary>
namespace AMS.Models
{
    public class Device : DatabaseNamedObject
	{
        private int _BrandID;
        private Brand _Brand;
        private string _Name;
        private DateTime _DateArchived;

        public Device()
        {

        }
        /// <summary>
        /// Constructor utilizing a fill method to insert into the database.
        /// </summary>
        /// <param name="dr"></param>
        public Device(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

        /// <summary>
        /// Gets and sets the BrandID
        /// </summary>
        [Display(Name = "Brand")]
        public int BrandID
        {
            get { return _BrandID; }
            set { _BrandID = value; }
        }

        /// <summary>
        /// <summary>
        /// Gets and sets the Device Object
        /// </summary>
        public Brand Brand
        {
            get
            {
                if (_Brand == null)
                {
                    _Brand = DAL.GetBrandByID(_BrandID);
                }
                return _Brand;
            }
            set
            {
                _Brand = value;
                if (value == null)
                {
                    _BrandID = -1;
                }
                else
                {
                    _BrandID = value.ID;
                }
            }
        }

        /// <summary>
        /// The date the device was archived
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
        /// Fills the object out with data from the datareader.
        /// </summary>
        /// <param name="dr"></param>
        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            ID = dr.GetInt32("DeviceID");
            Name = dr.GetString("Name");
            DateArchived = dr.GetDateTime("DateArchived");
            BrandID = dr.GetInt32("BrandID");
        }
    }
}
