using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

namespace App.Helpers
{
	public class DataManager : MonoBehaviour
	{
		public static DataManager Instance { get; private set; }
		public string Version { get; set; }

		private const string Directory = "gData";
		private const string File = "ht3ds.hl";
		private static string _fullPath = "";

		public UserSave UserData { get; private set; }


#if UNITY_EDITOR
		private void OnValidate()
		{
			Version = PlayerSettings.bundleVersion;
		}
#endif


		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);

				_fullPath = Path.Combine(Application.persistentDataPath, Directory);

				LoadSaveData();
			}
			else
			{
				Destroy(this);
			}
		}

		public void DeleteDataBase()
		{
			if (System.IO.File.Exists(_fullPath + "/" + File))
				System.IO.File.Delete(_fullPath + "/" + File);

			PlayerPrefs.DeleteAll();
		}

		#region General Database Functions

		private void LoadSaveData()
		{
			switch (CreateDatabaseIfThereIsNoDataBase())
			{
				case 1:
				case 2:
					Load();
					break;
				default:
					CreateDatabaseIfThereIsNoDataBase();
					break;
			}

			UpdateDatabase();
		}

		private void Load()
		{
			var bf = new BinaryFormatter();
			var file = System.IO.File.Open(_fullPath + "/" + File, FileMode.Open);

			if (file.Length == 0)
			{
				file.Close();
				DeleteDataBase();
				CreateDatabaseIfThereIsNoDataBase();
				return;
			}

			try
			{
				UserData = (UserSave) bf.Deserialize(file);
				file.Close();
			}
			catch (Exception exception)
			{
				Debug.Log(exception);

				file.Close();
				CreateDatabaseIfThereIsNoDataBase();
			}
		}


		/// <summary>
		/// Does what it say.
		/// </summary>
		/// <returns>0 => error, 1 => exists, 2 => created</returns>
		private int CreateDatabaseIfThereIsNoDataBase()
		{
			try
			{
				if (System.IO.File.Exists(_fullPath + "/" + File)) return 1;

				if (!System.IO.Directory.Exists(_fullPath))
					System.IO.Directory.CreateDirectory(_fullPath);

				UserData = new UserSave(Version);

				return Save() ? 2 : 0;
			}
			catch
			{
				return 0;
			}
		}

		public bool Save()
		{
			if (UserData == null) return false;
			try
			{
				var bf = new BinaryFormatter();
				var file = System.IO.File.Create(_fullPath + "/" + File);

				bf.Serialize(file, UserData);
				file.Close();

				return true;
			}
			catch
			{
				return false;
			}
		}

		private void UpdateDatabase()
		{
			if (UserData.Version == Version)
				return;
			
			//Set upgrade
			UserData.Version = Version;

			Save();
		}

		#endregion
	}

	[Serializable]
	public class UserSave
	{
		public string Version { get; set; }

		public UserSave(string version)
		{
			this.Version = version;
		}
	}
}