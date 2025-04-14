using DataAccess.Entities;
using DTO.PostTag;
using DTO.PostType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Post
{
    public class ReturnPostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string? Image { get; set; }
        public string Slug { get; set; }
        public int Views { get; set; }
        public PostStatusEnum Status { get; set; }
        public string? Summary { get; set; }
        public DateTime? PublishDate { get; set; }

      //  public ReturnEmployeeDTO Author { get; set; }
        public ReturnPostTypeDTO PostType { get; set; }
        public List<ReturnPostTagDTO> PostTags { get; set; }
    }
}
