namespace Deste.Domain.Utils;

public static class FolderUtils
{
    public static void CreateFoldersIfNotExist(string folderPath)
    {
        try
        {
            if (Directory.Exists(folderPath)) return;
            var folders = folderPath.Split('/');
            var currentPath = folders[0];
            for (var i = 1; i < folders.Length; i++)
            {
                currentPath = Path.Combine(currentPath, folders[i]);
                if (Directory.Exists(currentPath)) continue;
                Directory.CreateDirectory(currentPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании папок: {ex.Message}");
        }
    }
}