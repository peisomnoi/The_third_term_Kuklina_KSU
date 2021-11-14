using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortLab
{
    class GoogleTable
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "MNK";
        public string SpreadsheetId { get; set; }
        static SheetsService service;
        public GoogleTable(string spreadSheetId)
        {
            SpreadsheetId = spreadSheetId;
            GoogleCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }
            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }
        public string[] GetSheetTitles()
        {
            List<string> titles = new List<string>();
            var sheets = service.Spreadsheets.Get(SpreadsheetId).Execute().Sheets;
            foreach (var item in sheets)
            {
                titles.Add(item.Properties.Title);
            }
            return titles.ToArray();
        }
        public object ReadEntries(string sheet)
        {
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, sheet);
            var response = request.Execute();
            var values = response.Values;
            return values;
        }
    }
}
