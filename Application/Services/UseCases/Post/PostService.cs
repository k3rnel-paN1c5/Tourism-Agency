//namespace Tourism-Agency;
using Application.DTOs.Post;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.IRepositories;

namespace Application.Services.UseCases
{
public class PostService: IPostService
{
      private readonly IRepository<Domain.Entities.Post, int> _postRepository;
        private readonly IMapper _mapper;

        public PostService(IRepository<Domain.Entities.Post, int> postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<GetPostDTO> CreatePostAsync(CreatePostDTO dto)
        {
            // Convert CreatePostDTO to Post using Mapper
            var post = _mapper.Map<Domain.Entities.Post>(dto);

            // Modify values that need processing
            post.Slug = GenerateSlug(dto.Title); // ✅ Manually generate Slug
            post.PublishDate = DateTime.UtcNow;  // ✅ Automatically set publish date
            
                Console.WriteLine($"Post Created: {post.Title} - {post.Body}"); // ✅ Debug log

            await _postRepository.AddAsync(post);
            await _postRepository.SaveAsync();
            
            return _mapper.Map<GetPostDTO>(post);
        }

        private string GenerateSlug(string title) 
        {
            return title.ToLower().Replace(" ", "-");
        }
}
}

