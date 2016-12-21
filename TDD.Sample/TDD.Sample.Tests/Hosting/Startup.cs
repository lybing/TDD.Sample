﻿using Autofac;
using Autofac.Integration.WebApi;
using Moq;
using NUnit.Framework;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using TDD.Sample.API.Core;
using TDD.Sample.API.Core.Controllers;
using TDD.Sample.API.Core.Filters;
using TDD.Sample.API.Core.HeaderAppenderHandler;
using TDD.Sample.API.Core.MessageHandlers;
using TDD.Sample.Data;
using TDD.Sample.Domain;
using TDD.Sample.Services;

namespace TDD.Sample.Tests.Hosting
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.MessageHandlers.Add(new HeaderAppenderHandler());
            config.MessageHandlers.Add(new EndRequestHandler());
            config.Filters.Add(new ArticlesReversedFilter());
            config.Services.Replace(typeof(IAssembliesResolver), new CustomAssembliesResolver());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.MapHttpAttributeRoutes();

            // Autofac configuration
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(typeof(ArticlesController).Assembly);

            // Unit of Work
            var _unitOfWork = new Mock<IUnitOfWork>();
            builder.RegisterInstance(_unitOfWork.Object).As<IUnitOfWork>();

            //Repositories
            var _articlesRepository = new Mock<IArticleRepository>();
            _articlesRepository.Setup(x => x.GetAll()).Returns(
                    BloggerInitializer.GetAllArticles()
                );
            builder.RegisterInstance(_articlesRepository.Object).As<IArticleRepository>();

            var _blogsRepository = new Mock<IBlogRepository>();
            _blogsRepository.Setup(x => x.GetAll()).Returns(
                BloggerInitializer.GetBlogs
                );
            builder.RegisterInstance(_blogsRepository.Object).As<IBlogRepository>();

            // Services
            builder.RegisterAssemblyTypes(typeof(ArticleService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterInstance(new ArticleService(_articlesRepository.Object, _unitOfWork.Object));
            builder.RegisterInstance(new BlogService(_blogsRepository.Object, _unitOfWork.Object));

            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            appBuilder.UseWebApi(config);
        }
    }
}
