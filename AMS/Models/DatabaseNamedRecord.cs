//This code is originally from Jon Holmes
// Jack Bradley began implementing it for the DropTables project on March 3rd 2019. 
///This is used for almost every model this project has. 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Models {
    public abstract class DatabaseRecord {
        protected int _ID;
        /// <summary>
        /// The User given Name for the Object.
        /// </summary>
        [Display(Name = "ID")]
        public int ID {
            get { return _ID; }
            set { _ID = value; }
        }
        public abstract int dbSave();

        protected abstract int dbAdd();

        protected abstract int dbUpdate();

        public abstract void Fill(MySql.Data.MySqlClient.MySqlDataReader dr);

        public abstract override string ToString();
    }
    /// <summary>
    /// An abstract class that inherents the name property, and then makes it public. 
    /// </summary>
    public abstract class DatabaseNamedRecord : DatabaseRecord {
        protected string _Name;
        /// <summary>
        /// The User given Name for the Object.
        /// </summary>
        //[Display(Name = "Name")]
        [DataType(DataType.Text)]
        //[Required]
        [Display(Name = "Name")]
        public String Name {
            get { return _Name; }
            set { _Name = value; }
        }
    }
}
