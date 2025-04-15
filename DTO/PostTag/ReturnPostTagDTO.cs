using DTO.Post;
using DTO.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.PostTag
{
   public class ReturnPostTagDTO
    {
        public int PostId { get; set; }
        public int TagId { get; set; }
        public ReturnPostDTO? Post { get; set; }
        public ReturnTagDTO? Tag { get; set; }

    }
}
