using AMS.Models;
using System;
using System.ComponentModel.DataAnnotations;
using AMS.Data;

///
/// Coded by Jack Bradley 2/13/19
///
/// <summary>
/// This is a model used to track the Model of an asset. 
/// </summary>
namespace AMS.Models
{
	public class Model : DatabaseNamedObject
	{
        private int _DeviceID;
        private Device _Device;
        private DateTime _DateArchived;

        public Model()
        {

        }

        /// <summary>
        /// Constructor that calls the fill method and inserts a new model into the database. 
        /// </summary>
        /// <param name="dr"></param>
        public Model(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

        /// <summary>
        /// Gets and sets the DeviceID
        /// </summary>
        [Display(Name = "Device Type")]
        public int DeviceID
        {
            get { return _DeviceID; }
            set { _DeviceID = value; }
        }

        /// <summary>
        /// Gets and sets the Device Object
        /// </summary>
        public Device Device
        {
            get
            {
                if (_Device == null)
                {
                    _Device = DAL.GetDeviceByID(_DeviceID);
                }
                return _Device;
            }
            set
            {
                _Device = value;
                if (value == null)
                {
                    _DeviceID = -1;
                }
                else
                {
                    _DeviceID = value.ID;
                }
            }
        }

        /// <summary>
        /// The date the model was archived
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
        /// Fills the object from the datareader
        /// </summary>
        /// <param name="dr"></param>
        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            ID = dr.GetInt32("ModelID");
            Name = dr.GetString("Name");
            DateArchived = dr.GetDateTime("DateArchived");
            DeviceID = dr.GetInt32("DeviceID");
        }
    }
}

