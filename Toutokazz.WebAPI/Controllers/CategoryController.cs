using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Toutokaz.Data.Interfaces;
using Toutokaz.Data.Repositories;
using Toutokaz.Domain.Models;
using Toutokazz.WebAPI.Models;

namespace Toutokazz.WebAPI.Controllers
{
    public class CategoryController : ApiController
    {
        ISectionRepository _secRepository;
        IAnnoncesRepository _annonceRepository;
        private IModelFactory _modelFactory;

        public CategoryController()
        {

            _secRepository = new SectionRepository();
            _modelFactory = new ModelFactory();
            _annonceRepository = new AnnoncesRepository();

        }
  

        public IHttpActionResult Get()
        {
            var menuList = _secRepository.GetAll();
            var models = menuList.Select(_modelFactory.Create);
            return Ok(models);

        }

        public IHttpActionResult Get(int id)
        {
            var categoryList = _secRepository.GetCategoryBySectionId(id);
            var models = categoryList.Select(_modelFactory.Create);
            return Ok(models);

        }
    }
}
