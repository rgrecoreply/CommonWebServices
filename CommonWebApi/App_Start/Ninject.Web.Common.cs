using System;
using System.Web;
using System.Web.Http;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using Ninject.Web.WebApi;
using PersistenceContext.Implementations;
using PersistenceContext.Interfaces;
using ServiceDecorator.EntityDecorators;
using Services.EntityInterfaces;
using Services.EntityServices;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(CommonWebApi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(CommonWebApi.App_Start.NinjectWebCommon), "Stop")]

namespace CommonWebApi.App_Start
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();
        private static readonly string CRM_CREDENTIAL = "crmCredential";

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            RegisterContext(kernel);
            RegisterEntityService(kernel);
        }

        private static void RegisterContext(IKernel kernel)
        {
            // IMPLEMENTATION OF CONTEXT FOR HTTPCLIENT
            //string clientId = ConfigHelper.GetConfigSectionValue<string>(CRM_CREDENTIAL, "ApplicationId");
            //string clientSecret = ConfigHelper.GetConfigSectionValue<string>(CRM_CREDENTIAL, "AppSecret");
            //string authorityUri = ConfigHelper.GetConfigSectionValue<string>(CRM_CREDENTIAL, "Authority");
            //string crmId = ConfigHelper.GetConfigSectionValue<string>(CRM_CREDENTIAL, "CrmId");
            //string dynamicsUrl = ConfigHelper.GetConfigSectionValue<string>(CRM_CREDENTIAL, "Url");

            //kernel.Bind<IPersistenceContext>()
            //    .ToConstructor(c => new HttpClientFactory(
            //        dynamicsUrl, clientId, clientSecret, authorityUri, crmId))
            //    .InSingletonScope();

            // IMPLEMENTATION OF CONTEXT FOR IORGANIZATIONSERVICE

            kernel.Bind<IPersistenceContext>()
                .ToConstructor(x => new CrmContext())
                .InSingletonScope();
        }

        private static void RegisterEntityService(IKernel kernel)
        {
            IPersistenceContext context = kernel.Get<IPersistenceContext>();

            kernel.Bind<IAppointmentService>()
                .ToConstructor(x => new AppointmentServiceDecorator(new AppointmentService(context)))
                .InSingletonScope();

        }
    }
}