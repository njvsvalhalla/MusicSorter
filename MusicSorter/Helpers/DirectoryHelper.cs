using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Serilog;
using Serilog.Core;

namespace MusicSorterCore.Helpers
{
    public static class DirectoryHelper
    {
        public static List<string> GetAllDirectoriesRecursively(string startingPath)
        {
            var directoryList = new List<string>();
            foreach (var directory in Directory.EnumerateDirectories(startingPath))
            {
                try
                {
                    directoryList.Add(directory);
                    directoryList.AddRange(GetAllDirectoriesRecursively(directory));
                }
                catch
                {
                    Log.Error($"Couldn't get information for directory {directory}. Check permissions on the folder.");
                }
            }

            return directoryList;
        }

        public static List<string> GetAllSupportedMusicFiles(string path)
        {
            var fileList = new List<string>();

            //TODO add this in the actual config file as a CDL
            try
            {
                fileList.AddRange(Directory.EnumerateFiles(path, "*.mp3"));
                fileList.AddRange(Directory.EnumerateFiles(path, "*.flac"));
                fileList.AddRange(Directory.EnumerateFiles(path, "*.mp4"));
            }
            catch (Exception ex)
            {
                Log.Information($"Cannot get file list for {path}");
            }

            return fileList;
        }
    }
}
