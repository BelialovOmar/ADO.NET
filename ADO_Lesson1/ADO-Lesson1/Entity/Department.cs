using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_Lesson1.Entity
{
	public class Department
	{
		public Guid Id
		{
			get; set;
		}            // відбраження поля Id	 UNIQUEIDENTIFIER
		public String Name
		{
			get; set;
		}        // відбраження поля Name NVARCHAR(50)
		public DateTime? DeleteDt
		{
			get; set;
		} // ! типи БД та мови як правило відрізняються

		public Department()
		{
			Id = Guid.NewGuid();
			Name = null!;
		}

		public Department(DbDataReader reader)
		{
			Id = reader.GetGuid(0);
			Name = reader.GetString(1);
			DeleteDt = reader.IsDBNull(2)
						? null
						: reader.GetDateTime(2);
		}


	}
}
