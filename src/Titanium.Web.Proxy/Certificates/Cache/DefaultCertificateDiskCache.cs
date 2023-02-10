using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Titanium.Web.Proxy.Helpers;

namespace Titanium.Web.Proxy.Certificates.Cache;

public sealed class DefaultCertificateDiskCache : ICertificateCache
{
    private const string defaultCertificateDirectoryName = "crts";
    private const string defaultCertificateFileExtension = ".pfx";
    private const string defaultRootCertificateFileName = "rootCert" + defaultCertificateFileExtension;
    private string? rootCertificatePath;

    public X509Certificate2? LoadRootCertificate(string pathOrName, string password, X509KeyStorageFlags storageFlags)
    {
        var path = getRootCertificatePath(pathOrName);
        return loadCertificate(path, password, storageFlags);
    }

    public void SaveRootCertificate(string pathOrName, string password, X509Certificate2 certificate)
    {
        var path = getRootCertificatePath(pathOrName);
        var exported = certificate.Export(X509ContentType.Pkcs12, password);
        File.WriteAllBytes(path, exported);
    }

    /// <inheritdoc />
    public X509Certificate2? LoadCertificate(string subjectName, X509KeyStorageFlags storageFlags)
    {
        var filePath = Path.Combine(getCertificatePath(false), subjectName + defaultCertificateFileExtension);
        return loadCertificate(filePath, string.Empty, storageFlags);
    }

    /// <inheritdoc />
    public void SaveCertificate(string subjectName, X509Certificate2 certificate)
    {
        var filePath = Path.Combine(getCertificatePath(true), subjectName + defaultCertificateFileExtension);
        var exported = certificate.Export(X509ContentType.Pkcs12);
        File.WriteAllBytes(filePath, exported);
    }

    public void Clear()
    {
        try
        {
            var path = getCertificatePath(false);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
        catch (Exception)
        {
            // do nothing
        }
    }

    private X509Certificate2? loadCertificate(string path, string password, X509KeyStorageFlags storageFlags)
    {
        byte[] exported;

        if (!File.Exists(path))
        {
            return null;
        }

        try
        {
            exported = File.ReadAllBytes(path);
        }
        catch (IOException)
        {
            // file or directory not found
            return null;
        }

        return new X509Certificate2(exported, password, storageFlags);
    }

    private string getRootCertificatePath(string pathOrName)
    {
        if (Path.IsPathRooted(pathOrName))
        {
            return pathOrName;
        }

        return Path.Combine(getRootCertificateDirectory(),
            string.IsNullOrEmpty(pathOrName) ? defaultRootCertificateFileName : pathOrName);
    }

    private string getCertificatePath(bool create)
    {
        var path = getRootCertificateDirectory();

        var certPath = Path.Combine(path, defaultCertificateDirectoryName);
        if (create && !Directory.Exists(certPath))
        {
            Directory.CreateDirectory(certPath);
        }

        return certPath;
    }

    private string getRootCertificateDirectory()
    {
        if (rootCertificatePath == null)
        {
            if (RunTime.IsUwpOnWindows())
            {
                rootCertificatePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            }
            else if (RunTime.IsLinux())
            {
                rootCertificatePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }
            else if (RunTime.IsMac())
            {
                rootCertificatePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }
            else
            {
                var assemblyLocation = GetType().Assembly.Location;

                // dynamically loaded assemblies returns string.Empty location
                if (assemblyLocation == string.Empty)
                {
                    assemblyLocation = Assembly.GetEntryAssembly().Location;
                }

#if NETSTANDARD2_1
                // single-file app returns string.Empty location
                if (assemblyLocation == string.Empty)
                {
                    assemblyLocation = AppContext.BaseDirectory;
                }
#endif

                var path = Path.GetDirectoryName(assemblyLocation);

                rootCertificatePath = path ?? throw new NullReferenceException();
            }
        }

        return rootCertificatePath;
    }
}
