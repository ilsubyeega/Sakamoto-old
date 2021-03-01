using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Database.Models.Group
{
	[Table("user_groups")]
	public class DBUserGroup
	{
		[Key]
		[Column("group_id")]
		public int GroupId { get; set; }
		[Key]
		[Column("user_id")]
		public int UserId { get; set; }
		[Column("groupm_type")]
		public int MemberType { get; set; }
		[Column("groupm_description")]
		public string Description { get; set; }
	}
}
