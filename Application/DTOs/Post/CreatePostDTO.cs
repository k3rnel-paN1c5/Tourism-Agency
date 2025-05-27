//namespace Tourism-Agency;
namespace Application.DTOs.Post
{
public class CreatePostDTO
{
    
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public int PostTypeId { get; set; }
}
}

