
// service.cs

// 2017-11-29 : added to GitHub

using System;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Timers;

namespace WindowsService
{

// ----- This code should be placed in a separate C# file (with the same namespace)
//       and linked through a DLL, but it doesn't work. Probably because the compiler
//       dispenses with links that appear not to be called if treated as a console
//       app. It is the service installer that calls this. When linked through the 
//       DLL, the installer cannot find this entry point.
// namespace WindowsService
// {

		[RunInstaller(true)]
		public class WindowsServiceInstaller : Installer
		{
			/// <summary>
			/// Public Constructor for WindowsServiceInstaller.
			/// - Put all of your Initialization code here.
			/// </summary>
			public WindowsServiceInstaller()
			{
				ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
				ServiceInstaller serviceInstaller = new ServiceInstaller();

				//# Service Account Information
				serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
				serviceProcessInstaller.Username = null;
				serviceProcessInstaller.Password = null;

				//# Service Information
				serviceInstaller.DisplayName = Constants.DisplayName;
				serviceInstaller.StartType = ServiceStartMode.Automatic;

				//# This must be identical to the WindowsService.ServiceBase name set in the constructor of WindowsService.cs
				serviceInstaller.ServiceName = Constants.ServiceName;

				this.Installers.Add(serviceProcessInstaller);
				this.Installers.Add(serviceInstaller);
			}
		}

// }
// ----- This code could be placed in a separate C# file and linked through a DLL

    class WindowsService : ServiceBase
    {
		
		static System.Timers.Timer tmrMain = null;
		static bool IsConsole = false;
        /// <summary>
        /// Public Constructor for WindowsService.
        /// - Put all of your Initialization code here.
        /// </summary>
        public WindowsService()
        {
            this.ServiceName = Constants.ServiceName;
            this.EventLog.Log = "Application";
            
            // These Flags set whether or not to handle that specific
            //  type of event. Set to true if you need it, false otherwise.
            this.CanHandlePowerEvent = true;
            this.CanHandleSessionChangeEvent = true;
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;
        }

        /// <summary>
        /// Dispose of objects that need it here.
        /// </summary>
        /// <param name="disposing">Whether
        ///    or not disposing is going on.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// OnStart(): Put startup code here
        ///  - Start threads, get inital data, etc.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
			tmrMain.Enabled = true;
			WriteEvent("Started");
            base.OnStart(args);
        }

        /// <summary>
        /// OnStop(): Put your stop code here
        /// - Stop threads, set final data, etc.
        /// </summary>
        protected override void OnStop()
        {
			tmrMain.Enabled = false;
			WriteEvent("Stopped");
            base.OnStop();
        }

        /// <summary>
        /// OnPause: Put your pause code here
        /// - Pause working threads, etc.
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
        }

        /// <summary>
        /// OnContinue(): Put your continue code here
        /// - Un-pause working threads, etc.
        /// </summary>
        protected override void OnContinue()
        {
            base.OnContinue();
        }

        /// <summary>
        /// OnShutdown(): Called when the System is shutting down
        /// - Put code here when you need special handling
        ///   of code that deals with a system shutdown, such
        ///   as saving special data before shutdown.
        /// </summary>
        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        /// <summary>
        /// OnCustomCommand(): If you need to send a command to your
        ///   service without the need for Remoting or Sockets, use
        ///   this method to do custom methods.
        /// </summary>
        /// <param name="command">Arbitrary Integer between 128 & 256</param>
        protected override void OnCustomCommand(int command)
        {
            //  A custom command can be sent to a service by using this method:
            //#  int command = 128; //Some Arbitrary number between 128 & 256
            //#  ServiceController sc = new ServiceController("NameOfService");
            //#  sc.ExecuteCommand(command);

            base.OnCustomCommand(command);
        }

        /// <summary>
        /// OnPowerEvent(): Useful for detecting power status changes,
        ///   such as going into Suspend mode or Low Battery for laptops.
        /// </summary>
        /// <param name="powerStatus">The Power Broadcast Status
        /// (BatteryLow, Suspend, etc.)</param>
        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return base.OnPowerEvent(powerStatus);
        }

        /// <summary>
        /// OnSessionChange(): To handle a change event
        ///   from a Terminal Server session.
        ///   Useful if you need to determine
        ///   when a user logs in remotely or logs off,
        ///   or when someone logs into the console.
        /// </summary>
        /// <param name="changeDescription">The Session Change
        /// Event that occured.</param>
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);
        }
		
		// ------------------------------------------------------------------------------------------------------------------
		
		static void WriteEvent(string evt)
		{
			// string pth = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			// string pth = Environment.CurrentDirectory;
			string pth = @"C:\Temp";
			string dtm = DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
			StreamWriter sw = new StreamWriter(pth + @"\service.log", true);
			sw.WriteLine(dtm + " " + evt);
			sw.Close();
		}

 		static void tmrTick(object sender, ElapsedEventArgs e)
		{
			if (IsConsole) Console.WriteLine("tick .. ");
			WriteEvent("Event");
			GC.Collect();
		}
		
		static void ConsoleMode()
		{
			ConsoleKeyInfo key;
			IsConsole = true;
			WriteEvent("Running as console");
			tmrMain.Enabled = true;
			Console.WriteLine(Constants.DisplayName);
			Console.WriteLine("Enter 'Q' to stop the timer ... ");
			do {
				key = Console.ReadKey();
			} while (key.KeyChar != 'Q' && key.KeyChar != 'q');
			tmrMain.Enabled = false;
		}

		/// <summary>
        /// The Main Thread: This is where your Service is Run.
        /// </summary>
        static void Main(string[] args)
        {
			tmrMain = new Timer();
			tmrMain.Interval = 60000; // 5000 => 5 seconds
			tmrMain.Elapsed += new System.Timers.ElapsedEventHandler(tmrTick);
			if (Environment.UserInteractive) {
				ConsoleMode();
			} else {
				ServiceBase.Run(new WindowsService());
			}
		}
    }
}