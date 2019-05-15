using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AMS.Data;

namespace AMS.Models
{

    /// <summary>
    /// Created By: Paul Michael
    /// Created on: February 12, 2019
    /// Updated on:
    /// </summary>
    public class Asset : DatabaseObject
    {
        private string _InventoryNumber;
        private DateTime _DatePurchased;
        private DateTime _DateWarrantyExpires;
        private DateTime _DateArchived;
        private bool _IsLoanable;
        private bool _IsLoaned;
        private int _ModelID;
        private Model _Model;
        private int _StateID;
        private State _State;
        private int _LocationID;
        private Location _Location;

        public Asset()
        {

        }
        internal Asset(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

        /// <summary>
        /// Unique number to identify the asset in the inventory
        /// </summary>
        [Required]
        [Display(Name = "Inventory Number")]
        public string InventoryNumber
        {
            get { return _InventoryNumber; }
            set
            {
                if (value == "0")
                {
                    _InventoryNumber = "-1";
                }
                else
                {
                    _InventoryNumber = value;
                }
            }
        }

        /// <summary>
        /// The date the asset was purchased
        /// </summary>
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date Purchased")]
        public DateTime DatePurchased
        {
            get { return _DatePurchased; }
            set
            {
                _DatePurchased = value;
            }
        }

        /// <summary>
        /// The date the warrnty of the asset will expire
        /// set to 9999-12-31 23:59:59 by default
        /// </summary>
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Warranty Expiration Date")]
        public DateTime DateWarrantyExpires
        {
            get { return _DateWarrantyExpires; }
            set { _DateWarrantyExpires = value; }
        }
        /// <summary>
        /// The date the asset was archived
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
        /// Boolean if the asset is able to be loaned out
        /// </summary>
        [Display(Name = "Is Loanable")]
        public bool IsLoanable
        {
            get { return _IsLoanable; }
            set { _IsLoanable = value; }
        }

        /// <summary>
        /// Boolean if the asset is loaned out
        /// </summary>
        [Display(Name = "Is Loaned")]
        public bool IsLoaned
        {
            get { return _IsLoaned; }
            set
            {
                if (_IsLoanable == true)
                {
                    _IsLoaned = value;
                }
                else
                {
                    _IsLoaned = false;
                }
            }
        }

        /// <summary>
        /// Foreign key to identify the Model the asset is
        /// </summary>
        [Required]
        [Display(Name = "Model Type")]
        public int ModelID
        {
            get { return _ModelID; }
            set { _ModelID = value; }
        }

        /// <summary>
        /// Gets and sets the Model Object
        /// </summary>
        public Model Model
        {
            get
            {
                if (_Model == null)
                {
                    _Model = DAL.GetModelByID(_ModelID);
                }
                return _Model;
            }
            set
            {
                _Model = value;
                if (value == null)
                {
                    _ModelID = -1;
                }
                else
                {
                    _ModelID = value.ID;
                }
            }
        }

        /// <summary>
        /// Foreign key to identify the state/condiction the asset is in
        /// </summary>
        [Required]
        [Display(Name = "State")]
        public int StateID
        {
            get { return _StateID; }
            set { _StateID = value; }
        }

        /// <summary>
        /// Gets and sets the State Object
        /// </summary>
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
        /// Foreign key to identify the location that the asset is in.
        /// </summary>
        [Required]
        [Display(Name = "Location")]
        public int LocationID
        {
            get { return _LocationID; }
            set { _LocationID = value; }
        }

        /// <summary>
        /// Gets and sets the Location Object
        /// </summary>
        public Location Location
        {
            get
            {
                if (_Location == null)
                {
                    _Location = DAL.GetLocationByID(_LocationID);
                }
                return _Location;
            }
            set
            {
                _Location = value;
                if (value == null)
                {
                    _LocationID = -1;
                }
                else
                {
                    _LocationID = value.ID;
                }
            }
        }
        /// <summary>
        /// This Method fills out an Asset's various properties to be inserted as a single asset into the database.
        /// </summary>
        /// <param name="dr"></param>

        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            ID = dr.GetInt32("AssetID");
            InventoryNumber = dr.GetString("InventoryNumber");
            DatePurchased = dr.GetDateTime("DatePurchased");
            DateWarrantyExpires = dr.GetDateTime("DateWarrantyExpires");
            DateArchived = dr.GetDateTime("DateArchived");
            IsLoanable = dr.GetBoolean("IsLoanable");
            IsLoaned = dr.GetBoolean("IsLoaned");
            ModelID = dr.GetInt32("ModelID");
            StateID = dr.GetInt32("StateID");
            LocationID = dr.GetInt32("LocationID");
        }

        /// <summary>
        /// Updates the assets associated with a ticket
        /// </summary>
        /// <param name="TicketID">ID of ticket to be updated</param>
        /// <param name="Add">List of AssetIDs to add</param>
        /// <param name="Remove">List of AssetIDs to remove</param>
        public static void UpdateAssetsForTicket(int TicketID, string Add)
        {
            if (Add != null)
            {

            }
        }
        
        /// <summary>
        /// By Jack: This method is used to remove assets from a ticket. A role permission check happens before this method is called. 
        /// </summary>
        /// <param name="TicketID"></param>
        /// <param name="Remove"></param>
        public static void RemoveAssetsForTicket(int TicketID,string Remove)
        {

            if (Remove != null)
            {
                List<string> AssetIDs = new List<string>(Remove.Split(","));
                DAL.RemoveAssetFromTicket(TicketID, AssetIDs);
            }
        }

    }
}