using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Database.Models
{
	public class DBModel
	{
        public object this[string propertyName]
        {
            get
            {
                var prop = this.GetType().GetProperty(propertyName);
                return prop.GetValue(this);
            }
            set
            {
                var prop = this.GetType().GetProperty(propertyName);
                prop.SetValue(this, value);
            }
        }
    }
}
