using System;
using System.ComponentModel.DataAnnotations;
using AMS.Data;

/// <summary>
/// Created by Paul Michael on 4/26/19
/// </summary>
namespace AMS.Models
{
    public class LoanedAsset : DatabaseObject
    {
        private int _UserID;
        private User _User;
        private int _AssetID;
        private Asset _Asset;
        private DateTime _DateExpectedReturn;

        public LoanedAsset()
        {

        }
        public LoanedAsset(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

        /// <summary>
        /// Foreign key to identify the asset that is loaned
        /// </summary>
        [Required]
        public int AssetID
        {
            get { return _AssetID; }
            set { _AssetID = value; }
        }

        /// <summary>
        /// Gets and sets the Asset Object
        /// </summary>
        public Asset Asset
        {
            get
            {
                if (_Asset == null)
                {
                    _Asset = DAL.GetAssetByID(_AssetID);
                }
                return _Asset;
            }
            set
            {
                _Asset = value;
                if (value == null)
                {
                    _AssetID = -1;
                }
                else
                {
                    _AssetID = value.ID;
                }
            }
        }


        /// <summary>
        /// Foreign key to identify the User that the asset is loaned to
        /// </summary>
        [Required]
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        /// <summary>
        /// Gets and sets the User Object
        /// </summary>
        public User User
        {
            get
            {
                if (_User == null)
                {
                    _User = DAL.GetUserByID(_UserID);
                }
                return _User;
            }
            set
            {
                _User = value;
                if (value == null)
                {
                    _UserID = -1;
                }
                else
                {
                    _UserID = value.ID;
                }
            }
        }

        /// <summary>
        /// The date the asset is expected to be returned
        /// </summary>
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Expected Return")]
        public DateTime DateExpectedReturn
        {
            get { return _DateExpectedReturn; }
            set
            {
                _DateExpectedReturn = value;
            }
        }

        /// <summary>
        /// Fills the user and asset onjects for loaned assets
        /// </summary>
        /// <param name="dr">DataReader</param>
        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            User = new User(dr);
            Asset = new Asset(dr);
            AssetID = dr.GetInt32("AssetID");
            UserID = dr.GetInt32("UserID");
            DateExpectedReturn = dr.GetDateTime("DateExpectedReturn");
        }

    }
}