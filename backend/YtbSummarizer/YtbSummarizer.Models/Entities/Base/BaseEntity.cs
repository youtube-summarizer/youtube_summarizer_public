using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YtbSummarizer.Models.Entities.Base;

public class BaseEntity : BaseEntityWithoutPrimaryKey
{
	public BaseEntity() { }

	public BaseEntity(int id)
	{
		Id = id;
	}

	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Key]
	public new int Id { get; protected set; }
}

public class BaseEntityWithoutPrimaryKey
{
	public int Id { get; protected set; }
}