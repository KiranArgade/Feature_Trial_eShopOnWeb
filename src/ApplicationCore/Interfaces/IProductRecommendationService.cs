using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.eShopWeb.ApplicationCore.Entities;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;

public interface IProductRecommendationService
{
    Task<List<CatalogItem>> GetRecommendationAsync(int basketId);
}
