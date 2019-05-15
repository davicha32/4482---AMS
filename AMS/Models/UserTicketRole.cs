
using System.ComponentModel.DataAnnotations;
using AMS.Data;

/// <summary>
/// This model is used to track users that are assigned to a specific ticket as a requestor on the ticket. 
/// </summary>
namespace AMS.Models
{
    public class UserTicketRole : DatabaseObject
    {
        private int _UserID;
        private User _User;
        private int _TicketID;
        private Ticket _Ticket;
        private int _RoleID;
        private Role _Role;

        public UserTicketRole()
        {

        }
        public UserTicketRole(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

        /// <summary>
        /// Foreign key to identify the Ticket that is loaned
        /// </summary>
        [Required]
        public int TicketID
        {
            get { return _TicketID; }
            set { _TicketID = value; }
        }

        /// <summary>
        /// Gets and sets the Ticket Object
        /// </summary>
        public Ticket Ticket
        {
            get
            {
                if (_Ticket == null)
                {
                    _Ticket = DAL.GetTicketByID(_TicketID);
                }
                return _Ticket;
            }
            set
            {
                _Ticket = value;
                if (value == null)
                {
                    _TicketID = -1;
                }
                else
                {
                    _TicketID = value.ID;
                }
            }
        }


        /// <summary>
        /// Foreign key to identify the User that the Ticket is loaned to
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

        [Required]
        public int RoleID
        {
            get { return _RoleID; }
            set { _RoleID = value; }
        }

        /// <summary>
        /// Gets and sets the Role Object
        /// </summary>
        public Role Role
        {
            get
            {
                if (_Role == null)
                {
                    _Role = DAL.GetRoleByID(_RoleID);
                }
                return _Role;
            }
            set
            {
                _Role = value;
                if (value == null)
                {
                    _RoleID = -1;
                }
                else
                {
                    _RoleID = value.ID;
                }
            }
        }

        /// <summary>
        /// Fills the user and Ticket onjects for loaned Tickets
        /// </summary>
        /// <param name="dr">DataReader</param>
        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            User = new User(dr);
            Ticket = new Ticket(dr);
            Role = new Role(dr);
            TicketID = dr.GetInt32("TicketID");
            UserID = dr.GetInt32("UserID");
            RoleID = dr.GetInt32("RoleID");

        }

    }
}