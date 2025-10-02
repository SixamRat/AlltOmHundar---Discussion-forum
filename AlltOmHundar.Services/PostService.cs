using AlltOmHundar.Core.Interfaces.Repositories;
using AlltOmHundar.Core.Interfaces.Services;
using AlltOmHundar.Core.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlltOmHundar.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private static readonly string[] ProfanityWords =
        {
            "fan", "helvete", "jävla", "jävlar", "skit", "idiot",
            "ass", "damn", "hell", "shit", "fuck", "bastard"
        };

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await _postRepository.GetByIdAsync(id);
        }

        public async Task<Post?> GetPostWithRepliesAsync(int id)
        {
            return await _postRepository.GetPostWithRepliesAsync(id);
        }

        public async Task<IEnumerable<Post>> GetPostsByTopicAsync(int topicId)
        {
            return await _postRepository.GetPostsByTopicAsync(topicId);
        }

        public async Task<IEnumerable<Post>> GetTopLevelPostsByTopicAsync(int topicId)
        {
            return await _postRepository.GetTopLevelPostsByTopicAsync(topicId);
        }

        public async Task<IEnumerable<Post>> GetPostsByUserAsync(int userId)
        {
            return await _postRepository.GetPostsByUserAsync(userId);
        }

        public async Task<Post> CreatePostAsync(int topicId, int userId, string content, string? imageUrl, int? parentPostId = null)
        {
            // Filtrera svordomar
            var filteredContent = FilterProfanity(content);

            var post = new Post
            {
                TopicId = topicId,
                UserId = userId,
                Content = filteredContent,
                ImageUrl = imageUrl,
                ParentPostId = parentPostId,
                CreatedAt = DateTime.UtcNow
            };

            await _postRepository.AddAsync(post);
            return post;
        }

        public async Task<bool> UpdatePostAsync(int postId, int userId, string content)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null || post.UserId != userId)
                return false;

            post.Content = FilterProfanity(content);
            post.UpdatedAt = DateTime.UtcNow;

            await _postRepository.UpdateAsync(post);
            return true;
        }

        public async Task<bool> DeletePostAsync(int postId, int userId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null || post.UserId != userId)
                return false;

            post.IsDeleted = true;
            await _postRepository.UpdateAsync(post);
            return true;
        }

        public string FilterProfanity(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return content;

            var filtered = content;
            foreach (var word in ProfanityWords)
            {
                var pattern = $@"\b{Regex.Escape(word)}\b";
                filtered = Regex.Replace(filtered, pattern, new string('*', word.Length), RegexOptions.IgnoreCase);
            }

            return filtered;
        }
    }
}