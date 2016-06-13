using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;

namespace SmartDesk.BotApp {
  public class NotificationController : ApiController {
    public async Task<HttpResponseMessage> Get(string mail, string msg, string pin = null) {
      if(string.IsNullOrEmpty(pin) || pin != WebConfigurationManager.AppSettings["Pin"])
        return Request.CreateErrorResponse(HttpStatusCode.Forbidden, $"Pin was not correct. ({pin}, {WebConfigurationManager.AppSettings["Pin"]})");

      var connector = new ConnectorClient(WebConfigurationManager.AppSettings["AppId"], WebConfigurationManager.AppSettings["AppSecret"]);
      var message = new Message {
        To = new ChannelAccount {
          ChannelId = "skype",
          Address = $"8:live:{mail}",
          IsBot = false
        },
        From = new ChannelAccount {
          Name = "hptestbot",
          ChannelId = "skype",
          Address = "758a4cb4-4d9d-4b92-9509-23df26971f2b",
          Id = "hptestbot",
          IsBot = true
        },
        Text = msg,
        Language = "en"
      };

      try {
        connector.Messages.SendMessage(message);
        return Request.CreateResponse(HttpStatusCode.OK);
      }
      catch (Exception e) {
        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
      }
    }
  }
}