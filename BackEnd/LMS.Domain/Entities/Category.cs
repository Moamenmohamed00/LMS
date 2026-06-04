using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
