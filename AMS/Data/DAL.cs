//Created by Tyson Baker
//Last Modified 2/16/2019

//MySQL resource used: https://dev.mysql.com/doc/connector-net/en/connector-net-programming-stored-using.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data.SqlClient;
using AMS.Models;
using MySql.Data.MySqlClient;

/// <summary>
/// This will be our DAL that we will use for the project.
/// </summary>
namespace AMS.Data
{
    public static class DAL
    {
        //Localhost server strings
        //private static string _EditConnectionString = "server=localhost;port=3306;username=AMSEditUser;password=yViEpZpg7i&11*f&O;database=db_a48a53_ams;";
        //private static string _ReadConnectionString = "server=localhost;port=3306;username=AMSReadUser;password=&3kn90kR5w&PUNlZR;database=db_a48a53_ams;";

        //SmarterASP.net server MySQL strings
        private static string _EditConnectionString = "Server=MYSQL7001.site4now.net;Database=db_a48a53_ams;Uid=a48a53_ams;Pwd=4482_ams";
        private static string _ReadConnectionString = "Server=MYSQL7001.site4now.net;Database=db_a48a53_ams;Uid=a48a53_ams;Pwd=4482_ams";


        public static string _Pepper = "fkE4kDMaEdke48iLAq39ca"; //HACK: set here for now, will move elsewhere later.
        public static int _Stretches = 10000;

