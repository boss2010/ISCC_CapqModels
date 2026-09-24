using System.ComponentModel;
using System.Runtime.InteropServices;

namespace PlantMobile.Infrastructure;

public static class AttachmentShareConnection
{
    public static string? GetSharePath(string filePath)
    {
        string path = filePath.Replace('/', '\\');
        if (!path.StartsWith(@"\\")) return null;
        string[] parts = path[2..].Split('\\', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2) throw new IOException("مسار المجلد المشترك غير صحيح.");
        return $@"\\{parts[0]}\{parts[1]}";
    }

    public static void Connect(string filePath, IConfiguration configuration, bool reconnectForUpload = false)
    {
        string? share = GetSharePath(filePath);
        if (share == null) return;
        if (!OperatingSystem.IsWindows())
            throw new IOException("الاتصال بالمجلد المشترك يتطلب تشغيل التطبيق على Windows.");

        string configuredServer = (configuration["Path_NetworkShare"] ?? "").Trim().Trim('\\', '/');
        bool isConfiguredServer = share.StartsWith($@"\\{configuredServer}\", StringComparison.OrdinalIgnoreCase);
        // Credentials are only sent to the server configured for attachments.
        string? username = isConfiguredServer ? configuration["NetworkShare:Username"] : null;
        string? password = isConfiguredServer ? configuration["NetworkShare:Password"] : null;
        if (string.IsNullOrWhiteSpace(username))
        {
            username = null;
            password = null;
        }
        var resource = new NetResource { Type = 1, RemoteName = share };
        if (reconnectForUpload)
            WNetCancelConnection2(share, 0, true);
        int result = WNetUseConnection(IntPtr.Zero, resource, password, username, 0,
            IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        // The legacy uploader proceeds to SaveAs after attempting the connection;
        // directory creation/file writing reports any remaining access failure.
        if (reconnectForUpload) return;
        if (result == 0) return;
        string message = result == 1219
            ? "يوجد اتصال بالمجلد المشترك ببيانات دخول مختلفة. راجع حساب تشغيل التطبيق واتصالات السيرفر."
            : new Win32Exception(result).Message;
        throw new IOException($"تعذر الاتصال بالمجلد المشترك {share} (رمز {result}): {message}");
    }

    [DllImport("mpr.dll", EntryPoint = "WNetUseConnectionW", CharSet = CharSet.Unicode)]
    private static extern int WNetUseConnection(IntPtr owner, NetResource resource, string? password,
        string? username, int flags, IntPtr accessName, IntPtr bufferSize, IntPtr result);

    [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2W", CharSet = CharSet.Unicode)]
    private static extern int WNetCancelConnection2(string name, int flags,
        [MarshalAs(UnmanagedType.Bool)] bool force);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private sealed class NetResource
    {
        public int Scope;
        public int Type;
        public int DisplayType;
        public int Usage;
        public string? LocalName;
        public string? RemoteName;
        public string? Comment;
        public string? Provider;
    }
}
