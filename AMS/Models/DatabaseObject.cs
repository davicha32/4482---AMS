//This code was implemented by Jack Brtadley 2/17/19
//It is from Jon Holmes  originally.
//This has mostly beeen migrated out in favor of DatabaseNamedRecord or NamedObject. 

using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
	public abstract class DatabaseObject
	{
		private int _ID;

        /// <summary>
        /// ID for all models that inherit from this model
        /// </summary>
		[Key]
		public int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

	}
}