using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;

/**
 * @author Knilax
 */
class Program
{
    // If modifying these scopes, delete your previously saved credentials
    // at ~/.credentials/drive-dotnet-quickstart.json
    static string[] Scopes = { DriveService.Scope.DriveReadonly };
    static string ApplicationName = "Drive API .NET Quickstart";

    /**
     * @desc Main
     */
    static void Main(string[] args)
  {
        // Retrieve credentials
        UserCredential credential;

        using (var stream =
            new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        {
            // The file token.json stores the user's access and refresh tokens, and is created
            // automatically when the authorization flow completes for the first time.
            string credPath = "token.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
            Console.WriteLine("Credential file saved to: " + credPath);
        }

        // Create Drive API service.
        var service = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        // Create request
        FilesResource.ExportRequest exportRequest = service.Files.Export("1-4AHcoC87nNvgFR2N41UzlZmeTx6XAMbG7OPKYUJ_5g", "text/csv");

        // Export data sheet
        string sheetData = exportRequest.Execute();

        using Stream sheetStream = GenerateStreamFromSheetData(sheetData);
        StreamReader sheetFile = null;
        try
        {
            sheetFile = new StreamReader(sheetStream);
        }
        catch (Exception exc)
        {
            Console.Error.Write(exc);
            Environment.Exit(1);
        }
        Sheet sheet = new Sheet(sheetFile);
  }

    static Stream GenerateStreamFromSheetData(string sheetData)
	{
        MemoryStream sheetStream = new MemoryStream();
        StreamWriter writer = new StreamWriter(sheetStream);
        writer.Write(sheetData);
        writer.Flush();
        sheetStream.Position = 0;
        return sheetStream;
	}
}