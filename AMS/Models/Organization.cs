///
/// Coded by Jack Bradley 2/13/19
///



/// <summary>
/// This model is to track the organizations that can be involved in a ticket. An example of an organization could
/// be the College of Business, or the Physics Department.
/// </summary>
namespace AMS.Models
{

	public class Organization : DatabaseNamedObject
	{
		private string _Description;

        public Organization()
        {

        }
        public Organization(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

        /// <summary>
        /// If the Description is null, then we should set the Description
        /// String to be blank. 
        /// </summary>
        public string Description
		{
			get { return _Description; }
			set
			{
				if (value == null)
				{
					_Description = "";
				}
				else
				{
					_Description = value;
				}
			}
		}

        /// <summary>
        /// Fills the object from the datareader
        /// </summary>
        /// <param name="dr"></param>
        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            ID = dr.GetInt32("ModelID");
            Name = dr.GetString("Name");
            Description = dr.GetString("Description");
        }
    }
}