using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toutokaz.Domain.Models;
using Toutokaz.Data.Interfaces;
using Toutokaz.Data.Repositories;
using Ninject;
using System.Web.Routing;
using Toutokaz.WebUI.Security;


namespace Toutokaz.WebUI.IoC
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance(RequestContext requestContext,
        Type controllerType)
        {
            return controllerType == null
            ? null
            : (IController)ninjectKernel.Get(controllerType);
        }
        private void AddBindings()
        {
            // put additional bindings here
            ninjectKernel.Bind<IAnnoncesRepository>().To<AnnoncesRepository>();
            ninjectKernel.Bind<ISectionRepository>().To<SectionRepository>();
            ninjectKernel.Bind<ICategoryRepository>().To<CategoryRepository>();
            ninjectKernel.Bind<IContactRepository>().To<ContactRepository>();
            ninjectKernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
            ninjectKernel.Bind<IAccountRepository>().To<AccountRepository>();

           
        }
    }
}