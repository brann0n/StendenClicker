using System;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage.Streams;
using Windows.System.Profile;

namespace StendenClickerGame
{
	public sealed class DeviceInfo
	{
		private static DeviceInfo _Instance;
		public static DeviceInfo Instance
		{
			get
			{
				if (_Instance == null)
					_Instance = new DeviceInfo();
				return _Instance;
			}
		}

		public string Id { get; private set; }
		public string Model { get; private set; }
		public string Manufracturer { get; private set; }
		public string Name { get; private set; }
		public static string OSName { get; set; }

		private DeviceInfo()
		{
			Id = GetId();
			var deviceInformation = new EasClientDeviceInformation();
			Model = deviceInformation.SystemProductName;
			Manufracturer = deviceInformation.SystemManufacturer;
			Name = deviceInformation.FriendlyName;
			OSName = deviceInformation.OperatingSystem;
		}

		private static string GetId()
		{
			if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.System.Profile.HardwareIdentification"))
			{
				var token = HardwareIdentification.GetPackageSpecificToken(null);
				var hardwareId = token.Id;
				var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

				byte[] bytes = new byte[hardwareId.Length];
				dataReader.ReadBytes(bytes);

				return BitConverter.ToString(bytes).Replace("-", "");
			}

			throw new Exception("NO API FOR DEVICE ID PRESENT!");
		}

		public string GetSystemId()
		{
			var systemId = SystemIdentification.GetSystemIdForPublisher();

			// Make sure this device can generate the IDs
			if (systemId.Source != SystemIdentificationSource.None)
			{
				var dataReader = DataReader.FromBuffer(systemId.Id);
				byte[] bytes = new byte[systemId.Id.Length];
				dataReader.ReadBytes(bytes);
				// The Id property has a buffer with the unique ID
				return BitConverter.ToString(bytes);
			}

			//if for some reason the above didnt generate a device id -> use a fallback method.
			return GetId();
		}
	}
}
