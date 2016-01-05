using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toutokaz.Data.Interfaces;
using Toutokaz.Domain.Models;


namespace Toutokaz.Data.Repositories
{
    public class AnnoncesRepository : EntityRepository<tb_ads>, IAnnoncesRepository
    {
        public IEnumerable<tb_ads> GetAllAds()
        {
            var query = from c in _db.tb_ads.Include("tb_ad_image")
                        orderby c.ad_date_created descending
                        select c;

            return query.ToList();
                                              
        }

        public IEnumerable<tb_ads> GetAllActiveAds()
        {
            var query = from c in _db.tb_ads.Include("tb_ad_image")
                        where c.ad_status == 1 || c.ad_status == 4
                        orderby c.ad_date_created descending
                        select c;

            return query.ToList();

        }

        public IEnumerable<tb_ads> GetRecommendedAds()
        {
            var query = from c in _db.tb_ads.Include("tb_ad_image")
                        where c.ad_status == 1 
                        orderby c.ad_date_created descending
                        select c;
            return query.Take(8).ToList();
        }

        public IEnumerable<tb_ads> GetNewAds()
        {
            var query = _db.tb_ads.Include("tb_ad_image")
                        .Where(x=>x.ad_status == 1 || x.ad_status ==4)
                        .OrderByDescending(x => x.ad_date_created);
                        
            return query.Take(8).ToList();
        }

        public IEnumerable<tb_ads> GetPremiumAds()
        {
            var query = from c in _db.tb_ads.Include("tb_ad_image")
                        select c;
            return query.Take(6).ToList();
        }

        public IEnumerable<tb_ads> GetAdsByType(string typeannonce)
        {
            var type = 1;
            if (typeannonce.Equals("avendre"))
            {
                type = 1;
            }
            else if (typeannonce.Equals("acheter"))
            {
                type = 2;
            }
            var result = from c in _db.tb_ads.Include("tb_ad_image")
                         where c.ad_type == type
                         orderby c.ad_date_created descending
                         select c;

            return result.ToList();
        }

       public  IEnumerable<tb_ads> GetAdsByCategory(int id_category)
        {
            var result = from c in _db.tb_ads.Include("tb_ad_image")
                         where c.id_category == id_category
                         orderby c.ad_date_created descending
                         select c;
            return result.ToList();
        }
     
        public IEnumerable<tb_ads> GetAdsBySection(int id_section)
       {
           var result = from c in _db.tb_ads.Include("tb_ad_image")
                        where c.id_section == id_section
                        orderby c.ad_date_created descending
                        select c;
           return result.ToList();
       }

        public IEnumerable<tb_ads> SearchAds(string typeannonce, String keywords, int? id_category, int? id_section, int? ad_city, int? id_account_type, string sortOrder)
        {


            var result = from c in _db.tb_ads.Include("tb_ad_image")
                         where c.ad_status == 1 || c.ad_status == 4
                          select c;

            if (!String.IsNullOrEmpty(keywords) || id_category != null || id_section !=null || ad_city != null || id_account_type!=null)
            {
                 if (!String.IsNullOrEmpty(typeannonce)) {
                        var type = 1;
                        if (typeannonce.Equals("avendre"))
                        {
                            type = 1;   
                        }
                        else if (typeannonce.Equals("acheter"))
                        {
                            type = 2;
                        }
                       result = result.Where(s => s.ad_type == type);
                 }

                if (!String.IsNullOrEmpty(keywords))
                {
                    result = result.Where(s => s.ad_description.Contains(keywords) || s.ad_title.Contains(keywords));
                }

                if (id_category != null)
                {
                    result = result.Where(s => s.id_category == id_category);
                }

                if (id_section != null)
                {
                    result = result.Where(s => s.id_section == id_section);
                }

                if (ad_city != null)
                {
                    result = result.Where(s => s.id_commune == ad_city);
                }

                if(id_account_type!=null){

                    result = result.Where(s => s.tb_account.id_account_type == id_account_type);
                }

            }

            switch (sortOrder)
            {
                case "Prix desc":
                    result = result.OrderByDescending(s => s.ad_price);
                    break;
                case "Prix":
                    result = result.OrderBy(s => s.ad_price);
                    break;
                case "Date asc":
                    result = result.OrderBy(s => s.ad_date_created);
                    break;
                default:
                    result = result.OrderByDescending(s => s.ad_date_created);
                    break;
            }

            return result.ToList();
        }

        public tb_ads GetAd(int id)
        {
            tb_ads ad = _db.tb_ads.Include("tb_image")
                                   .Where(x => x.id_ad == id)
                                   .FirstOrDefault();
            return ad;
                                
        }

        public IEnumerable<tb_ads> GetAdsByUser(Guid userkey)
        {
     
            var result = from c in _db.tb_ads.Include("tb_ad_image")
                         where c.id_user == userkey
                         select c;
            return result.ToList();
        }

        public IEnumerable<tb_ads> GetAdsByUserId(int id)
        {
            var result = from c in _db.tb_ads.Include("tb_ad_image")
                         where c.UserId == id
                         select c;
            return result.ToList();
        }

        public IEnumerable<tb_ad_status> GetAllAdsStatus()
        {
            var query = from c in _db.tb_ad_status
                         select c;

            return query.ToList();
        }

        public tb_ad_status GetAdsStatus(int id)
        {
            var query = (from c in _db.tb_ad_status
                        where c.id_ad_status  == id
                        select c).SingleOrDefault();

            return query;
        }

        public Boolean DeleteAdsImages(tb_ads ad)
        {
            if (ad != null)
            {
                try
                {
                    foreach (var item in ad.tb_ad_image)
                    {
                        _db.tb_ad_image.Remove(item);
                
                    }

                    _db.SaveChanges();
                    return true;
                }
                catch (Exception exp)
                {
                    return false;
                }
            }
            return false;
                         
        }

        public tb_account GetAnnonceurById(int ad_id)
        {
            var ad = _db.tb_ads.Where(x=>x.id_ad == ad_id)
                                .Select(x=>(int)x.id_account).SingleOrDefault();
            if (ad == null)
            {
                return null;
            }

            var account = _db.tb_account.Where(x => x.id_account == ad).SingleOrDefault();

            return account;
        }

        public IEnumerable<tb_ads> GetAdsByAnnonceur(int id) 
        {
            var query = _db.tb_ads.Where(x => x.id_account == id);

            return query.ToList();
        }

        public IEnumerable<tb_ads> GetSimilarAds(int id)
        {
           int category =(int)  _db.tb_ads.Where(x => x.id_ad == id).Select(c => c.id_category).SingleOrDefault();

           var query = _db.tb_ads.Where(x => x.id_category == category && x.ad_status == 1);

           return query.OrderBy(x => Guid.NewGuid()).Take(6).ToList();
           
        }
    
        public IEnumerable<tb_ad_reference> GetRefItems(string ref_item){

            var item = _db.tb_ad_reference.Where(x => x.ref_name.Equals(ref_item));          
            return item.ToList();

        }
    
    }
}
