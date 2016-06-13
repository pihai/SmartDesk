using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using SmartDesk.Shared.Queries;

namespace SmartDesk.BotApp {
  [BotAuthentication]
  public class MessagesController : ApiController {
    private const string DeviceId = "1";

    /// <summary>
    /// POST: api/Messages
    /// Receive a message from a user and reply to it
    /// </summary>
    public async Task<Message> Post([FromBody]Message message) {
      if (message.Type != "Message") return HandleSystemMessage(message);
      switch (message.Text) {
        case "my address?":
          return message.CreateReplyMessage($"{message.To.Address}, {message.To.Id}");
        case "how long i'm standing?":
          return message.CreateReplyMessage("not long enough!");
        case "today?":
          return message.CreateReplyMessage(await CreateDayInfo(DateTime.Now));
        default:
          return message.CreateReplyMessage("I don't understand you my friend.");
      }
    }

    private static CloudStorageAccount CreateStorageAccount() {
      return CloudStorageAccount.Parse(WebConfigurationManager.AppSettings["AzureStorage"]);
    }

    private async Task<string> CreateDayInfo(DateTime day) {
      var query = new DayRatioQuery(CreateStorageAccount());
      var result = await query.Query(DeviceId, day.Date);

      var additionalText = TimeSpan.FromSeconds(result.Sitting).TotalHours > 3
        ? "Create!"
        : "Try to stand more often!";

      return $"Today you were {FormatTime(result.Standing)} standing, {FormatTime(result.Sitting)} sitting and {FormatTime(result.Inactive)} inactive. {additionalText}";
    }

    private string FormatTime(double sec) {
      var t = TimeSpan.FromSeconds(sec);
      return $"{t.Hours} hours {t.Minutes} minutes";
    }

    private Message HandleSystemMessage(Message message) {
      if (message.Type == "Ping") {
        Message reply = message.CreateReplyMessage();
        reply.Type = "Ping";
        return reply;
      } else if (message.Type == "DeleteUserData") {
        // Implement user deletion here
        // If we handle user deletion, return a real message
      } else if (message.Type == "BotAddedToConversation") {
      } else if (message.Type == "BotRemovedFromConversation") {
      } else if (message.Type == "UserAddedToConversation") {
      } else if (message.Type == "UserRemovedFromConversation") {
      } else if (message.Type == "EndOfConversation") {
      }

      return null;
    }
  }
}