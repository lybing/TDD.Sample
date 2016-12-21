﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDD.Sample.Data;
using TDD.Sample.Domain;

namespace TDD.Sample.Services
{
    // operations you want to expose
    public interface IArticleService
    {
        /// <summary>
        /// Get Articles by name if have name, else return all articles
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IEnumerable<Article> GetArticles(string name = null);
        /// <summary>
        /// Get article by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Article GetArticle(int id);
        /// <summary>
        /// Get Articles by name if have name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Article GetArticle(string name);
        /// <summary>
        /// Create Article
        /// </summary>
        /// <param name="article"></param>
        void CreateArticle(Article article);
        /// <summary>
        /// Update Article
        /// </summary>
        /// <param name="article"></param>
        void UpdateArticle(Article article);
        /// <summary>
        /// Delete Article
        /// </summary>
        /// <param name="article"></param>
        void DeleteArticle(Article article);
        /// <summary>
        /// Save Article of context
        /// </summary>
        void SaveArticle();
    }

    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository articlesRepository;
        private readonly IUnitOfWork unitOfWork;

        public ArticleService(IArticleRepository articlesRepository, IUnitOfWork unitOfWork)
        {
            this.articlesRepository = articlesRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IArticleService Members

        public IEnumerable<Article> GetArticles(string title = null)
        {
            if (string.IsNullOrEmpty(title))
                return articlesRepository.GetAll();
            else
                return articlesRepository.GetAll().Where(c => c.Title.ToLower().Contains(title.ToLower()));
        }

        public Article GetArticle(int id)
        {
            var article = articlesRepository.GetById(id);
            return article;
        }

        public Article GetArticle(string title)
        {
            var article = articlesRepository.GetArticleByTitle(title);
            return article;
        }

        public void CreateArticle(Article article)
        {
            articlesRepository.Add(article);
        }

        public void UpdateArticle(Article article)
        {
            articlesRepository.Update(article);
        }

        public void DeleteArticle(Article article)
        {
            articlesRepository.Delete(article);
        }

        public void SaveArticle()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
