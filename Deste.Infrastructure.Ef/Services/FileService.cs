using Deste.Domain.Services;
using Deste.Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using File = Deste.Domain.Entities.File;

namespace Deste.Infrastructure.Ef.Services;

public class FileService(DataContext dataContext, IConfiguration configuration) : IFileService
{
    private string BasePath => configuration.GetValue<string>("FileStorages:File")!;

    public async Task<FileContentResult> Get<TFile>(long id, bool download = false) where TFile : File, new()
    {
        var file = await dataContext.Set<TFile>().FindAsync(id);
        if (file == null) throw new Exception($"Файл по ID {id}");
        var data = FileUtils.GetByteArrayAsync($"{BasePath}/{file.StorageId}");
        return download
            ? new FileContentResult(data, "image/jpg") { FileDownloadName = file.Name }
            : new FileContentResult(data, "image/jpg");
    }
    public async Task<TFile> Save<TFile>(TFile model, IFormFile file) where TFile : File, new()
    {
        FolderUtils.CreateFoldersIfNotExist(BasePath);
        var filePath = Path.Combine(BasePath, model.StorageId);
        var data = await file.GetByteArrayAsync();
        await FileUtils.SaveToFileSystemAsync(filePath, data);
        dataContext.Add(model);
        return model;
    }

    public async Task<TFile> Update<TFile>(TFile model) where TFile : File, new()
    {
        if (!model.Base64.IsBase64String()) throw new Exception("Файл повреждён");
        var data = Convert.FromBase64String(model.Base64);
        var folderPath = $"{BasePath}/{model.StorageId}";
        await FileUtils.SaveToFileSystemAsync(folderPath, data);
        dataContext.Set<TFile>().Update(model);
        return model;
    }

    public bool Delete<TFile>(TFile model) where TFile : File, new()
    {
        try
        {
            var filePath = $"{BasePath}/{model.StorageId}";
            FileUtils.DeleteFile(filePath);
            dataContext.Set<TFile>().Remove(model);
            return true;
        }
        catch (Exception e)
        {
            throw new Exception("не удалось удалить файл");
        }
    }
}