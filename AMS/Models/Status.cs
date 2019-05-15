///
/// Coded by Jack Bradley 2/13/19
///

using System.ComponentModel.DataAnnotations;


/// <summary>
/// This model is used to track the statuses of tickets. 
/// </summary>
namespace AMS.Models
{
	public class Status : DatabaseObject
	{
		private string _Condition;

        public Status()
        {

        }
        public Status(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

        /// Getter and Setter for the Condition of the status currently. 
        [Display(Name = "Status")]
		public string Condition
		{
			get { return _Condition; }
			set { _Condition = value; }
		}

        /// <summary>
        /// Fills the object using the datareader
        /// </summary>
        /// <param name="dr"></param>
        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            ID = dr.GetInt32("StatusID");
            Condition = dr.GetString("Status");
        }
    }
}
