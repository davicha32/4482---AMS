using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using AMS.Data;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
///
/// Coded by Jack Bradley 2/13/19
/// Much of this code was later taken from Jon Holmes and modified on March 2nd 2019 
/// And then Jack Bradley went out and refined it on 3/27

/// <summary>
/// This Model is used to track the roles a user could have in the system. An Example could be technician or administrator. 
/// Some of these permissions aren't ever utilized in the program. We wanted to have as much modularity and functionality when it came to Roles. Chas later refined this to support the ability to dynamically change permissions based on a 
/// specific role. That led to these various permissions checks being quite useful!
/// </summary>

namespace AMS.Models {

    public class Role : DatabaseObject {
        private string _Title;
        //private bool _IsAdmin;
        //private bool _IsTechnician;
        private bool _TicketsView;
        private bool _TicketsComment;
        private bool _TicketsResolve;
        private bool _TicketsOpen;
        private bool _TicketsEdit;
        private bool _AssetsView;
        private bool _AssetsAdd;
        private bool _AssetsEdit;
        private bool _AssetsArchive;
        private bool _UsersView;
        private bool _UsersAdd;
        private bool _UsersEdit;
        private bool _UsersDisable;
        private bool _RolesView;
        private bool _DeleteAsset;

        //Maybe add these later?
        //private bool _RolesEdit;
        //private bool _RolesAdd;
        //private bool _RolesDelete;

        public Role()
        {

        }
        public Role(MySql.Data.MySqlClient.MySqlDataReader dr)
        {
            Fill(dr);
        }

        [Display(Name = "Role")]
        [Required]
        public string Title {
            get { return _Title; }
            set { _Title = value; }
        }
        /// <summary>
        /// We concluded that these properties were irrelevant once we started using dynamic role/permissions.
        /// </summary>
        //public bool IsAdmin {
        //    get {
        //        return _IsAdmin;
        //    }
        //    set {
        //        _IsAdmin = value;
        //    }
        //}

        //public bool IsTechnician {
        //    get {
        //        return _IsTechnician;
        //    }
        //    set {
        //        _IsTechnician = value;
        //    }
        //}

        
        /// Gets and sets the bool permission: View Tickets
        [Display(Name = "View Ticket")]
        public bool TicketsView {
            get {
                return _TicketsView;
            }
            set {
                _TicketsView = value;
            }
        }

        /// Gets and sets the bool permission: Comment on Tickets
        [Display(Name = "Add Ticket Comments")]
        public bool TicketsComment {
            get {
                return _TicketsComment;
            }
            set {
                _TicketsComment = value;
            }
        }

        /// Gets and sets the bool permission: Resolve Tickets
        [Display(Name = "Resolve Ticket")]
        public bool TicketsResolve {
            get {
                return _TicketsResolve;
            }
            set {
                _TicketsResolve = value;
            }
        }

        /// Gets and sets the bool permission: Open Tickets
        [Display(Name = "Open Ticket")]
        public bool TicketsOpen {
            get {
                return _TicketsOpen;
            }
            set {
                _TicketsOpen = value;
            }
        }

        /// Gets and sets the bool permission: Edit Tickets
        [Display(Name = "Edit Ticket")]
        public bool TicketsEdit
        {
            get
            {
                return _TicketsEdit;
            }
            set
            {
                _TicketsEdit = value;
            }
        }

        /// Gets and sets the bool permission: View Assets
        [Display(Name = "View Asset")]
        public bool AssetsView {
            get {
                return _AssetsView;
            }
            set {
                _AssetsView = value;
            }
        }

