using Microsoft.AspNetCore.StaticFiles;

namespace PlantMobile.Infrastructure;

public static class ImportAttachmentFiles
{
    public static string ResolvePath(string storedPath, string networkShare)
    {
        string path = storedPath.Trim().Replace('/', '\\');
        if (path.StartsWith(@"\\") || Path.IsPathFullyQualified(path)) return path;
        string server = networkShare.Trim().Trim('\\', '/');
        if (string.IsNullOrWhiteSpace(server))
            throw new InvalidOperationException("Path_NetworkShare is not configured.");
        return $@"\\{server}\{path.TrimStart('\\')}";
    }

    public static string ContentType(string path, string? storedType)
    {
        var provider = new FileExtensionContentTypeProvider();
        if (provider.TryGetContentType(path, out string? type)) return type;
        if (!string.IsNullOrWhiteSpace(storedType))
        {
            if (storedType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)
                || storedType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                return storedType;
            if (provider.TryGetContentType("attachment." + storedType.TrimStart('.'), out type)) return type;
        }
        return "application/octet-stream";
    }
}