        #region Asset
        /// <summary>
        /// Gets all of the assets from the database.
        /// </summary>
        internal static List<Asset> GetAllAssets()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetAssets", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                List<Asset> al = new List<Asset>();
                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new Asset(dr).DateArchived == maxDate)
                    {
                        al.Add(new Asset(dr));
                    }
                }

                return al;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        /// <summary>
        /// Gets all of the assets from the database that match parameter in inventory number, device, brand, or model name.
        /// </summary>
        internal static List<Asset> GetAssetsLike(string search)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetAssetsLike", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_search", search);

                MySqlDataReader dr = cmd.ExecuteReader();

                List<Asset> al = new List<Asset>();
                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new Asset(dr).DateArchived == maxDate)
                    {
                        al.Add(new Asset(dr));
                    }
                }

                return al;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        /// <summary>
        /// Adds an asset to the database.
        /// </summary>
        /// <param name="a">The asset provided.</param>
        /// <returns></returns>
        internal static int AddAsset(Asset a)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_AddAsset", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_InventoryNumber", a.InventoryNumber);
                cmd.Parameters.AddWithValue("@i_DatePurchased", a.DatePurchased);
                cmd.Parameters.AddWithValue("@i_DateWarrantyExpires", a.DateWarrantyExpires);
                cmd.Parameters.AddWithValue("@i_IsLoanable", a.IsLoanable);
                cmd.Parameters.AddWithValue("@i_ModelID", a.ModelID);
                cmd.Parameters.AddWithValue("@i_StateID", a.StateID);
                cmd.Parameters.AddWithValue("@i_LocationID", a.LocationID);

                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Update an Asset in the database
        /// </summary>
        /// <param name="a">Asset</param>
        /// <returns>-1 if not updated</returns>
        internal static int UpdateAsset(Asset a)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordUpdated = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_UpdateAssetByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_AssetID", a.ID);
                cmd.Parameters.AddWithValue("@i_InventoryNumber", a.InventoryNumber);
                cmd.Parameters.AddWithValue("@i_DatePurchased", a.DatePurchased);
                cmd.Parameters.AddWithValue("@i_DateWarrantyExpires", a.DateWarrantyExpires);
                cmd.Parameters.AddWithValue("@i_IsLoanable", a.IsLoanable);
                cmd.Parameters.AddWithValue("@i_ModelID", a.ModelID);
                cmd.Parameters.AddWithValue("@i_StateID", a.StateID);
                cmd.Parameters.AddWithValue("@i_LocationID", a.LocationID);

                recordUpdated = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordUpdated;
        }

        /// <summary>
        /// Get an asset from the database by ID
        /// </summary>
        /// <param name="id">AssetID</param>
        /// <returns>An Asset</returns>
        public static Asset GetAssetByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            Asset retObj = null;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetAssetByID", conn);
                cmd.Parameters.AddWithValue("@i_AssetID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    retObj = new Asset(dr);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return retObj;
        }

        /// <summary>
        /// Retrieve all assets accociated with a ticket
        /// </summary>
        /// <param name="id">Ticket ID</param>
        /// <returns>List of Asset</returns>
        public static List<Asset> GetAssestsByTicketID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            List<Asset> retList = new List<Asset>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetAssetsByTicketID", conn);
                cmd.Parameters.AddWithValue("@i_TicketID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    retList.Add(new Asset(dr));
                }
                conn.Close();
                return retList;
            }
            catch (Exception ex)
            {
                conn.Close();
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Adds assets to a ticket
        /// </summary>
        /// <param name="TicketID">TicketID to be added to</param>
        /// <param name="AssetIDList">List<string> of AssetIDs to be added to a ticket</param>
        internal static void AddAssetToTicket(int TicketID, List<string> AssetIDList)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;
            try
            {
                conn.Open();
                foreach (string s in AssetIDList)
                {
                    int AssetID = Convert.ToInt32(s);
                    MySqlCommand cmd = new MySqlCommand("sproc_AddAssetToTicket", conn);
                    cmd.Parameters.AddWithValue("@i_AssetID", AssetID);
                    cmd.Parameters.AddWithValue("@i_TicketID", TicketID);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Removes an asset from a ticket
        /// </summary>
        /// <param name="TicketID">TicketID to be removed from</param>
        /// <param name="AssetIDList">List<string> of AssetIDs to be removed from a ticket</param>
        internal static void RemoveAssetFromTicket(int TicketID, List<string> AssetIDList)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;
            try
            {
                conn.Open();
                foreach (string s in AssetIDList)
                {
                    int AssetID = Convert.ToInt32(s);
                    MySqlCommand cmd = new MySqlCommand("sproc_RemoveAssetFromTicket", conn);
                    cmd.Parameters.AddWithValue("@i_AssetID", AssetID);
                    cmd.Parameters.AddWithValue("@i_TicketID", TicketID);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Archove an asset by its ID
        /// </summary>
        /// <param name="id">AssetID to be archived</param>
        /// <returns>-1 if asset is not added</returns>
        public static int ArchiveAssetByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_ArchiveAssetByID", conn);
                cmd.Parameters.AddWithValue("i_AssetID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Restores the archived Asset by the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int RestoreAssetByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_RestoreAssetByID", conn);
                cmd.Parameters.AddWithValue("i_AssetID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Get all of the archived Assets from the database.
        /// </summary>
        internal static List<Asset> GetAllArchivedAssets()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetAssets", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                List<Asset> al = new List<Asset>();
                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new Asset(dr).DateArchived != maxDate)
                    {
                        al.Add(new Asset(dr));
                    }
                }

                return al;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        /// <summary>
        /// Loans an asset out
        /// </summary>
        /// <param name="UserID">UserID</param>
        /// <param name="AssetID">AssetID</param>
        /// <param name="DateExpectedReturn">DateExpectedReturn</param>
        /// <returns></returns>
        internal static int LoanAsset(LoanedAsset LA)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordUpdated = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_AddLoanedAsset", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_UserID", LA.UserID);
                cmd.Parameters.AddWithValue("@i_AssetID", LA.AssetID);
                cmd.Parameters.AddWithValue("@i_DateExpectedReturn", LA.DateExpectedReturn);

                recordUpdated = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordUpdated;
        }
        
        /// <summary>
        /// Returns a loaned asset
        /// </summary>
        /// <param name="AssetID">AssetID</param>
        /// <returns>-1 if not returend</returns>
        internal static int ReturnAsset(int AssetID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordUpdated = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_DeleteLoanedAssetByAssetID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_AssetID", AssetID);

                cmd.ExecuteNonQuery();
                recordUpdated = AssetID;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordUpdated;
        }

        /// <summary>
        /// Find Loaned Asset by Asset
        /// </summary>
        /// <param name="AssetID">AssetID</param>
        /// <returns>-1 if not returend</returns>
        internal static LoanedAsset GetLoanedAssetAndUserByAssetID(int AssetID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;

            LoanedAsset LA = null;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetLoanedAssetAndUserByAssetID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_AssetID", AssetID);

                MySqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount > 1)
                {
                    while (dr.Read())
                    {
                        LA = new LoanedAsset(dr);
                    }
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return LA;
        }
        /// <summary>
        /// Find Loaned Asset By User ID
        /// </summary>
        /// <param name="UserID">UserID</param>
        /// <returns>-1 if not returend</returns>
        internal static List<LoanedAsset> GetLoanedAssetAndUserByUserID(int UserID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;

            List<LoanedAsset> LLA = new List<LoanedAsset>();

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetLoanedAssetAndUserByUserID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_UserID", UserID);

                MySqlDataReader dr = cmd.ExecuteReader();

                if (dr.FieldCount > 1)
                {
                    while (dr.Read())
                    {
                        LLA.Add(new LoanedAsset(dr));
                    }
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return LLA;
        }

        #endregion

        #region Brand
        /// <summary>
        /// Gets all of the brands from the database.
        /// </summary>
        internal static List<Brand> GetAllBrands()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            List<Brand> bl = new List<Brand>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetBrands", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();
                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new Brand(dr).DateArchived == maxDate)
                    {
                        bl.Add(new Brand(dr));
                    }
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return bl;
        }

        /// <summary>
        /// Adds a brand to the database.
        /// </summary>
        /// <param name="b">The brand provided.</param>
        /// <returns></returns>

        internal static int AddBrand(Brand b)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_AddBrand", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@i_Name", b.Name);

                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Takes in a BrandID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>A Brand object</returns>
        internal static Brand GetBrandByID(int ID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            Brand Brand = new Brand();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetBrandByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_BrandID", ID);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Brand = new Brand(dr);
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return Brand;
        }

        /// <summary>
        /// Getsa brand from the database by brandname
        /// </summary>
        /// <param name="Name">Brand Name</param>
        /// <returns>a BrandID</returns>
        internal static int GetBrandByName(string Name)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            Brand Brand = new Brand();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetBrandByName", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@i_Name", Name);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Brand = new Brand(dr);
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return Brand.ID;
        }

        /// <summary>
        /// Update an Brand in the database
        /// </summary>
        /// <param name="a">Brand</param>
        /// <returns>-1 if not updated</returns>
        internal static int UpdateBrand(Brand b)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;
            int recordUpdated = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_UpdateBrandByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);

                cmd.Parameters.AddWithValue("@i_BrandID", b.ID);
                cmd.Parameters.AddWithValue("@i_Name", b.Name);
                cmd.Parameters.AddWithValue("@i_DateArchived", maxDate);

                recordUpdated = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordUpdated;
        }

        /// <summary>
        /// Archive a Brand
        /// </summary>
        /// <param name="id">BrandID</param>
        /// <returns>-1 if not Archived</returns>
        public static int ArchiveBrandByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordArchived = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_ArchiveBrandByID", conn);
                cmd.Parameters.AddWithValue("i_BrandID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                recordArchived = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordArchived;
        }

        /// <summary>
        /// Restores the disabled Brand by the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int RestoreBrandByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_RestoreBrandByID", conn);
                cmd.Parameters.AddWithValue("i_BrandID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Get all of the archived Brands from the database.
        /// </summary>
        internal static List<Brand> GetAllArchivedBrands()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetBrands", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                List<Brand> bl = new List<Brand>();
                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new Brand(dr).DateArchived != maxDate)
                    {
                        bl.Add(new Brand(dr));
                    }
                }

                return bl;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }
        #endregion

        #region Category

        /// <summary>
        /// Get a list of all Category from the database
        /// </summary>
        internal static List<Category> GetAllCategories()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            List<Category> cl = new List<Category>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetCategories", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    cl.Add(new Category(dr));
                }


            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return cl;
        }

        /// <summary>
        /// Takes in a CategoryID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>A Category object</returns>
        internal static Category GetCategoryByID(int ID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            Category Category = null;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetCategoryByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_CategoryID", ID);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Category = new Category(dr);
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return Category;
        }
        #endregion

        #region Device

        /// <summary>
        /// Get a list of all Device from the database
        /// </summary>
        internal static List<Device> GetAllDevices()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            List<Device> dl = new List<Device>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetDevices", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new Device(dr).DateArchived == maxDate)
                    {
                        dl.Add(new Device(dr));
                    }
                }


            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dl;
        }

        /// <summary>
        /// Takes in a BrandID
        /// </summary>
        /// <param name="ID">BrandID</param>
        /// <returns>A List of Device objects</returns>
        internal static List<Device> GetDevicesByBrandID(int ID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            List<Device> dl = new List<Device>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetDevicesByBrandID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_BrandID", ID);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    dl.Add(new Device(dr));
                }

                return dl;

            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dl;
        }

        /// <summary>
        /// Takes in a DeviceID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>A Device object</returns>
        internal static Device GetDeviceByID(int ID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            Device Device = new Device();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetDeviceByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_DeviceID", ID);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Device = new Device(dr);
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return Device;
        }

        /// <summary>
        /// Adds a device to the database.
        /// </summary>
        /// <param name="b">The device provided.</param>
        /// <returns>-1 if not added</returns>
        internal static int AddDevice(Device d)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_AddDevice", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_Name", d.Name);
                cmd.Parameters.AddWithValue("@i_BrandID", d.BrandID);

                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Gets device from database by name
        /// </summary>
        /// <param name="Name">Device Name</param>
        /// <returns>DeviceID</returns>
        internal static int GetDeviceByName(string Name)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            Device Device = new Device();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetDeviceByName", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@i_Name", Name);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Device = new Device(dr);
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return Device.ID;
        }

        /// <summary>
        /// Update an Device in the database
        /// </summary>
        /// <param name="a">Device</param>
        /// <returns>-1 if not updated</returns>
        internal static int UpdateDevice(Device d)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordUpdated = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_UpdateDeviceByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);

                cmd.Parameters.AddWithValue("@i_DeviceID", d.ID);
                cmd.Parameters.AddWithValue("@i_Name", d.Name);
                cmd.Parameters.AddWithValue("@i_BrandID", d.BrandID);
                cmd.Parameters.AddWithValue("@i_DateArchived", maxDate);

                recordUpdated = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordUpdated;
        }

        /// <summary>
        /// Archive a Device
        /// </summary>
        /// <param name="id">DeviceID</param>
        /// <returns>-1 if not Archived</returns>
        public static int ArchiveDeviceByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordArchived = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_ArchiveDeviceByID", conn);
                cmd.Parameters.AddWithValue("i_DeviceID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                recordArchived = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordArchived;
        }

        /// <summary>
        /// Restores the disabled Device by the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int RestoreDeviceByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_RestoreDeviceByID", conn);
                cmd.Parameters.AddWithValue("i_DeviceID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Get all of the archived Devices from the database.
        /// </summary>
        internal static List<Device> GetAllArchivedDevices()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetDevices", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                List<Device> dl = new List<Device>();
                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new Device(dr).DateArchived != maxDate)
                    {
                        dl.Add(new Device(dr));
                    }
                }

                return dl;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }
        #endregion

        #region Locations

        /// <summary>
        /// Get a list of all Locations from the database
        /// </summary>
        internal static List<Location> GetAllLocations()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            List<Location> ll = new List<Location>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetLocations", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new Location(dr).DateArchived == maxDate)
                    {
                        ll.Add(new Location(dr));
                    }
                }
                
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return ll;
        }

        /// <summary>
        /// Takes in a LocationID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>A Location object</returns>
        internal static Location GetLocationByID(int ID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            Location Location = new Location();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetLocationByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_LocationID", ID);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Location = new Location(dr);
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return Location;
        }

        /// <summary>
        /// Add a location to the database
        /// </summary>
        /// <param name="l">Location Object</param>
        /// <returns>-1 if not added</returns>
        internal static int AddLocation(Location l)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_AddLocation", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@i_Name", l.Name);
                cmd.Parameters.AddWithValue("i_StateID", l.StateID);
                cmd.Parameters.AddWithValue("@i_PLocationID", -1);
                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Update an Location in the database
        /// </summary>
        /// <param name="a">Location</param>
        /// <returns>-1 if not updated</returns>
        internal static int UpdateLocation(Location l)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;
            int recordUpdated = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_UpdateLocationByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_LocationID", l.ID);
                cmd.Parameters.AddWithValue("@i_Name", l.Name);
                cmd.Parameters.AddWithValue("@i_StateID", l.StateID);
                cmd.Parameters.AddWithValue("@i_PLocationID", -1);

                recordUpdated = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordUpdated;
        }

        /// <summary>
        /// Archive a Location
        /// </summary>
        /// <param name="id">LocationID</param>
        /// <returns>-1 if not Archived</returns>
        public static int ArchiveLocationByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordArchived = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_ArchiveLocationByID", conn);
                cmd.Parameters.AddWithValue("i_LocationID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                recordArchived = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordArchived;
        }

        /// <summary>
        /// Restores the disabled Location by the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int RestoreLocationByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_RestoreLocationByID", conn);
                cmd.Parameters.AddWithValue("i_LocationID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Get all of the archived Locations from the database.
        /// </summary>
        internal static List<Location> GetAllArchivedLocations()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetLocations", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                List<Location> ll = new List<Location>();
                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new Location(dr).DateArchived != maxDate)
                    {
                        ll.Add(new Location(dr));
                    }
                }

                return ll;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }
        #endregion

        #region Models
        /// <summary>
        /// Gets all of the models from the database.
        /// </summary>
        internal static List<Model> GetAllModels()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            List<Model> ml = new List<Model>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetModels", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new Model(dr).DateArchived == maxDate)
                    {
                        ml.Add(new Model(dr));
                    }
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return ml;
        }


        /// <summary>
        /// Adds a model to the database.
        /// </summary>
        /// <param name="m">The model provided.</param>
        /// <returns></returns>
        internal static int AddModel(Model m)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_AddModel", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_Name", m.Name);

                //This one I'm not sure about. The Model doesn't have a DeviceID, but the sproc does. Why is this?
                cmd.Parameters.AddWithValue("@i_DeviceID", m.DeviceID);

                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Takes in a ModelID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>A Model object</returns>
        internal static Model GetModelByID(int ID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            Model Model = new Model();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetModelByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_ModelID", ID);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Model = new Model(dr);
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return Model;
        }

        /// <summary>
        /// Gets a model by the name
        /// </summary>
        /// <param name="Name">Model Name</param>
        /// <returns>Model Object</returns>
        internal static Model GetModelByName(string Name)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            Model Model = new Model();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetModelByName", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@i_ModelName", Name);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Model = new Model(dr);
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return Model;
        }

        /// <summary>
        /// Takes in a DeviceID
        /// </summary>
        /// <param name="ID">DeviceID</param>
        /// <returns>A List of Model objects</returns>
        internal static List<Model> GetModelsByDeviceID(int ID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            List<Model> ml = new List<Model>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetModelsByDeviceID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_DeviceID", ID);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ml.Add(new Model(dr));
                }

                return ml;

            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return ml;
        }

        /// <summary>
        /// Gets the last Model from the Models table in the database.
        /// </summary>
        /// <returns></returns>
        internal static Model GetLastModel()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetLastAddedModel", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                Model m = new Model();

                while (dr.Read())
                {
                    m = new Model(dr);
                }
                return m;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        /// <summary>
        /// Update an Model in the database
        /// </summary>
        /// <param name="a">Model</param>
        /// <returns>-1 if not updated</returns>
        internal static int UpdateModel(Model m)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordUpdated = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_UpdateModelByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);

                cmd.Parameters.AddWithValue("@i_ModelID", m.ID);
                cmd.Parameters.AddWithValue("@i_Name", m.Name);
                cmd.Parameters.AddWithValue("@i_DeviceID", m.DeviceID);
                cmd.Parameters.AddWithValue("@i_DateArchived", maxDate);

                recordUpdated = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordUpdated;
        }

        /// <summary>
        /// Archive a Model
        /// </summary>
        /// <param name="id">ModelID</param>
        /// <returns>-1 if not Archived</returns>
        public static int ArchiveModelByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordArchived = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_ArchiveModelByID", conn);
                cmd.Parameters.AddWithValue("i_ModelID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                recordArchived = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordArchived;
        }

        /// <summary>
        /// Restores the disabled Model by the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int RestoreModelByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_RestoreModelByID", conn);
                cmd.Parameters.AddWithValue("i_ModelID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Get all of the archived Models from the database.
        /// </summary>
        internal static List<Model> GetAllArchivedModels()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetModels", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                List<Model> ml = new List<Model>();
                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new Model(dr).DateArchived != maxDate)
                    {
                        ml.Add(new Model(dr));
                    }
                }

                return ml;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }
        #endregion

        #region Note


        internal static IEnumerable<Note> GetNotesByTicketID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            List<Note> retList = new List<Note>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetNotesByTicketID", conn);
                cmd.Parameters.AddWithValue("@i_TicketID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    retList.Add(new Note(dr));
                }
                conn.Close();
                return retList;
            }
            catch (Exception ex)
            {
                conn.Close();
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Add a note to a ticket
        /// </summary>
        /// <param name="Description">Pass in the note</param>
        /// <param name="UserID">Pass in the User that created the note</param>
        /// <param name="TicketID">Pass in the ticket ID that the note is for</param>
        /// <returns>-1 if the note is note added</returns>
        internal static int AddNote(string Description, int UserID, int TicketID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_AddNote", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_Note", Description);
                cmd.Parameters.AddWithValue("@i_UserID", UserID);
                cmd.Parameters.AddWithValue("@i_TicketID", TicketID);

                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        
        
        #endregion

        #region State

        /// <summary>
        /// Get a list of all State from the database
        /// </summary>
        internal static List<State> GetAllStates()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            List<State> sl = new List<State>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetStates", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new State(dr).DateArchived == maxDate)
                    {
                        sl.Add(new State(dr));
                    }
                }

            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return sl;
        }

        /// <summary>
        /// Takes in a StateID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>A State object</returns>
        internal static State GetStateByID(int ID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            State State = new State();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetStateByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_StateID", ID);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    State = new State(dr);
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return State;
        }

        /// <summary>
        /// Add a state to the database
        /// </summary>
        /// <param name="s">State Object</param>
        /// <returns>-1 if not added</returns>
        internal static int AddState(State s)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_AddStates", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@i_Name", s.Name);

                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Update an State in the database
        /// </summary>
        /// <param name="a">State</param>
        /// <returns>-1 if not updated</returns>
        internal static int UpdateState(State s)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;
            int recordUpdated = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_UpdateStateByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);

                cmd.Parameters.AddWithValue("@i_StateID", s.ID);
                cmd.Parameters.AddWithValue("@i_Name", s.Name);
                cmd.Parameters.AddWithValue("@i_DateArchived", maxDate);

                recordUpdated = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordUpdated;
        }

        /// <summary>
        /// Archive a State
        /// </summary>
        /// <param name="id">StateID</param>
        /// <returns>-1 if not Archived</returns>
        public static int ArchiveStateByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordArchived = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_ArchiveStateByID", conn);
                cmd.Parameters.AddWithValue("i_StateID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                recordArchived = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordArchived;
        }

        /// <summary>
        /// Restores the disabled State by the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int RestoreStateByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_RestoreStateByID", conn);
                cmd.Parameters.AddWithValue("i_StateID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Get all of the archived States from the database.
        /// </summary>
        internal static List<State> GetAllArchivedStates()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetStates", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                List<State> sl = new List<State>();
                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new State(dr).DateArchived != maxDate)
                    {
                        sl.Add(new State(dr));
                    }
                }

                return sl;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }
        #endregion

        #region Status

        /// <summary>
        /// Get a list of all statuses from the database
        /// </summary>
        internal static List<Status> GetAllStatuses()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            List<Status> sl = new List<Status>();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetStatuses", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    sl.Add(new Status(dr));
                }


            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return sl;
        }

        /// <summary>
        /// Takes in a StatusID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>A Status object</returns>
        internal static Status GetStatusByID(int ID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            Status Status = new Status();
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetStatusByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_StatusID", ID);

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Status = new Status(dr);
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return Status;
        }
        #endregion

        #region Ticket
        /// <summary>
        /// Get all of the tickets from the database.
        /// </summary>
        internal static List<Ticket> GetAllTickets()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetTickets", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                List<Ticket> tl = new List<Ticket>();
                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                   if (new Ticket(dr).DateResolved == maxDate)
                  {
                        tl.Add(new Ticket(dr));
                  }
                }

                return tl;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        /// <summary>
        /// Restores the resolved Ticket by the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int RestoreTicketByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_RestoreTicketByID", conn);
                cmd.Parameters.AddWithValue("i_TicketID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Get all of the resolved tickets from the database.
        /// </summary>
        internal static List<Ticket> GetAllResolvedTickets()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetTickets", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                List<Ticket> tl = new List<Ticket>();
                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new Ticket(dr).DateResolved != maxDate)
                    {
                        tl.Add(new Ticket(dr));
                    }
                }

                return tl;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }


        /// <summary>
        /// Gets the last ticket from the tickets table in the database.
        /// </summary>
        /// <returns></returns>
        internal static Ticket GetLastTicket()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetLastTicket", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                Ticket t = new Ticket();

                while (dr.Read())
                {
                    t = new Ticket(dr);
                }
                return t;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        /// <summary>
        /// Add a ticket to the database
        /// </summary>
        /// <param name="t">The created ticket and its properties.</param>
        /// <returns>Returns ID of new ticket if its added, or -1 is returned if it was not added.</returns>
        internal static int AddTicket(Ticket t, int UserID, int RoleID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_AddTicket", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                Ticket lastTicketAdded = GetLastTicket();


                t.ID = lastTicketAdded.ID + 1;
                t.Number = t.ID;

                cmd.Parameters.AddWithValue("@i_Number", t.Number);
                cmd.Parameters.AddWithValue("@i_Subject", t.Subject);
                cmd.Parameters.AddWithValue("@i_Description", t.Description);
                cmd.Parameters.AddWithValue("@i_DateDue", t.DateDue);
                cmd.Parameters.AddWithValue("@i_CategoryID", t.CategoryID);
                cmd.Parameters.AddWithValue("@i_UserID", UserID);
                cmd.Parameters.AddWithValue("@i_RoleID", RoleID);

                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Update a ticket in the database
        /// </summary>
        /// <param name="t">Ticket Object</param>
        /// <returns>-1 if not added</returns>
        internal static int UpdateTicket(Ticket t)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_UpdateTicketByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_TicketID", t.ID);
                cmd.Parameters.AddWithValue("@i_Number", t.Number);
                cmd.Parameters.AddWithValue("@i_Subject", t.Subject);
                cmd.Parameters.AddWithValue("@i_Description", t.Description);
                cmd.Parameters.AddWithValue("@i_DateCreated", t.DateCreated);
                cmd.Parameters.AddWithValue("@i_DateLastUpdated", t.DateLastUpdated);
                cmd.Parameters.AddWithValue("@i_DateDue", t.DateDue);
                cmd.Parameters.AddWithValue("@i_DateResolved", t.DateResolved);
                cmd.Parameters.AddWithValue("@i_StatusID", t.StatusID);
                cmd.Parameters.AddWithValue("@i_CategoryID", t.CategoryID);

                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Get Ticket From database by ID
        /// </summary>
        /// <param name="id">TicketID</param>
        /// <returns>null if not found, or ticket object</returns>
        public static Ticket GetTicketByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            Ticket retObj = null;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetTicketByID", conn);
                cmd.Parameters.AddWithValue("@i_TicketID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    retObj = new Ticket(dr);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return retObj;
        }

        /// <summary>
        /// Resolve a ticket
        /// </summary>
        /// <param name="id">TicketID</param>
        /// <returns>-1 if not resolved</returns>
        public static int ResolveTicketByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_ResolveTicketByID", conn);
                cmd.Parameters.AddWithValue("i_TicketID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
                        
        }

        public static int DeleteUserTicketsRoles(int UserID, int TicketID, int RoleID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;
            var recordDeleted = -1;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_DeleteUserTicketsRoles", conn);
                cmd.Parameters.AddWithValue("i_UserID", UserID);
                cmd.Parameters.AddWithValue("i_TicketID", TicketID);
                cmd.Parameters.AddWithValue("i_RoleID", RoleID);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                recordDeleted = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return recordDeleted;
        }
      
        public static int AddUserTicketsRoles(int UserID, int TicketID, int RoleID)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;
            var recordAdded = -1;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_AddUserTicketsRoles", conn);
                cmd.Parameters.AddWithValue("i_UserID", UserID);
                cmd.Parameters.AddWithValue("i_TicketID", TicketID);
                cmd.Parameters.AddWithValue("i_RoleID", RoleID);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return recordAdded;
        }

        public static UserTicketRole GetUserTicketsRolesByTicketID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;

            UserTicketRole utr = null;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetUserTicketsRolesByTicketID", conn);
                cmd.Parameters.AddWithValue("i_TicketID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.FieldCount > 1)
                {
                    while (dr.Read())
                    {
                        utr = new UserTicketRole(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return utr;
        }

        public static UserTicketRole GetUserTicketsRolesByUserID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;

            UserTicketRole utr = null;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetUserTicketsRolesByUserID", conn);
                cmd.Parameters.AddWithValue("i_UserID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.FieldCount > 1)
                {
                    while (dr.Read())
                    {
                        utr = new UserTicketRole(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return utr;
        }

        public static UserTicketRole GetUserTicketsRolesByRoleID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;

            UserTicketRole utr = null;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetUserTicketsRolesByRoleID", conn);
                cmd.Parameters.AddWithValue("i_RoleID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.FieldCount > 1)
                {
                    while (dr.Read())
                    {
                        utr = new UserTicketRole(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return utr;
        }
        #endregion

        #region User
        /// <summary>
        /// Gets the User corresponding with the given email and password. This allows the user to login.
        /// </summary>
        /// <remarks></remarks>
        public static User GetUser(string Email, string Password)

        {
            MySqlConnection com = new MySqlConnection();
            com.ConnectionString = _ReadConnectionString;
            User retObj = null;

            try
            {
                com.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetUserByEmail", com);
                cmd.Parameters.AddWithValue("@i_Email", Email);
                cmd.Parameters.AddWithValue("@i_Password", Password);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    retObj = new User(dr);
                }
                com.Close();
            }
            catch (Exception ex)
            {
                com.Close();
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            /// Verify password matches.
            /// Jon's code for verify hash and password are equal
            if (retObj != null)
            {
                //Checks to see if the hash and the password provided match.
                if (!Tools.Hash.IsValid(Password, retObj.Salt, _Pepper, _Stretches, retObj.Password))
                {
                    retObj = null;
                }
            }
            return retObj;
        }

        /// <summary>
        /// Get the User to a note object from the database
        /// </summary>
        /// <param name="ID">NoteID</param>
        /// <returns>a User</returns>
        internal static User GetUserByNoteID(int ID)
        {
            MySqlConnection com = new MySqlConnection();
            com.ConnectionString = _ReadConnectionString;
            User retObj = null;
            try
            {
                com.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetUserByNoteID", com);
                cmd.Parameters.AddWithValue("@i_NoteID", ID);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    retObj = new User(dr);
                }
                com.Close();
            }
            catch (Exception ex)
            {
                com.Close();
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return retObj;
        }

        /// <summary>
        /// Add a user to the database
        /// </summary>
        /// <param name="u">User Object</param>
        /// <returns>-1 if not added</returns>
        internal static int AddUser(User u)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                u.Salt = Tools.Hash.GenerateSalt(50);
                string NewPass = Tools.Hash.Get(u.Password, u.Salt, _Pepper, _Stretches, 64);
                u.Password = NewPass;

                MySqlCommand cmd = new MySqlCommand("sproc_AddUser", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_FirstName", u.FirstName);
                cmd.Parameters.AddWithValue("@i_LastName", u.LastName);
                cmd.Parameters.AddWithValue("@i_Email", u.Email);
                cmd.Parameters.AddWithValue("@i_ID", u.ID);
                cmd.Parameters.AddWithValue("@i_Password", u.Password);
                cmd.Parameters.AddWithValue("@i_Salt", u.Salt);
                cmd.Parameters.AddWithValue("@i_RoleID", u.RoleID);

                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Gets a list of User objects from the database.
        /// </summary>
        /// <remarks> This code is from Jon Holmes initially.</remarks>
        public static List<User> GetUsers()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetUsers", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                List<User> u = new List<User>();
                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new User(dr).DateDisabled == maxDate)
                    {
                        u.Add(new User(dr));
                    }
                }
                return u;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        /// <summary>
        /// Update a user in the database
        /// </summary>
        /// <param name="u">User Object</param>
        /// <returns>-1 if not updated</returns>
        public static int UpdateUser(User u)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordUpdated = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_UpdateUserByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("i_UserID", u.ID);
                cmd.Parameters.AddWithValue("i_Email", u.Email);
                cmd.Parameters.AddWithValue("i_FirstName", u.FirstName);
                cmd.Parameters.AddWithValue("i_LastName", u.LastName);
                cmd.Parameters.AddWithValue("i_RoleID", u.RoleID);

                recordUpdated = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordUpdated;
        }

        /// <summary>
        /// Update a password for a user
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <param name="password">New password</param>
        /// <returns>New password and salt for user</returns>
        public static int UpdateUserPassword(int id, string password)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordUpdated = -1;

            try
            {
                conn.Open();
                string NewSalt = Tools.Hash.GenerateSalt(50);
                string NewPass = Tools.Hash.Get(password, NewSalt, _Pepper, _Stretches, 64);
                string pass= NewPass;
                
                MySqlCommand cmd = new MySqlCommand("sproc_UpdatePasswordByUserID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("i_UserID", id);
                cmd.Parameters.AddWithValue("i_Password", pass);
                cmd.Parameters.AddWithValue("i_Salt", NewSalt);

                recordUpdated = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordUpdated;
        }

        
       		/// <summary>
        /// Disable the user by the passed in ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DisableUserByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_DisableUserByID", conn);
                cmd.Parameters.AddWithValue("i_UserID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Restores the disabled user by the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int RestoreUserByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_RestoreUserByID", conn);
                cmd.Parameters.AddWithValue("i_UserID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }
		
		/// <summary>
        /// Get all of the Disabled Users from the database.
        /// </summary>
        internal static List<User> GetAllDisabledUsers()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetUsers", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                List<User> tl = new List<User>();
                DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59);
                while (dr.Read())
                {
                    if (new User(dr).DateDisabled != maxDate)
                    {
                        tl.Add(new User(dr));
                    }
                }

                return tl;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        /// <summary>
        /// Get a user from the database by ID
        /// </summary>
        /// <param name="id">UserID</param>
        /// <returns>null if nto found, or User Object</returns>
        public static User GetUserByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            User retObj = null;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetUserByID", conn);
                cmd.Parameters.AddWithValue("@i_UserID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    retObj = new User(dr);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return retObj;
        }

        #endregion
        
        #region Role
        /// This region was written by Jack Bradley starting on 3/27/19
        /// This gets all of the roles in the database.
        public static List<Role> GetRoles()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetRoles", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataReader dr = cmd.ExecuteReader();
                List<Role> r = new List<Role>();
                while (dr.Read())
                {
                    r.Add(new Role(dr));
                }
                return r;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        /// <summary>
        /// Add Role based on all of the different permissions. Coded by one Mr. Jack Bradley
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        internal static int AddRoles(Role r)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordAdded = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_AddRole", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_Title", r.Title);
                cmd.Parameters.AddWithValue("@i_TicketsView", r.TicketsView);
                cmd.Parameters.AddWithValue("@i_TicketsComment", r.TicketsComment);
                cmd.Parameters.AddWithValue("@i_TicketsResolve", r.TicketsResolve);
                cmd.Parameters.AddWithValue("@i_TicketsOpen", r.TicketsOpen);
                cmd.Parameters.AddWithValue("@i_TicketsEdit", r.TicketsEdit);
                cmd.Parameters.AddWithValue("@i_AssetsView", r.AssetsView);
                cmd.Parameters.AddWithValue("@i_AssetsAdd", r.AssetsAdd);
                cmd.Parameters.AddWithValue("@i_AssetsEdit", r.AssetsEdit);
                cmd.Parameters.AddWithValue("@i_AssetsArchive", r.AssetsArchive);
                cmd.Parameters.AddWithValue("@i_UsersView", r.UsersView);
                cmd.Parameters.AddWithValue("@i_UsersAdd", r.UsersAdd);
                cmd.Parameters.AddWithValue("@i_UsersEdit", r.UsersEdit);
                cmd.Parameters.AddWithValue("@i_UsersDisable", r.UsersDisable);
                cmd.Parameters.AddWithValue("@i_RolesView", r.RolesView);
                cmd.Parameters.AddWithValue("@i_DeleteAsset", r.DeleteAsset);

                recordAdded = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordAdded;
        }

        /// <summary>
        /// Update a role in the database
        /// </summary>
        /// <param name="r">Role Object</param>
        /// <returns>-1 if not updated</returns>
        internal static int UpdateRole(Role r)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordUpdated = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_UpdateRoleByID", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_RoleID", r.ID);
                cmd.Parameters.AddWithValue("@i_Title", r.Title);
                cmd.Parameters.AddWithValue("@i_TicketsView", r.TicketsView);
                cmd.Parameters.AddWithValue("@i_TicketsComment", r.TicketsComment);
                cmd.Parameters.AddWithValue("@i_TicketsResolve", r.TicketsResolve);
                cmd.Parameters.AddWithValue("@i_TicketsOpen", r.TicketsOpen);
                cmd.Parameters.AddWithValue("@i_TicketsEdit", r.TicketsEdit);
                cmd.Parameters.AddWithValue("@i_AssetsView", r.AssetsView);
                cmd.Parameters.AddWithValue("@i_AssetsAdd", r.AssetsAdd);
                cmd.Parameters.AddWithValue("@i_AssetsEdit", r.AssetsEdit);
                cmd.Parameters.AddWithValue("@i_AssetsArchive", r.AssetsArchive);
                cmd.Parameters.AddWithValue("@i_UsersView", r.UsersView);
                cmd.Parameters.AddWithValue("@i_UsersAdd", r.UsersAdd);
                cmd.Parameters.AddWithValue("@i_UsersEdit", r.UsersEdit);
                cmd.Parameters.AddWithValue("@i_UsersDisable", r.UsersDisable);
                cmd.Parameters.AddWithValue("@i_RolesView", r.RolesView);

                recordUpdated = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordUpdated;
        }

        /// <summary>
        /// Get a Role from the database by ID
        /// </summary>
        /// <param name="id">RoleID</param>
        /// <returns>null if not found, or Role Object</returns>
        public static Role GetRoleByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _ReadConnectionString;
            Role retObj = null;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_GetRoleByID", conn);
                cmd.Parameters.AddWithValue("@i_RoleID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    retObj = new Role(dr);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return retObj;
        }

        /// <summary>
        /// Delete a role
        /// </summary>
        /// <param name="id">RoleID</param>
        /// <returns>-1 if not deleted</returns>
        public static int DeleteRoleByID(int id)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = _EditConnectionString;

            int recordDeleted = -1;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("sproc_DeleteRoleByID", conn);
                cmd.Parameters.AddWithValue("i_RoleID", id);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                recordDeleted = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return recordDeleted;
        }
        #endregion
    }
}