using Microsoft.AspNetCore.Http;

namespace Deste.Domain.Utils;

public static class FileUtils
{
    public static async Task<byte[]> GetByteArrayAsync(this IFormFile file)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var data = ms.ToArray();
        return data;
    }

    public static async Task SaveToFileSystemAsync(string path, byte[] data)
    {
        await using var fs = new FileStream(path, FileMode.OpenOrCreate);
        await fs.WriteAsync(data);
    }

    public static byte[] GetByteArrayAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException($"Файл {filePath} не найден.");
            return File.ReadAllBytes(filePath);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при чтении файла: {ex.Message}", ex);
        }
    }

    public static void DeleteFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                throw new FileNotFoundException($"Файл {filePath} не найден.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при удалении файла: {ex.Message}", ex);
        }
    }
}