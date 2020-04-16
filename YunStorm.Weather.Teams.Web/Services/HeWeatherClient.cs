using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace YunStorm.Weather.Teams.Web.Services
{
    public class HeWeatherClient
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        public HeWeatherClient(HttpClient http, IConfiguration configuration)
        {
            _apiKey = configuration["HeWeatherKey"];
            _http = http;
        }

        /// <summary>
        /// 实况天气
        /// </summary>
        /// <param name=""></param>
        public Task<NowWeather> NowAsync(string location)
        {
            return GetAsync<NowWeather>($"now?key={_apiKey}&location={location}");
        }

        private async Task<T> GetAsync<T>(string uri)
        {
            var response = await _http.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
            return default;
        }
    }


    public class Basic
    {
        /// <summary>
        /// 
        /// </summary>
        public string cid { get; set; }
        /// <summary>
        /// 杭州
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// 杭州
        /// </summary>
        public string parent_city { get; set; }
        /// <summary>
        /// 浙江
        /// </summary>
        public string admin_area { get; set; }
        /// <summary>
        /// 中国
        /// </summary>
        public string cnty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tz { get; set; }
    }

    public class Update
    {
        /// <summary>
        /// 
        /// </summary>
        public string loc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string utc { get; set; }
    }

    public class Now
    {
        /// <summary>
        /// 
        /// </summary>
        public string cloud { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cond_code { get; set; }
        /// <summary>
        /// 晴
        /// </summary>
        public string cond_txt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pcpn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pres { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tmp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vis { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wind_deg { get; set; }
        /// <summary>
        /// 西南风
        /// </summary>
        public string wind_dir { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wind_sc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wind_spd { get; set; }
    }

    public class HeWeather6Item
    {
        /// <summary>
        /// 
        /// </summary>
        public Basic basic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Update update { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Now now { get; set; }
    }

    public class NowWeather
    {
        /// <summary>
        /// 
        /// </summary>
        public List<HeWeather6Item> HeWeather6 { get; set; }
    }
}
