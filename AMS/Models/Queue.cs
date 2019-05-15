//This was programmed by Chas Davis

namespace AMS.Models
{

	public class Queue : DatabaseNamedObject
	{
		private int _TicketID;
		private int _UserID;

        public Queue()
        {

        }
        public Queue(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

         /// <summary>
         /// Foreign key for the tickets table
         /// </summary>
        public int TicketID
		{
			get { return _TicketID; }
			set { _TicketID = value; }
		}


		/// <summary>
        /// foreign key for the user table
        /// </summary>
		public int UserID
		{
			get { return _UserID; }
			set { _UserID = value; }
		}

        /// <summary>
        /// Fills the object from the datareader
        /// </summary>
        /// <param name="dr"></param>
        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            ID = dr.GetInt32("QueueID");
            Name = dr.GetString("Name");
            TicketID = dr.GetInt16("TicketID");
            UserID = dr.GetInt32("UserID");
        }
    }
}