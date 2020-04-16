using AdaptiveCards;
using AdaptiveCards.Templating;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YunStorm.Weather.Teams.Web.Services;

namespace YunStorm.Weather.Teams.Web.Bots
{
    public class TeamsBot : TeamsActivityHandler
    {
        private readonly HeWeatherClient _weatherClient;
        public TeamsBot(HeWeatherClient weatherClient)
        {
            _weatherClient = weatherClient;
        }

        protected override async Task<MessagingExtensionResponse> OnTeamsMessagingExtensionQueryAsync(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionQuery query, CancellationToken cancellationToken)
        {
            List<MessagingExtensionAttachment> attachments = null;
            var cityName = query?.Parameters?.FirstOrDefault(p => p.Name == "CityName")?.Value as string ?? string.Empty;
            if (string.IsNullOrWhiteSpace(cityName))
            {
                attachments = new List<MessagingExtensionAttachment>();
            }
            else
            {
                var weather = await _weatherClient.NowAsync(cityName);

                attachments = weather?.HeWeather6?.Where(w => w.status == "ok" && w.now != null)
                    .Select(item => new MessagingExtensionAttachment
                    {
                        ContentType = AdaptiveCard.ContentType,
                        Content = CreateWeatherCard(item),
                        Preview = new ThumbnailCard
                        {
                            Title = $"{item.basic.parent_city}-{item.basic.location}",
                            Text = $"{item.now.cond_txt} {item.now.tmp}",
                            Images = new List<CardImage>() { new CardImage($"https://cdn.heweather.com/cond_icon/{item.now.cond_code}.png", "Icon") }
                        }.ToAttachment()
                    }).ToList();
            }



            return new MessagingExtensionResponse
            {
                ComposeExtension = new MessagingExtensionResult
                {
                    Type = "result",
                    AttachmentLayout = "list",
                    Attachments = attachments
                }
            };
        }

        private AdaptiveCard CreateWeatherCard(HeWeather6Item weather)
        {
            var templateJson = "{\r\n  \"$schema\": \"http://adaptivecards.io/schemas/adaptive-card.json\",\r\n  \"type\": \"AdaptiveCard\",\r\n  \"version\": \"1.0\",\r\n  \"body\": [\r\n    {\r\n      \"type\": \"TextBlock\",\r\n      \"text\": \"{name}\",\r\n      \"size\": \"Large\",\r\n      \"isSubtle\": true\r\n    },\r\n    {\r\n      \"type\": \"ColumnSet\",\r\n      \"columns\": [\r\n        {\r\n          \"type\": \"Column\",\r\n          \"width\": \"auto\",\r\n          \"items\": [\r\n            {\r\n              \"type\": \"Image\",\r\n              \"url\": \"{iconUrl}\",\r\n              \"size\": \"Small\"\r\n            }\r\n          ]\r\n        },\r\n        {\r\n          \"type\": \"Column\",\r\n          \"width\": \"auto\",\r\n          \"items\": [\r\n            {\r\n              \"type\": \"TextBlock\",\r\n              \"text\": \"{tmp}\",\r\n              \"size\": \"ExtraLarge\",\r\n              \"spacing\": \"None\"\r\n            }\r\n          ]\r\n        },\r\n        {\r\n          \"type\": \"Column\",\r\n          \"width\": \"stretch\",\r\n          \"items\": [\r\n            {\r\n              \"type\": \"TextBlock\",\r\n              \"text\": \"℃\",\r\n              \"weight\": \"Bolder\",\r\n              \"spacing\": \"Small\"\r\n            }\r\n          ]\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";
            var dataJson = JsonConvert.SerializeObject(new
            {
                name= $"{weather.basic.location},{weather.basic.parent_city}",
                iconUrl = $"https://cdn.heweather.com/cond_icon/{weather.now.cond_code}.png",
                weather.now.tmp
            });
            var transformer = new AdaptiveTransformer();
            var cardJson = transformer.Transform(templateJson, dataJson);

            var result = AdaptiveCard.FromJson(cardJson);

            return result.Card;
        }
    }
}
