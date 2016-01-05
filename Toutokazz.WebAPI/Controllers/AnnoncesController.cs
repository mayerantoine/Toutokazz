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
    public class AnnoncesController : ApiController
    {
        ISectionRepository _secRepository;
        IAnnoncesRepository _annonceRepository;
        private IModelFactory _modelFactory;

        public AnnoncesController()
        {

            _secRepository = new SectionRepository();
            _modelFactory = new ModelFactory();
            _annonceRepository = new AnnoncesRepository();
     
        }


       public IHttpActionResult Get()
        {

            var allads = _annonceRepository.GetAllActiveAds();
            var model = allads.Select(_modelFactory.Create);

            return Ok(model);
       
        }

        [HttpGet]
        public IHttpActionResult details(int id)
        {
            try {
                var ads = _annonceRepository.GetById(id);
                var model = _modelFactory.Create(ads);

                return Ok(model);
            }
            catch (Exception ex)
            {
                //Logging
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        public IHttpActionResult category(int id)
        {
            try
            {
                var ads = _annonceRepository.GetAdsByCategory(id);
                var model = ads.Select(_modelFactory.Create);

                return Ok(model);
            }
            catch (Exception ex)
            {
                //Logging
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        public IHttpActionResult section(int id)
        {
            try
            {
                var ads = _annonceRepository.GetAdsBySection(id);
                var model = ads.Select(_modelFactory.Create);

                return Ok(model);
            }
            catch (Exception ex)
            {
                //Logging
                return InternalServerError(ex);
            }

        }
    }
}
