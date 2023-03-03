using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
namespace Microsoft.eShopWeb.ApplicationCore.Services;
public class ProductRecommendationService : IProductRecommendationService
{    
    private readonly IRepository<Basket> _basketRepository;
    private readonly IRepository<CatalogItem> _itemRepository;
    private readonly IAppLogger<BasketService> _logger;
	
    public ProductRecommendationService(
		IRepository<Basket> basketRepository,
        IAppLogger<BasketService> logger,
        IRepository<CatalogItem> itemRepository)
    {
        _basketRepository = basketRepository;
        _logger = logger;
        _itemRepository = itemRepository;
    }

    public async Task<List<CatalogItem>> GetRecommendationAsync(int basketId)
    {
        var basketSpec = new BasketWithItemsSpecification(basketId);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);
        List<CatalogItem> recommendedItems = new List<CatalogItem>();
        var catalogItems = await _itemRepository.ListAsync();     

        //for each basket item
        foreach(var basketItem in basket.Items){
            var catalogBasketItem = (dynamic)null;
            foreach(var catItem in catalogItems){
                if(catItem.Id == basketItem.CatalogItemId){
                    catalogBasketItem = catItem;
                    break;
                }
            }

            //Input : Find matching words from two strings in c sharp language
            if(catalogBasketItem != null){
                //for each catalog item
                foreach(var catItem in catalogItems){
                    if(catItem.Id != catalogBasketItem.Id){                           
                        string str1 = catItem.Description;
                        string str2 = catalogBasketItem.Description;
                        string[] words1  = str1.Split(' ');
                        string[] words2  = str2.Split(' ');
                        // Find matching words using LINQ
                        IEnumerable<string> matchingWords = words1.Intersect(words2);
                        if(matchingWords.Count() >= 1)
                        {
                            recommendedItems.Add(catItem);
                        }
                    }
                }
            }
        } 

        //removed recommonded items which are already in basket
        foreach(var basketItem in basket.Items)
        {        
            for (int i = recommendedItems.Count - 1; i >= 0; i--)
            {
                if (recommendedItems[i].Id == basketItem.CatalogItemId){
                    recommendedItems.RemoveAt(i);
                    break;
                }
            }
        }

        return recommendedItems;
    }
}