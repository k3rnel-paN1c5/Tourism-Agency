//namespace Tourism-Agency;
using Domain.Enums;
namespace Application.DTOs.Post
{
        public class GetPostDTO
        {
                public int Id { get; set; }
                public string Title { get; set; } = string.Empty;
                public string Body { get; set; } = string.Empty;
                public string Summary { get; set; } = string.Empty;
                public string Slug { get; set; } = string.Empty;

                public PostStatus Status { get; set; }
                public DateTime PublishDate { get; set; }
                public int PostTypeId { get; set; }
                public string EmployeeId { get; set; } = string.Empty;
        }
}

