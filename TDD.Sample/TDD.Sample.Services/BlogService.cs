using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.Sample.Data;
using TDD.Sample.Domain;

namespace TDD.Sample.Services
{
    // operations you want to expose
    public interface IBlogService
    {
        /// <summary>
        /// Get blogs by name if have elsewise return all blogs
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IEnumerable<Blog> GetBlogs(string name = null);
        /// <summary>
        /// Get blog by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Blog GetBlog(int id);
        /// <summary>
        /// Get Blog by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Blog GetBlog(string name);
        /// <summary>
        /// Create Blog
        /// </summary>
        /// <param name="blog"></param>
        void CreateBlog(Blog blog);
        /// <summary>
        /// Update Blog
        /// </summary>
        /// <param name="blog"></param>
        void UpdateBlog(Blog blog);
        /// <summary>
        /// Delete Blog
        /// </summary>
        /// <param name="blog"></param>
        void DeleteBlog(Blog blog);
        /// <summary>
        /// Save Article of context
        /// </summary>
        void SaveBlog();
    }

    public class BlogService : IBlogService
    {
        private readonly IBlogRepository blogsRepository;
        private readonly IUnitOfWork unitOfWork;

        public BlogService(IBlogRepository blogsRepository, IUnitOfWork unitOfWork)
        {
            this.blogsRepository = blogsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IBlogService Members

        public IEnumerable<Blog> GetBlogs(string name = null)
        {
            if (string.IsNullOrEmpty(name))
                return blogsRepository.GetAll();
            else
                return blogsRepository.GetAll().Where(c => c.Name == name);
        }

        public Blog GetBlog(int id)
        {
            var blog = blogsRepository.GetById(id);
            return blog;
        }

        public Blog GetBlog(string name)
        {
            var blog = blogsRepository.GetBlogByName(name);
            return blog;
        }

        public void CreateBlog(Blog blog)
        {
            blogsRepository.Add(blog);
        }

        public void UpdateBlog(Blog blog)
        {
            blogsRepository.Update(blog);
        }

        public void DeleteBlog(Blog blog)
        {
            blogsRepository.Delete(blog);
        }

        public void SaveBlog()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
