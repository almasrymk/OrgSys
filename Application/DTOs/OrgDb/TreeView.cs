using Domain.Entities;

namespace Application.DTOs
{
    public class TreeView : BaseModel
    {
        public string? Key { get; set; }

        public string? Value { get; set; }

        public bool Select { get; set; }
    }
}