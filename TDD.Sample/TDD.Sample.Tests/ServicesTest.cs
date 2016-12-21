using System;
using NUnit.Framework;
using TDD.Sample.Services;
using TDD.Sample.Domain;
using System.Collections.Generic;
using TDD.Sample.Data;
using Moq;
using System.Linq;

namespace TDD.Sample.Tests
{
    [TestFixture]
    public class ServicesTest
    {
        #region Variables
        IArticleService _articleService;
        IArticleRepository _articleRepository;
        IUnitOfWork _unitOfWork;
        List<Article> _randomArticles;
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            _randomArticles = SetupArticles();

            _articleRepository = SetupArticleRepository();
            _unitOfWork = new Mock<IUnitOfWork>().Object;
            _articleService = new ArticleService(_articleRepository, _unitOfWork);
        }

        /// <summary>
        /// Setup Articles
        /// </summary>
        /// <returns></returns>
        public List<Article> SetupArticles()
        {
            int _counter = new int();
            List<Article> _articles = BloggerInitializer.GetAllArticles();

            foreach (Article _article in _articles)
                _article.ID = ++_counter;

            return _articles;
        }

        /// <summary>
        /// Emulate _articleRepository behavior
        /// </summary>
        /// <returns></returns>
        public IArticleRepository SetupArticleRepository()
        {
            // Init repository
            var repo = new Mock<IArticleRepository>();

            // Get all articles
            repo.Setup(r => r.GetAll()).Returns(_randomArticles);

            // Get by ID
            repo.Setup(r => r.GetById(It.IsAny<int>()))
                .Returns(new Func<int, Article>(
                    id => _randomArticles.Find(a => a.ID.Equals(id))));

            // Add
            repo.Setup(r => r.Add(It.IsAny<Article>()))
                .Callback(new Action<Article>(newArticle =>
                {
                    dynamic maxArticleID = _randomArticles.Last().ID;
                    newArticle.ID = maxArticleID + 1;
                    newArticle.DateCreated = DateTime.Now;

                    _randomArticles.Add(newArticle);
                }));

            // Update
            repo.Setup(r => r.Update(It.IsAny<Article>()))
                .Callback(new Action<Article>(x =>
                {
                    var oldArticle = _randomArticles.Find(a => a.ID == x.ID);
                    oldArticle.DateEdited = DateTime.Now;
                    oldArticle = x;
                }));

            // Delete
            repo.Setup(r => r.Delete(It.IsAny<Article>()))
                .Callback(new Action<Article>(x =>
                {
                    var _articleToRemove = _randomArticles.Find(a => a.ID == x.ID);

                    if (_articleToRemove != null)
                        _randomArticles.Remove(_articleToRemove);
                }));

            // Return mock implementation
            return repo.Object;
        }

        #endregion

        #region Test Method

        [Test]
        public void ServiceShouldReturnAllArticles()
        {
            var articles = _articleService.GetArticles();

            NUnit.Framework.Assert.That(articles, Is.EqualTo(_randomArticles));
        }

        [Test]
        public void ServiceShouldAddNewArticle()
        {
            var _newArticle = new Article()
            {
                Author = "Chris Sakellarios",
                Contents = "If you are an ASP.NET MVC developer, you will certainly..",
                Title = "URL Rooting in ASP.NET (Web Forms)",
                URL = "https://chsakell.com/2013/12/15/url-rooting-in-asp-net-web-forms/"
            };

            int _maxArticleIDBeforeAdd = _randomArticles.Max(a => a.ID);
            _articleService.CreateArticle(_newArticle);

            NUnit.Framework.Assert.That(_newArticle, Is.EqualTo(_randomArticles.Last()));
            NUnit.Framework.Assert.That(_maxArticleIDBeforeAdd + 1, Is.EqualTo(_randomArticles.Last().ID));
        }

        [Test]
        public void ServiceShouldUpdateArticle()
        {
            var _firstArticle = _randomArticles.First();

            _firstArticle.Title = "OData feat. ASP.NET Web API"; // reversed<img draggable="false" class="emoji" alt="" src="https://s.w.org/images/core/emoji/2/svg/1f642.svg">
            _firstArticle.URL = "http://t.co/fuIbNoc7Zh"; // short link
            _articleService.UpdateArticle(_firstArticle);

            NUnit.Framework.Assert.That(_firstArticle.DateEdited, Is.Not.EqualTo(DateTime.MinValue));
            NUnit.Framework.Assert.That(_firstArticle.URL, Is.EqualTo("http://t.co/fuIbNoc7Zh"));
            NUnit.Framework.Assert.That(_firstArticle.ID, Is.EqualTo(1)); // hasn't changed
        }

        [Test]
        public void ServiceShouldDeleteArticle()
        {
            int maxID = _randomArticles.Max(a => a.ID); // Before removal
            var _lastArticle = _randomArticles.Last();

            // Remove last article
            _articleService.DeleteArticle(_lastArticle);

            NUnit.Framework.Assert.That(maxID, Is.GreaterThan(_randomArticles.Max(a => a.ID))); // Max reduced by 1
        } 

        #endregion
    }
}
