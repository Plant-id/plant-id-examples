using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;

namespace PlantIdApi.Controllers
{
    [Route("api/plants")]
    [ApiController]
    public class PlantsController : ControllerBase
    {
        private readonly ILogger<PlantsController> _logger;
        private readonly IConfiguration _config;

        public PlantsController(ILogger<PlantsController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpPost("identify")]
        public async Task<string> Identify(IFormFile files)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_config.GetValue<string>("PlantIdUrl"));
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Api-Key", _config.GetValue<string>("PlantIdApiKey"));

                    string base64File = String.Empty;
                    using (var ms = new MemoryStream())
                    {
                        files.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        base64File = Convert.ToBase64String(fileBytes);
                    }

                    var jsonBodyObj = (dynamic)new JObject();
                    jsonBodyObj.images = new JArray(base64File);
                    jsonBodyObj.modifiers = new JArray("crops_fast", "similar_images");
                    jsonBodyObj.plant_language = "en";
                    jsonBodyObj.plant_details = new JArray("common_names",
                      "url",
                      "name_authority",
                      "wiki_description",
                      "taxonomy",
                      "synonyms");

                    var content = new StringContent(jsonBodyObj.ToString(), Encoding.UTF8, "application/json");
                    var result = await client.PostAsync("identify", content);
                    var resultContent = await result.Content.ReadAsStringAsync();

                    return resultContent;
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}