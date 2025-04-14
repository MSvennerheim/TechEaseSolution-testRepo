using System.Data;
using System.Text.Json;
using Npgsql;
using server.Properties;

namespace server.TestServices;

public class TestService : IDisposable
{
   private readonly Queries queries;
   private NpgsqlDataSource _dataSource;
   private NpgsqlConnection? _dbConnection;

   public TestService()
   {
      Database database = new();
      _dataSource = database.Connection(); // Assuming this returns NpgsqlDataSource
      _dbConnection = _dataSource.CreateConnection(); // Create a connection from the data source
      queries = new(_dataSource);
   }

   public async Task<string?> GetChatIdForCustomerLogin(string company, string message, string email)
   {
      string jsonResponse = await queries.GetChatsForCsRep(company, true, false);

      using (JsonDocument messages = JsonDocument.Parse(jsonResponse))
      {
         var root = messages.RootElement;

         foreach (var chat in root.EnumerateArray())
         {
            if (chat.GetProperty("email").GetString() == email && chat.GetProperty("message").GetString() == message)
            {
               return chat.GetProperty("chatid").ToString();
            }
         }
      }
      return ""; // if nothing is found. Shouldn't happen but who knows
   }

   public async Task MakeTicketForTest(string email, string issue, string text)
   {
      Ticket ticketInformation = new();
      ticketInformation.email = email;
      ticketInformation.option = issue;
      ticketInformation.description = text;
      
      await queries.CompanyName(ticketInformation);
      await queries.customerTempUser(ticketInformation);
      await queries.postNewTicket(ticketInformation);
   }
   
   public void Dispose()
   {
      _dbConnection?.Dispose();
   }
}