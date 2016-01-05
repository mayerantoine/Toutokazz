using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toutokaz.Data.Interfaces;
using Toutokaz.Data.Repositories;
using Toutokaz.Domain.Models;


namespace Toutokazz.WebAPI.Models
{

    public interface IModelFactory
    {
        SectionModel Create(tb_section section);
        AnnonceModel Create(tb_ads ad);
        CategoryModel Create(tb_category category);

    } 


    public class ModelFactory : IModelFactory
    {
        public SectionModel Create(tb_section section){
            List<CategoryModel>  category = new List<CategoryModel>();

            foreach(var cat in section.tb_category){
                 category.Add(new CategoryModel{
                    id_category = cat.id_category,
                    category_title = cat.category_title,
                    category_order = cat.category_order
                 });
            };

            return new SectionModel{
                        id_section = section.id_section,
                        section_title = section.section_title,
                        categories = category
                    };
        }

        public CategoryModel Create(tb_category category)
        {
           return new CategoryModel
            {
               id_category = category.id_category,
               category_title = category.category_title,
               category_order = category.category_order
            };
        }

        public AnnonceModel Create(tb_ads ad)
        {
            return new AnnonceModel
            {
                id_ad = ad.id_ad,
                ad_description = ad.ad_description,
                ad_title = ad.ad_title,
                image_filename = String.IsNullOrEmpty(ad.tb_ad_image.FirstOrDefault().image_filename) ? "default-img.jpg" : ad.tb_ad_image.FirstOrDefault().image_filename,
                devise = ad.tb_devise.description,
                price = ad.ad_price,
                category_title = ad.tb_category.category_title,
                commune_name = ad.tb_commune.commune,
                ad_status = ad.ad_status
            };
        }
    }
}