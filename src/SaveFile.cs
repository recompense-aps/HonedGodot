using System;
using Godot;

namespace HonedGodot
{
	public abstract class SaveFile
	{
		public string ResolvedSavePath => ProjectSettings.GlobalizePath(SavePath);
		protected abstract string SavePath { get; }
		protected virtual bool Encrypted { get; }
		protected virtual string EncryptionPassword { get; }

		public void Save()
		{
			var json = ToJSON();

			var saveFile = new File();
			
			if (Encrypted)
			{
				if (string.IsNullOrEmpty(EncryptionPassword))
					throw new ArgumentException($"{EncryptionPassword} is invalid");

				saveFile.OpenEncryptedWithPass(SavePath, File.ModeFlags.Write, EncryptionPassword);
			}
			else
			{
				saveFile.Open(SavePath, File.ModeFlags.Write);
			}
				

			saveFile.StoreLine(json);
			saveFile.Close();
		}

		public T Load<T>() where T:SaveFile
		{
			var saveFile = new File();

			if (!saveFile.FileExists(SavePath))
				return Activator.CreateInstance<T>();

			if (Encrypted)
			{
				if (string.IsNullOrEmpty(EncryptionPassword))
					throw new ArgumentException($"{EncryptionPassword} is invalid");

				saveFile.OpenEncryptedWithPass(SavePath, File.ModeFlags.Read, EncryptionPassword);
			}
			else
			{
				saveFile.Open(SavePath, File.ModeFlags.Read);
			}

			string json = "";

			while(saveFile.GetPosition() < saveFile.GetLen())
			{
				json += saveFile.GetLine();
			}
			
			return FromJSON<T>(json);
		}

		protected abstract string ToJSON();
		protected abstract T FromJSON<T>(string json) where T:SaveFile;
	}
}