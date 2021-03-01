using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Database.Models.Group
{
	[Table("groups")]
	public class DBGroup
	{
		[Column("group_id")]
		[Key]
		public int Id { get; set; }
		[Column("group_name")]
		public string Name { get; set; }
		[Column("group_short")]
		public string Short { get; set; }
		[Column("group_identifer")]
		public string Identifer { get; set; }
		[Column("group_description")]
		public string Description { get; set; }
		[Column("group_color")]
		public string Color { get; set; }
		[Column("group_type")]
		public int GroupType { get; set; }
		[Column("group_playmodes")]
		public int PlayModes { get; set; }
		[Column("group_leader")]
		public int Leader { get; set; }
		[Column("group_perms")]
		public int Permissions { get; set; }
		[Column("group_max_members")]
		public int MaxMembers { get; set; }
		[Column("group_settings")]
		public int Settings { get; set; }
	}
}
