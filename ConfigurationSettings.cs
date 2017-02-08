﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace KpdApps.Crm.Ms.Common
{
	public class ConfigurationSettings
	{
		private static readonly object SyncObject = new object();

		private static readonly Dictionary<string, ConfigurationSettings> Instance = new Dictionary<string, ConfigurationSettings>();

		private readonly NameValueCollection _settings;

		private string _userName;
		public string UserName => _userName ?? (_userName = GetValue("UserName"));

		private string _password;
		public string Password => _password ?? (_password = GetValue("Password"));

		private string _domain;

		public string Domain => _domain ?? (_domain = GetValue("Domain"));

		private string _organization;

		public string Organization => _organization ?? (_organization = GetValue("Organization"));

		private string _service;
		public string Service => _service ?? (_service = GetValue("Service"));


		private string _sqlConnectionString;
		public string SqlConnectionString => _sqlConnectionString ?? (_sqlConnectionString = GetValue("CrmConnectionString"));

		internal ConfigurationSettings(NameValueCollection collection)
		{
			_settings = collection;
		}

		public static ConfigurationSettings GetSettings(string sectionName, bool appSettingsKey)
		{
			lock (SyncObject)
			{
				if (Instance.ContainsKey(sectionName))
					return Instance[sectionName];

				var settings = (NameValueCollection)ConfigurationManager.GetSection(sectionName);

				if (!Instance.ContainsKey(sectionName))
					Instance.Add(sectionName, new ConfigurationSettings(settings));
				else
					Instance[sectionName] = new ConfigurationSettings(settings);

				return Instance[sectionName];
			}
		}

		public string GetValue(string key)
		{
			if (_settings != null)
				return _settings.Get(key) ?? string.Empty;
			return string.Empty;
		}

		public void SetValue(string key, string value)
		{
			if (_settings != null)
				_settings[key] = value;
		}
	}
}
