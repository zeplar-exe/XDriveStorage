using System.Numerics;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Discovery;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using XDriveStorage.Configuration;
using XDriveStorage.Users;

using File = Google.Apis.Drive.v3.Data.File;

namespace XDriveStorage.Drives.Builtin;

[BuiltinDrive]
public class GoogleDrive : IDrive
{
    private const string ModuleName = "XDRIVE_GOOGLE_DRIVE_APP";

    public string Name { get; }
    public DriveCredentials Credentials { get; }
    public DriveConfiguration Configuration { get; }

    public GoogleDrive(DriveConfiguration configuration)
    {
        Name = "Google_Drive";
        Credentials = new DriveCredentials(new[]
        {
            "client_id",
            "client_secret"
        });
        Configuration = configuration;
    }

    public async Task<StorageLimit> GetStorageLimit(UserCredentials userCredentials)
    {
        var service = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = await GetGoogleCredentialsAsync(userCredentials),
            ApplicationName = ModuleName
        });

        var getRequest = service.About.Get();
        var about = await getRequest.ExecuteAsync();

        var limit = about.StorageQuota.Limit;
        
        return limit == null ? StorageLimit.Unlimited : StorageLimit.From(limit.Value);
    }

    public async Task<string[]> ListFileNames(UserCredentials userCredentials)
    {
        var files = await ListFilesAsync(userCredentials);

        return files.Select(f => f.Name).ToArray();
    }

    public async Task<bool> ReadFile(UserCredentials userCredentials, string name, Stream outputStream)
    {
        var service = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = await GetGoogleCredentialsAsync(userCredentials),
            ApplicationName = ModuleName
        });

        var fileId = (await GetFileByName(userCredentials, name))?.Id;

        if (fileId == null)
            return false;

        var getRequest = service.Files.Get(fileId);
        getRequest.RequestParameters["alt"] = new Parameter { DefaultValue = "media" };

        var success = false;
        
        if (Program.Verbose)
        {
            getRequest.MediaDownloader.ProgressChanged += progress =>
            {
                Console.WriteLine($"Downloading '{name}': {progress.Status} {progress.BytesDownloaded}");

                if (progress.Status == DownloadStatus.Completed)
                    success = true;
            };
        }

        await getRequest.DownloadAsync(outputStream);

        return success;
    }

    public async Task<bool> WriteFile(UserCredentials userCredentials, string name, Stream content)
    {
        var service = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = await GetGoogleCredentialsAsync(userCredentials),
            ApplicationName = ModuleName
        });
        
        var driveFile = new File
        {
            Name = name
        };
        
        var existingFileId = (await GetFileByName(userCredentials, name))?.Id;

        if (existingFileId != null)
            await service.Files.Delete(existingFileId).ExecuteAsync();

        var insertRequest = service.Files.Create(driveFile, content, "");
        var success = false;

        if (Program.Verbose)
        {
            insertRequest.ProgressChanged += progress =>
            {
                Console.WriteLine($"Uploading '{name}': {progress.Status} {progress.BytesSent}/{content.Length}");
            };
        }
        
        insertRequest.ResponseReceived += file =>
        {
            Console.WriteLine($"File '{name}' successfully uploaded.");

            success = true;
        };

        await insertRequest.UploadAsync();

        return success;
    }

    public async Task<bool> DeleteFile(UserCredentials userCredentials, string name)
    {
        var service = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = await GetGoogleCredentialsAsync(userCredentials),
            ApplicationName = ModuleName
        });
        
        var fileId = (await GetFileByName(userCredentials, name))?.Id;

        if (fileId == null)
            return false;

        var deleteRequest = service.Files.Delete(fileId);

        await deleteRequest.ExecuteAsync();
        
        return true;
    }

    private async Task<File?> GetFileByName(UserCredentials userCredentials, string name)
    {
        var files = await ListFilesAsync(userCredentials);

        return files.FirstOrDefault(f => f.Name == name);
    }

    private async Task<IList<File>> ListFilesAsync(UserCredentials userCredentials)
    {
        var service = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = await GetGoogleCredentialsAsync(userCredentials),
            ApplicationName = ModuleName
        });

        var listRequest = service.Files.List();
        listRequest.PageSize = int.MaxValue;
        
        var files = await listRequest.ExecuteAsync();

        return files.Files;
    }
    
    private async Task<UserCredential> GetGoogleCredentialsAsync(UserCredentials userCredentials)
    {
        var secretsStream = new MemoryStream();

        var secretsJson = new JObject
        {
            { "client_id", userCredentials["client_id"] },
            { "client_secret", userCredentials["client_secret"] }
        };
        // https://developers.google.com/api-client-library/dotnet/guide/aaa_client_secrets
        // Only client_id and client_secret are required. Thus we can just use the credentials on their own.
        
        await using var secretsWriter = new StreamWriter(secretsStream);
        await secretsWriter.WriteAsync(secretsJson.ToString(Formatting.None));

        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            secretsStream, new[] { DriveService.Scope.Drive }, ModuleName, 
            CancellationToken.None);

        return credential;
    }

    public override string ToString()
    {
        return JObject.FromObject(this).ToString(Formatting.Indented);
    }
}