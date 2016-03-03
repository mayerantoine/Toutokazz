using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Toutokaz.Data.Interfaces;
using Toutokaz.Data.Repositories;
using Toutokaz.Domain.Models;

using Toutokaz.WebUI.Models;

namespace Toutokaz.WebUI.Controllers
{
    public class HomeController : Controller
    {
        ISectionRepository sectionRepository;     
        IAnnoncesRepository annoncesRepository;
        ICommuneRepository communeRepository;
        ICategoryRepository categoryRepository;
        ISectionRepository secRepository;

        public HomeController(IAnnoncesRepository repository)
        {
            annoncesRepository = repository;
            sectionRepository = new SectionRepository();
            communeRepository = new CommuneRepository();
            categoryRepository = new CategoryRepository();
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            HomePageModel model = new HomePageModel();       
           
            // Get Latest Ads
           var newAds = annoncesRepository.GetNewAds();
            if(newAds == null)
                throw new HttpException(404, "Recommended Ads not found");     
        
           model.ImgNew= annoncesRepository.GetNewAds().ToList();
           model.sectionList = sectionRepository.GetAll().OrderBy(x=>x.section_order).ToList();
           /* ViewBag.id_commune = new SelectList(communeRepository.GetAll(), "id_commune", "commune");
            ViewBag.ad_user_type = null;
            ViewBag.id_category = this.populateCategoryBySection();*/
            return View(model);
        }

        public ActionResult Home()
        {
             HomePageModel model = new HomePageModel();

            // Get Latest Ads
            var newAds = annoncesRepository.GetNewAds();
            if (newAds == null)
                throw new HttpException(404, "Recommended Ads not found");

            model.ImgNew = annoncesRepository.GetNewAds().ToList();
            model.sectionList = sectionRepository.GetAll().OrderBy(x => x.section_order).ToList();
            /* ViewBag.id_commune = new SelectList(communeRepository.GetAll(), "id_commune", "commune");
             ViewBag.ad_user_type = null;
             ViewBag.id_category = this.populateCategoryBySection();*/
            return View(model);
        }

        public ActionResult maintenance()
        {
            return View();
        }
           
        [ChildActionOnly]
        private IDictionary<string, IEnumerable<SelectListItem>> populateCategoryBySection()
        {


            IEnumerable<tb_section> section = sectionRepository.GetAll();
            IEnumerable<tb_category> category = categoryRepository.GetAll();

            IDictionary<string, IEnumerable<SelectListItem>> categoryBySection = new Dictionary<string, IEnumerable<SelectListItem>>();
            foreach (var sec in section)
            {
                var catList = new List<SelectListItem>();
                foreach (var cat in category.Where(c => c.id_section == sec.id_section))
                {
                    catList.Add(new SelectListItem { Value = cat.id_category.ToString(), Text = cat.category_title });
                }
                categoryBySection.Add(sec.section_title, catList);
            }

            return categoryBySection;

        }

        private IDictionary<string, IEnumerable<SelectListItem>> populateCommunetByDepartement()
        {


            IEnumerable<tb_departement> dep = communeRepository.GetAllDepartement();
            IEnumerable<tb_commune> com = communeRepository.GetAll();

            IDictionary<string, IEnumerable<SelectListItem>> CommuneByDepartement = new Dictionary<string, IEnumerable<SelectListItem>>();
            foreach (var item in dep)
            {
                var comList = new List<SelectListItem>();
                foreach (var city in com.Where(c => c.id_departement == item.id_departement))
                {
                    comList.Add(new SelectListItem { Value = city.id_commune.ToString(), Text = city.commune });
                }
                CommuneByDepartement.Add(item.departement, comList.OrderBy(x => x.Text));
            }

            return CommuneByDepartement;

        }

        
        [ChildActionOnly]
        public ActionResult _NavbarCategoriesMenu()
        {
            IEnumerable<tb_section> menuList = sectionRepository.GetAll().OrderBy(x=>x.section_order).ToList();

            if (menuList != null)
            {
                var section = (IList<tb_section>)menuList;// should not be cast
                return PartialView("_NavbarCategoriesMenu", section);

            }
            else
            {
                return HttpNotFound("Can not load category menu");
            }
        }

        public ActionResult quisommesnous()
        {
         
            return View();
        }

        public ActionResult conditionsdutilisation()
        {
         
            return View();
        }

      
        public ActionResult faq()
        {
           return View();
        }
    
    }
}
