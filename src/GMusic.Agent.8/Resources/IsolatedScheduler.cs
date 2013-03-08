﻿using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using GMusic.API;
using Newtonsoft.Json;

namespace GMusic.Agent._8.Resources
{
    public class IsolatedScheduler : IStorage
	{
        public IsolatedScheduler()
		{
			Load();
		}

        public Manager BackgroundManager;
        private const string FileName = "BackgroundManager.gmd";

        public class Manager
        {
            public Manager()
            {
                NowPlaying = new List<Models.GoogleMusicSong>();
                NowPlayingIndex = 0;
            }

            public IList<Models.GoogleMusicSong> NowPlaying { get; set; }

            public int NowPlayingIndex { get; set; }

            public bool IsPlaying { get; set; }
            public bool IsWaitingForSongUrl { get; set; }
        }

        #region I/O
        public void Load()
        {
            string output;

            using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var isolatedStorageFileStream = isolatedStorageFile.OpenFile(FileName, FileMode.Open))
                {
                    using (var streamReader = new StreamReader(isolatedStorageFileStream))
                    {
                        output = streamReader.ReadToEnd();
                    }
                }
            }

            BackgroundManager = JsonConvert.DeserializeObject<Manager>(output);
        }
        public void Save()
        {
            var content = JsonConvert.SerializeObject(BackgroundManager);

            using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var isolatedStorageFileStream = isolatedStorageFile.CreateFile(FileName))
                {
                    using (var streamWriter = new StreamWriter(isolatedStorageFileStream))
                    {
                        streamWriter.Write(content);
                    }
                }
            }
        }
        #endregion
    }
}