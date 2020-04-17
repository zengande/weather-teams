using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using YunStorm.Weather.Teams.Web.Services;

namespace YunStorm.Weather.Teams.Web.BackgroundTasks
{
    public class RealTimeWeatherForecastTask : BackgroundService
    {
        private readonly string _incomingWebhookUrl;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _http;
        private readonly ILogger<RealTimeWeatherForecastTask> _logger;
        public RealTimeWeatherForecastTask(IConfiguration configuration,
            ILogger<RealTimeWeatherForecastTask> logger)
        {
            _logger = logger;
            _configuration = configuration;

            _incomingWebhookUrl = configuration["IncomingWebhookUrl"];
            _http = new HttpClient();
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var card = await CreateWeatherCardAsync();
                var content = new StringContent(card);
                var response = await _http.PostAsync(_incomingWebhookUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("发送成功");
                }
                else
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning(msg);
                }

                await Task.Delay(TimeSpan.FromHours(2));
            }
        }

        private async Task<string> CreateWeatherCardAsync()
        {
            var http = new HttpClient
            {
                BaseAddress = new Uri("https://free-api.heweather.net/s6/weather/")
            };
            var client = new HeWeatherClient(http, _configuration);
            var weather = await client.NowAsync("西湖区");

            var weather6 = weather?.HeWeather6?.FirstOrDefault();
            if (weather6 != null)
            {

                return $"{{\"@context\": \"https://schema.org/extensions\",\"@type\": \"MessageCard\",\"themeColor\": \"0072C6\",\"title\": \"{weather6.basic.location},{weather6.basic.parent_city}\",\"text\": \"{weather6.now.cond_txt} {weather6.now.tmp}℃\"}}";
            }
            return string.Empty;
        }
    }
}
