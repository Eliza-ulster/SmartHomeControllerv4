using SmartHomeController;
using System.Net.NetworkInformation;

public class Program
{
    // Globally available variables within the SmartHomeController file
    private static List<SmartDevice> devices = new List<SmartDevice>();
    static string destinationFilePath;

    // Starting point for the application
    public static void Main()
    {
        string folder = "Data";
        string filename = "smartdevices.csv";

        destinationFilePath = CopyDataToWorkingDir(folder, filename);
        LoadSmartDevices(destinationFilePath);
        MainMenu();
    }

    // Copy csv file from the Data folder to the path
    // C:\Users\user profile\source\repos\SmartHomeControllerv4\SmartHomeControllerv4\bin\Debug\net8.0
    // Note that this method overwrites the csv file stored in the "net8.0" folder
    public static string CopyDataToWorkingDir(string folder, string filename)
    {
        // Define the source and destination paths
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        string sourceFilePath = Path.Combine(projectDirectory, folder, filename);
        string destinationFilePath = Path.Combine(Environment.CurrentDirectory, filename);

        // Copy the file to the working directory
        if (File.Exists(sourceFilePath))
        {
            File.Copy(sourceFilePath, destinationFilePath, true);
        }
        else
        {
            Console.WriteLine("Source file not found: " + sourceFilePath);

        }
        // Retrun the path to the copied file
        return destinationFilePath;
    }

    public static void LoadSmartDevices(string destinationFilePath)
    {
        using (var reader = new StreamReader(destinationFilePath))
        {
            // Skip the header line
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                // Need to convert missing data into 0 for
                // double and int
                if (values[3].Length == 0)
                {
                    values[3] = "0";
                }
                if (values[6].Length == 0)
                {
                    values[6] = "0";
                }
                if (values[7].Length == 0)
                {
                    values[7] = "0";
                }
                if (values[8].Length == 0)
                {
                    values[8] = "0";
                }
                // read each value into the relevant 
                // variable 
                int deviceID = int.Parse(values[0]);
                string deviceName = values[2];
                string deviceType = values[1];
                double brightness = Convert.ToDouble((values[3]));
                string colour = values[4];
                string cameraResolution = values[5];

                double currentTemperature = Convert.ToDouble(values[6]);
                double targetTemperature = Convert.ToDouble(values[7]);
                int volume = int.Parse(values[8]);

                SmartDevice device = null;

                // Device type is read from the csv file
                switch (deviceType)
                {
                    case "SmartLight":
                        device = new SmartLight(deviceID, deviceName, brightness, colour);
                        break;
                    case "SmartSecurityCamera":
                        device = new SmartSecurityCamera(deviceID, deviceName, cameraResolution);
                        break;
                    case "SmartThermostat":
                        device = new SmartThermostat(deviceID, deviceName, currentTemperature, targetTemperature);
                        break;
                    case "SmartSpeaker":
                        device = new SmartSpeaker(deviceID, deviceName, volume);
                        break;
                }
                // add the device into the list of devices
                if (device != null)
                {
                    devices.Add(device);
                }
            }
        }
    }

    public static void MainMenu()
    {
        while (true)
        {           
            Console.WriteLine("Main Menu:");
            Console.WriteLine("1. Install new device");
            Console.WriteLine("2. Control a device");
            Console.WriteLine("3. View all devices");
            Console.WriteLine("4. Exit");

            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    InstallDeviceMenu();
                    break;
                case "2":
                    //ControlDevicesMenu();
                    break;
                case "3":
                    //ViewAllDevices();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
    public static void InstallDeviceMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Intall a device name ");
            Console.WriteLine("1. Install new smart light");
            Console.WriteLine("2. Install a new smart camera");
            Console.WriteLine("3. Return to main menu");
            

            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    InstallSmartLightMenu();
                    break;
                case "2":
                    //InstallSmartCameraMenu();
                    break;
                case "3":
                    //ViewAllDevices();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

    }
    public static void InstallSmartLightMenu()
    {
        Console.WriteLine("\nInstall smart light");
        Console.WriteLine("Enter device ID:");
        int deviceId =Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("\nEnter device name:");
        string deviceName = Console.ReadLine();

        Console.WriteLine("Enter brightness:");
        double brightness = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Enter colour:");
        string colour = Console.ReadLine();

        SmartLight smartLight = new SmartLight(deviceId, deviceName , brightness,colour);

        devices.Add(smartLight);
        Console.WriteLine("Smart Light installed:");
        Console.WriteLine($"Device ID :{smartLight.DeviceID}");
        Console.WriteLine($"{smartLight.DeviceName}");
        Console.WriteLine(smartLight.Brightness);
        Console.WriteLine(smartLight.Colour);

        Console.WriteLine("Enter ANY KEY");
        Console.ReadKey();
    }

    public static void SaveDevices(string filepath,List<SmartDevice> devices)
    {
        var lines = new List<string>
        {
           " DeviceId, DeviceType ,Brightness, CameraResolution,CurrentTemperature,TargetTemperature,SpeakerVolume"
        };
        foreach (var device in devices)
        {
            string deviceLine="";
            if (device is SmartLight light)
            {
                deviceLine = $"{light.DeviceID},SmartLight,{light.DeviceName},{light.Status},{light.Brightness},{light.Colour},,,";
            }
            lines.Add(deviceLine);
            File.WriteAllLines(filepath,lines);

        }
    }
    public static void InstallSmartCameraMenu()
    {



        Console.WriteLine("hi");
    }



}
