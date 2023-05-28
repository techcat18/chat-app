using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using ChatApplication.BLL.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ChatApplication.BLL.Services;

public class AzureBlobStorageService: IStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _blobAccessKey;

    public AzureBlobStorageService(
        BlobServiceClient blobServiceClient,
        IConfiguration configuration)
    {
        _blobServiceClient = blobServiceClient;
        _blobAccessKey = configuration.GetSection("Azure:Blob:AccessKey").Value!;
    }
    
    public async Task<string> UploadAsync(
        Stream file, 
        string containerName, 
        string fileName)
    {
        var blobContainerClient = GetBlobContainerClient(containerName);
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(file, true);
        
        var blobUri = blobClient.Uri.ToString();
    
        return blobUri;
    }

    public Task<string> GetSasTokenAsync(
        string containerName,
        string fileName)
    {
        var blobSasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = fileName,
            ExpiresOn = DateTime.UtcNow.AddHours(6)
        };
        
        blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

        var sasToken = blobSasBuilder.ToSasQueryParameters(
            new StorageSharedKeyCredential(_blobServiceClient.AccountName, _blobAccessKey)).ToString();
        
        return Task.FromResult(sasToken);
    }

    public Task DeleteAsync(string blobFilename)
    {
        throw new NotImplementedException();
    }

    private BlobContainerClient GetBlobContainerClient(string blobContainerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
        containerClient.CreateIfNotExists();
        return containerClient;
    }
}