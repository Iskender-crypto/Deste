using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using File = Deste.Domain.Entities.File;

namespace Deste.Domain.Services;

public interface IFileService
{
    public  Task<FileContentResult> Get<TFile>(long id, bool download = false) where TFile : File, new();
    public Task<TFile> Save<TFile>(TFile model, IFormFile file) where TFile : File, new();
    public Task<TFile> Update<TFile>(TFile model) where TFile : File, new();
    public bool Delete<TFile>(TFile model) where TFile : File, new();
}