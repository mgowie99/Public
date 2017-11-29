
using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace WindowsService
{

	public class Constants
    {
        public const string DisplayName = "My New C# Windows Service";
        public const string ServiceName = "My Windows Service";
	}
/*
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
			serviceInstaller.ServiceName = "My Windows Service";

			this.Installers.Add(serviceProcessInstaller);
			this.Installers.Add(serviceInstaller);
		}
	}
*/
}
