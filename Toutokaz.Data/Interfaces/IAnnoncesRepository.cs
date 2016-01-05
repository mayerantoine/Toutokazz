using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toutokaz.Domain.Models;

namespace Toutokaz.Data.Interfaces
{
    public interface IAnnoncesRepository :IEntityRepository<tb_ads>
    {
        IEnumerable<tb_ads> GetAllAds();
        IEnumerable<tb_ads> GetAllActiveAds();
        IEnumerable<tb_ads> GetAdsByType(string typeannonce); // sell or buy
        IEnumerable<tb_ads> GetAdsByCategory(int id_category);
        IEnumerable<tb_ads> GetAdsBySection(int id_section); 
        IEnumerable<tb_ads> GetRecommendedAds();
        IEnumerable<tb_ads> GetNewAds();
        IEnumerable<tb_ads> GetPremiumAds();
        IEnumerable<tb_ads> GetAdsByAnnonceur(int id);
        IEnumerable<tb_ads> GetSimilarAds(int id);
        IEnumerable<tb_ads> SearchAds(string typeannonce, String keywords, int? id_category, int? id_section, int? ad_city, int? id_account_type,string sortOrder);
        IEnumerable<tb_ads> GetAdsByUser(Guid userkey);
        IEnumerable<tb_ads> GetAdsByUserId(int id);
        IEnumerable<tb_ad_status> GetAllAdsStatus();
        tb_ad_status GetAdsStatus(int ad_id);
        tb_ads GetAd(int id);
        Boolean DeleteAdsImages(tb_ads ad);
        tb_account GetAnnonceurById(int ad_id);
        IEnumerable<tb_ad_reference> GetRefItems(string ref_item);

    }
}
