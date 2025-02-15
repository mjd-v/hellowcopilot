using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
    static readonly string ApplicationName = "Google Sheets API .NET Quickstart";
    static readonly string SpreadsheetId = "your-spreadsheet-id";
    static readonly string SheetName = "Sheet1";
    static readonly string OpenAiApiKey = "your-openai-api-key";

    static async Task Main(string[] args)
    {
        UserCredential credential;

        using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        {
            string credPath = "token.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
        }

        var service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        var range = $"{SheetName}!A1:B10";
        SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(SpreadsheetId, range);

        ValueRange response = request.Execute();
        IList<IList<object>> values = response.Values;

        if (values != null && values.Count > 0)
        {
            for (int i = 0; i < values.Count && i < 10; i++)
            {
                var row = values[i];
                if (row.Count > 0 && !string.IsNullOrEmpty(row[0].ToString()))
                {
                    string prompt = "tell me about " + row[0].ToString();
                    string openAiResponse = await CallOpenAiApi(prompt);
                    row.Add(openAiResponse);
                    Console.WriteLine($"Processed row {i + 1}: {row[0]} -> {openAiResponse}");
                }
            }

            var updateRequest = service.Spreadsheets.Values.Update(new ValueRange { Values = values }, SpreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            updateRequest.Execute();
        }
        else
        {
            Console.WriteLine("No data found.");
        }
    }

    static async Task<string> CallOpenAiApi(string prompt)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OpenAiApiKey);

            var requestBody = new
            {
                prompt = prompt,
                max_tokens = 100
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.openai.com/v1/engines/davinci-codex/completions", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseBody);
            return result.choices[0].text.ToString();
        }
    }
}
