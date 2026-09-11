using System.ComponentModel.DataAnnotations.Schema;

namespace OrgSys.SharedKernel;

/// <summary>
/// Moved from Domain/Entities/BaseModel.cs verbatim (shape only — no navigation properties,
/// so it has zero cross-module coupling) per docs/modular-monolith-target-architecture.md §2.
/// Every module entity still inherits this exact shape; only its location changed.
/// </summary>
public class BaseModel
{
    public virtual long Id { get; set; }

    public virtual long CodeNumber { get; set; }

    public virtual string? Code { get; set; }

    public string? MaskText { get; set; }

    public long ParentId { get; set; }

    public long TypeId { get; set; }

    public bool Hide { get; set; }

    public string? ImgPath { get; set; }

    public Status Status { get; set; }

    [NotMapped]
    public List<string>? CssFiles { get; set; }
}
