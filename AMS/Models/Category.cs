///
/// Coded by Jack Bradley 
/// Updated by Paul Michael 3/5/2019
///

using System.ComponentModel.DataAnnotations;


/// <summary>
/// This is a model for Category. They are used to distinguish assets.
/// </summary>
namespace AMS.Models
{
    public class Category : DatabaseNamedObject
    {
        public Category()
        {

        }
        /// <summary>
        /// Constructor that calls the fill method. 
        /// </summary>
        /// <param name="dr"></param>
        public Category(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

        /// <summary>
        /// Fills the object out with data from the datareader.
        /// </summary>
        /// <param name="dr"></param>
        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            ID = dr.GetInt32("CategoryID");
            Name = dr.GetString("Name");
        }
    }
}