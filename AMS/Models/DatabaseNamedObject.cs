//This code was implemented by Jack Bradley 2/17/19
//It is from Jon Holmes  originally.
//This isn't used by all of the models, just where a name property exists. 

using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
	public abstract class DatabaseNamedObject : DatabaseObject
	{

		private string _Name;

        /// <summary>
        /// Name property for all models that inherit from this model
        /// </summary>
        [Required]
		public string Name
		{
			get { return _Name; }
			set
			{
				if (value == null)
				{
					_Name = "";
				}
				else
				{
					_Name = value;
				}
			}
		}
	}
}