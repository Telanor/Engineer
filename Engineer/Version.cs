// Kerbal Engineer Redux
// Author:  CYBUTEK
// License: Attribution-NonCommercial-ShareAlike 3.0 Unported

using System;
using UnityEngine;

namespace Engineer
{
	public class Version
	{
        public const string VERSION = "0.6.2.2";
		public const string PRODUCT_NAME = "engineer_redux";
		private string remoteVersion = null;
		private bool hasCompared = false;
		private bool isNewer = false;
		private WWW updateRequest;

		public bool Same
		{
			get
			{
				if(Remote != Local)
					return true;
				return false;
			}
		}

		public bool Older
		{
			get
			{
				if(!hasCompared)
				{
					try
					{
						CompareVersions();
					}
					catch {}
				}
				if(!isNewer && !Same)
					return true;
				return false;
			}
		}

		public bool Newer
		{
			get
			{
				if(!hasCompared)
				{
					try
					{
						CompareVersions();
					}
					catch {}
				}
				return isNewer;
			}
		}

		public string Local
		{
			get { return VERSION; }
		}

		public string Remote
		{
			get
			{
				if(remoteVersion == null)
				{
					try
					{
						remoteVersion = GetRemoteVersion();
					}
					catch {}
				}
				return remoteVersion;
			}
		}

		private void CompareVersions()
		{
			if(!string.IsNullOrEmpty(Remote))
			{
				hasCompared = true;
				return;
			}

			string[] local = Local.Split('.');
			string[] remote = Remote.Split('.');

			try
			{
				if(local.Length > remote.Length)
				{
					Array.Resize(ref remote, local.Length);

					for(int i = 0; i < remote.Length; i++)
					{
						if(remote[i] == null)
							remote[i] = "0";
					}
				}
				else
				{
					Array.Resize(ref local, remote.Length);

					for(int i = 0; i < local.Length; i++)
					{
						if(local[i] == null)
							local[i] = "0";
					}
				}

				for(int i = 0; i < local.Length; i++)
				{
					if(Convert.ToInt32(local[i]) < Convert.ToInt32(remote[i]))
					{
						isNewer = true;
						break;
					}

					if(Convert.ToInt32(local[i]) > Convert.ToInt32(remote[i]))
					{
						isNewer = false;
						break;
					}
				}
			}
			catch {}

			hasCompared = true;
		}

		private string GetRemoteVersion()
		{
			try
			{
				if(updateRequest == null)
					updateRequest = new WWW("http://www.cybutek.net/ksp/getversion.php?name=" + PRODUCT_NAME);

				return updateRequest.isDone ? updateRequest.text : string.Empty;
			}
			catch {}

			return string.Empty;
		}
	}
}