        /// Gets and sets the bool permission: Add Assets
        [Display(Name = "Add Asset")]
        public bool AssetsAdd {
            get {
                return _AssetsAdd;
            }
            set {
                _AssetsAdd = value;
            }
        }
        /// Gets and sets the bool permission: Edit Assets
        [Display(Name = "Edit Asset")]
        public bool AssetsEdit
        {
            get
            {
                return _AssetsEdit;
            }
            set
            {
                _AssetsEdit = value;
            }
        }
        /// Gets and sets the bool permission: Archive Assets
        [Display(Name = "Archive Asset")]
        public bool AssetsArchive {
            get {
                return _AssetsArchive;
            }
            set {
                _AssetsArchive = value;
            }
        }
        /// Gets and sets the bool permission: View Users
        [Display(Name = "View User")]
        public bool UsersView
        {
            get
            {
                return _UsersView;
            }
            set
            {
                _UsersView = value;
            }
        }
        /// Gets and sets the bool permission: Add Users
        [Display(Name = "Add User")]
        public bool UsersAdd {
            get {
                return _UsersAdd;
            }
            set {
                _UsersAdd = value;
            }
        }
        /// Gets and sets the bool permission: Edit Users
        [Display(Name = "Edit User")]
        public bool UsersEdit
        {
            get
            {
                return _UsersEdit;
            }
            set
            {
                _UsersEdit = value;
            }
        }
        /// Gets and sets the bool permission: Disable Users
        [Display(Name = "Disable User")]
        public bool UsersDisable {
            get {
                return _UsersDisable;
            }
            set {
                _UsersDisable = value;
            }
        }
        /// Gets and sets the bool permission: View Roles
        [Display(Name = "View Role")]
        public bool RolesView
        {
            get
            {
                return _RolesView;
            }
            set
            {
                _RolesView = value;
            }
        }
        /// Gets and sets the bool permission: Delete Assets
       [Display(Name="Delete Asset")]
        public bool DeleteAsset {
            get {
                return _DeleteAsset;
            }
            set {
                _DeleteAsset = value;
            }
        }

        #region: Permission Sets 
        /// <summary>
        /// Gets or sets the User for this PeerVal.Role object.
        /// </summary>
        /// <remarks></remarks>
        //public PermissionSet User {
        //        get {
        //            return _Users;
        //        }
        //        set {
        //            _Users = value;
        //        }
        //    }

        /// <summary>
        /// Gets or sets the Role for this object.
        /// </summary>
        /// <remarks></remarks>
        //public PermissionSet Role {
        //    get {
        //        return _Role;
        //    }
        //    set {
        //        _Role = value;
        //    }
        //}

        #endregion
        public override string ToString() {
            return this.GetType().ToString();
        }

        //Database Strings
        internal const string db_ID = "RoleID";
        internal const string db_Name = "Name";
        internal const string db_IsAdmin = "IsAdmin";
        internal const string db_Users = "User";
        internal const string db_Role = "Role";

        /// <summary>
        /// Getter and setter for the Title of the Role itself. 
        /// </summary>
        /// <param name="title"></param>
        public Role(string title) {
            Title = title;
        }
 
        /// <summary>
        /// Fills the object using the datareader
        /// </summary>
        /// <param name="dr"></param>
        public void Fill(MySql.Data.MySqlClient.MySqlDataReader dr) {
            ID = dr.GetInt32("RoleID");
            Title = dr.GetString("Title");
            //IsAdmin = dr.GetBoolean("IsAdmin");
            //IsTechnician = dr.GetBoolean("IsTechnician");
            TicketsView = dr.GetBoolean("TicketsView");
            TicketsComment = dr.GetBoolean("TicketsComment");
            TicketsResolve = dr.GetBoolean("TicketsResolve");
            TicketsOpen = dr.GetBoolean("TicketsOpen");
            TicketsEdit = dr.GetBoolean("TicketsEdit");
            AssetsView = dr.GetBoolean("AssetsView");
            AssetsAdd = dr.GetBoolean("AssetsAdd");
            AssetsEdit = dr.GetBoolean("AssetsEdit");
            AssetsArchive = dr.GetBoolean("AssetsArchive");
            UsersView = dr.GetBoolean("UsersView");
            UsersAdd = dr.GetBoolean("UsersAdd");
            UsersEdit = dr.GetBoolean("UsersEdit");
            UsersDisable = dr.GetBoolean("UsersDisable");
            RolesView = dr.GetBoolean("RolesView");
            DeleteAsset = dr.GetBoolean("DeleteAsset");
        }
    }
}




