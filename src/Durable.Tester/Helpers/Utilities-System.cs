using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Durable.Test.Harness.Helpers;

public static partial class Utilities
{
    #region Variables
    /// <summary>
    /// The operating system
    /// </summary>
    private static string operatingSystem = string.Empty;

    /// <summary>
    /// The operating architecture (i.e. ARM vs X64)
    /// </summary>
    private static string operatingArchitecture = string.Empty;

    /// <summary>
    /// The IP Address
    /// </summary>
    private static string ipAddress = string.Empty;

    /// <summary>
    /// The MAC Address
    /// </summary>
    private static string macAddress = string.Empty;

    /// <summary>
    /// The operating system
    /// </summary>
    public static string OperatingSystem
    {
        get
        {
            if (string.IsNullOrEmpty(operatingSystem))
            {
                GetOperatingSystemName();
            }
            return operatingSystem;
        }
    }

    /// <summary>
    /// The operating architecture (i.e. ARM vs X64)
    /// </summary>
    public static string OperatingArchitecture
    {
        get
        {
            if (string.IsNullOrEmpty(operatingArchitecture))
            {
                GetOperatingSystemName();
            }
            return operatingArchitecture;
        }
    }

    /// <summary>
    /// The IP Address
    /// </summary>
    public static string IpAddress
    {
        get
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                GetIPAddress();
            }
            return ipAddress;
        }
    }

    /// <summary>
    /// The MAC Address
    /// </summary>
    public static string MacAddress
    {
        get
        {
            if (string.IsNullOrEmpty(macAddress))
            {
                GetMacAddress();
            }
            return macAddress;
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Get Application Directory (without ending slash)
    /// </summary>
    /// <returns>Directory</returns>
    public static string GetApplicationDirectory()
    {
        //// In .NET Core 3+, you can create a Single file executable (SFE), which unzips itself into a temp directory.
        //// When you do that, the GetExecutingAssembly() line returns the temp folder where it's been unzipped.
        //// You need to look at GetCurrentProcess() to find the real home directory
        //// See https://github.com/dotnet/core-setup/issues/7491
        //// In previous versions of .NET 4.x and .NET Core 2.x, this worked fine...  not so much with .NET Core 3+ SFE
        ////   var applicationDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        //// In .NET Core 3+, you need to do it this way...
        using var processModule = Process.GetCurrentProcess().MainModule;
        var exeDirectory = Path.GetDirectoryName(processModule?.FileName);
        exeDirectory += exeDirectory.EndsWith(GetPathDelimiter()) ? string.Empty : GetPathDelimiter();
        return exeDirectory;
    }

    /// <summary>
    /// Get File Version Info
    /// </summary>
    /// <returns>Version</returns>
    public static string GetProgramVersion(string programTitle)
    {
        using var processModule = Process.GetCurrentProcess().MainModule;
        var version = processModule.FileVersionInfo.FileVersion;

        var exeFilePath = processModule.FileName;
        var fileInfo = new FileInfo(exeFilePath);
        var publishDate = fileInfo.LastWriteTime;

        return $"{programTitle}\n*   Version: {version}\n*   Published: {publishDate}";
    }

    /// <summary>
    /// Gets the name of the operating system.
    /// </summary>
    /// <returns>Operating System Name</returns>
    public static string GetOperatingSystemName()
    {
        if (string.IsNullOrEmpty(operatingSystem))
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    operatingSystem = OSPlatform.Windows.ToString();
                }
                else
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        operatingSystem = OSPlatform.Linux.ToString();
                    }
                    else
                    {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                        {
                            operatingSystem = OSPlatform.OSX.ToString();
                        }
                    }
                }
                operatingArchitecture = RuntimeInformation.OSArchitecture.ToString();
            }
            catch (Exception exc)
            {
                Console.WriteLine(string.Format(" => Error reading operating system: {0}", exc.Message));
                operatingSystem = "Error";
            }
        }
        ////Console.WriteLine("OS: " + operatingSystem + " " + operatingArchitecture);
        ////Console.WriteLine("  OSArchitecture: " + RuntimeInformation.OSArchitecture);
        ////Console.WriteLine("  ProcessArchitecture: " + RuntimeInformation.ProcessArchitecture);
        ////Console.WriteLine("  OSArchitecture: " + RuntimeInformation.OSArchitecture);
        ////Console.WriteLine("  FrameworkDescription: " + RuntimeInformation.FrameworkDescription);
        return operatingSystem;
    }

    /// <summary>
    /// Gets the path delimiter.
    /// </summary>
    /// <returns>Delimiter</returns>
    public static string GetPathDelimiter()
    {
        var operatingSystem = GetOperatingSystemName();
        var delimiter = IsLinux() ? "/" : "\\";
        return delimiter;
    }

    /// <summary>
    /// Determines whether this instance is Linux.
    /// </summary>
    /// <returns>
    ///   Returns true if this instance is Linux otherwise false
    /// </returns>
    public static bool IsLinux()
    {
        GetOperatingSystemName();
        return operatingSystem == OSPlatform.Linux.ToString();
    }

    /// <summary>
    /// Determines whether this instance is Linux ARM.
    /// </summary>
    /// <returns>
    ///   Returns true if this instance is Linux ARM otherwise false
    /// </returns>
    public static bool IsRaspberryPi()
    {
        GetOperatingSystemName();
        var isPi = operatingSystem == OSPlatform.Linux.ToString() && operatingArchitecture == "Arm";
        Console.WriteLine("  Checking for Raspberry Pi -> OS: {0} Arch: {1} -> Result={2}", operatingSystem, operatingArchitecture, isPi);
        return isPi;
    }

    /// <summary>
    /// Gets The IP address.
    /// </summary>
    /// <returns>IP Address</returns>
    public static string GetIPAddress()
    {
        if (!string.IsNullOrEmpty(ipAddress))
        {
            return ipAddress;
        }
        try
        {
            var localIp = IPAddress.None;
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                //// Connect socket to Google's Public DNS service
                socket.Connect("8.8.8.8", 65530);
                if (!(socket.LocalEndPoint is IPEndPoint endPoint))
                {
                    throw new InvalidOperationException($"Error occurred casting {socket.LocalEndPoint} to IPEndPoint");
                }
                localIp = endPoint.Address;
            }
            ipAddress = localIp.MapToIPv4().ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error getting IP Address: " + ex.Message);
            ipAddress = "0.0.0.0";
        }
        return ipAddress;
    }

    /// <summary>
    /// Gets the mac address.
    /// </summary>
    /// <returns>Mac Address</returns>
    public static string GetMacAddress()
    {
        if (!string.IsNullOrEmpty(macAddress))
        {
            return macAddress;
        }
        try
        {
            ////var testing_View_All_Nics =
            ////(
            ////    from nic in NetworkInterface.GetAllNetworkInterfaces()
            ////    //where nic.NetworkInterfaceType != NetworkInterfaceType.Loopback
            ////    //&& !nic.Description.Contains("Hyper-V", StringComparison.InvariantCultureIgnoreCase)
            ////    //&& nic.OperationalStatus == OperationalStatus.Up
            ////    select nic
            ////);

            //// NOTE/WARNING: You will get a different MAC address when on WiFi vs when an ethernet cable is plugged in...
            var macAddr =
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback
                && !nic.Description.Contains("Hyper-V", StringComparison.InvariantCultureIgnoreCase)
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();
            macAddress = macAddr.ToLower(CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error getting IP Address: " + ex.Message);
            macAddress = "00:00:00:00:00";
        }
        return macAddress;
    }
    #endregion
}
