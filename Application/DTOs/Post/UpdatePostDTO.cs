//namespace Tourism-Agency;

namespace Application.DTOs.Post
{
    public class UpdatePostDTO
    {
        public int Id { get; set; }  //  To select which post to update
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public int PostTypeId { get; set; }
    }
}
