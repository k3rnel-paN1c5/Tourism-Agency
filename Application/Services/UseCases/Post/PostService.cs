//namespace Tourism-Agency;
using Application.DTOs.Post;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.IRepositories;
using Application.Utilities;

namespace Application.Services.UseCases
{
    public class PostService : IPostService
    {
        private readonly IRepository<Domain.Entities.Post, int> _postRepository;
        private readonly IRepository<Domain.Entities.Employee, string> _employeeRepository;
        private readonly IMapper _mapper;



        public PostService(IRepository<Domain.Entities.Post, int> postRepository, IRepository<Domain.Entities.Employee, string> employeeRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<GetPostDTO> CreatePostAsync(CreatePostDTO dto)
        {
            //  Check if the employee exists before creating the post
            var employeeExists = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
            if (employeeExists == null)
            {
                throw new Exception("Invalid employeeId! The employee does not exist.");
            }

            // Convert CreatePostDTO to Post using Mapper
            var post = _mapper.Map<Domain.Entities.Post>(dto);


            post.Slug = SlugHelper.GenerateSlug(dto.Title); // Manually generate Slug
            post.PublishDate = DateTime.UtcNow;  // Automatically set publish date


            await _postRepository.AddAsync(post);
            await _postRepository.SaveAsync();


            return _mapper.Map<GetPostDTO>(post);
        }

        public async Task<GetPostDTO> UpdatePostAsync(UpdatePostDTO dto)
        {
            // Retrieve the post from the database
            var post = await _postRepository.GetByIdAsync(dto.Id);
            if (post == null)
            {
                throw new Exception("Post not found!");
            }

            // Use Mapper to update only the provided values
            _mapper.Map(dto, post);

            // Log update confirmation in terminal
            Console.WriteLine($"Post Updated: {post.Id} - {post.Title} - {post.Body}");

            // Save the updated entity
            _postRepository.Update(post);
            await _postRepository.SaveAsync();

            return _mapper.Map<GetPostDTO>(post); // Return updated post DTO
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            // Retrieve the post from the database
            var post = await _postRepository.GetByIdAsync(id);

            // If the post does not exist, throw an exception
            if (post == null)
            {
                throw new Exception("Post not found!");
            }

            // Remove the post from the repository
            _postRepository.Delete(post);

            // Save changes to the database
            await _postRepository.SaveAsync();

            // Log confirmation message in the terminal
            Console.WriteLine($"Post Deleted: {id}");

            return true; // Return success status
        }


    }
}
