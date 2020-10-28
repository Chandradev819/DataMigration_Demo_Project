using Data_Migration_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Data_Migration_App.Controllers
{
    public class ProductController : Controller
    {
        // GET: ProductController
        public ActionResult Index()
        {
            return View();
        }

       

        //  GET: ProductController/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult CreateAsync(IFormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Root product = new Root();
                    Item item = new Item();
                    Image objImg = new Image();
                    
                    CustomAttributes custAtt = new CustomAttributes();
                    List<Image> listImg = new List<Image>();
                    List<Attributes> listAttrbute = new List<Attributes>();
                    item.barcode = collection["items.barcode"];
                    item.title = collection["items.title"];
                    item.productMainId = collection["items.productMainId"];
                    item.brandId = Convert.ToInt32(collection["items.brandId"]);
                    item.categoryId = Convert.ToInt32(collection["items.categoryId"]);
                    item.quantity = Convert.ToInt32(collection["items.quantity"]);
                    item.stockCode = collection["items.stockCode"];
                    item.dimensionalWeight = Convert.ToInt32(collection["items.dimensionalWeight"]);
                    item.description = collection["items.description"];
                    item.currencyType = collection["items.currencyType"];
                    item.listPrice = Convert.ToDouble(collection["items.listPrice"]);
                    item.salePrice = Convert.ToDouble(collection["items.salePrice"]);
                    item.vatRate = Convert.ToInt32(collection["items.vatRate"]);
                    item.cargoCompanyId = Convert.ToInt32(collection["items.cargoCompanyId"]);
                    item.shipmentAddressId = Convert.ToInt32(collection["items.shipmentAddressId"]);
                    item.returningAddressId = Convert.ToInt32(collection["items.returningAddressId"]);
                    
                    objImg.url = collection["imgs.url"].ToString();
                    listImg.Add(objImg);
                    item.images = listImg;

                    for (int i = 0; i < Convert.ToInt32(collection["attributes.attributeId"].Count) ; i++)
                    {
                        Attributes objAttribute = new Attributes();
                        objAttribute.attributeId = Convert.ToInt32(collection["attributes.attributeId"][i]);
                        objAttribute.attributeValueId = Convert.ToInt32(collection["attributes.attributeValueId"][i]);
                        listAttrbute.Add(objAttribute);
                    }
                    item.attributes = listAttrbute;
                    //Custom Attribute
                    custAtt.attributeId = Convert.ToInt32(collection["custAttribute.attributeId"][0]);
                    custAtt.customAttributeValue= collection["custAttribute.customAttributeValue"][0].ToString();
                    //item.attributes = custAtt;
                    List<Item> lstItem = new List<Item>();
                    lstItem.Add(item);
                    product.items = lstItem;

                    string productJson;
                    productJson = System.Text.Json.JsonSerializer.Serialize(product);
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string authInfo = Convert.ToBase64String(Encoding.Default.GetBytes("VsSY3KtZ6BijgOCfyws8:Q7FfKUSDb6SpHJw4ufxs"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);

                        StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                        HttpResponseMessage response =  client.PostAsync("https://api.trendyol.com/sapigw/suppliers/130188/v2/products", content).Result;
                       // ViewBag.Message = response.Content.ReadAsStringAsync().Result;
                        if (response.IsSuccessStatusCode)
                        {
                            ViewBag.Message = "Data has sent sucessfully to trendyol.com";
                        }
                        else
                        {
                            ViewBag.Message = "There is some problem, Please try again after some time";
                        }
                        
                    }

                        return View();
                }
                else
                {
                    StatusCodeResult res = this.StatusCode(400);
                    return res;
                }
            }
            catch (Exception ex)
            {
                return View();
            }

        }

    }
}
