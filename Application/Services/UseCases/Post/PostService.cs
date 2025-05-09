//namespace Tourism-Agency;
using Application.DTOs.Post;
using Application.IServices.UseCases.Post;
using AutoMapper;
using Domain.IRepositories;

namespace Application.Services.UseCases.Post
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
            var slug = GenerateSlug(dto.Title);
            var post = new Domain.Entities.Post
            {
                Title = dto.Title,
                Body = dto.Body,
                Slug = slug,
                Summary = dto.Summary,
                PublishDate = DateTime.UtcNow,
                PostTypeId = dto.PostTypeId,
                EmployeeId = dto.EmployeeId
            };
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

