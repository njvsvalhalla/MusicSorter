using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using MusicSorterCore.Helpers;
using Serilog;

namespace MusicSorterCore
{
    public class MusicSorter : IMusicSorter
    {
        private string _pathFormat;
        private string _outputPath;

        public void Sort(string path, string outputPath, string format, bool actuallyMove)
        {
            //move to a constructor
            _pathFormat = format;
            _outputPath = outputPath;

            Log.Information("Beginning sort..");

            var directories = DirectoryHelper.GetAllDirectoriesRecursively(path);

            foreach (var directory in directories)
            {
                Log.Information($"Looking through directory {directory} for supported files.");
                foreach (var file in DirectoryHelper.GetAllSupportedMusicFiles(directory))
                {
                    Log.Information("Found file {file}..");
                    var newFile = GetFilePath(TagLib.File.Create(file));

                    Log.Information($"Moving to {newFile}");
                }
            }
        }

        private string GetFilePath(TagLib.File file)
        {
            //default %ARTIST%/[%EXT|BR%] %YEAR% - %ALBUM%/%FILE%

            var artist = file.Tag.JoinedAlbumArtists;
            var year = file.Tag.Year;
            var album = file.Tag.Album;
            var bitRate = file.Properties.AudioBitrate;

            var baseFormat = _pathFormat;

            if (year < 1)
                baseFormat = baseFormat.Replace("%YEAR% - ", "");

            baseFormat = baseFormat
                .Replace("%ARTIST%", artist)
                .Replace("%YEAR%", $"{year}")
                .Replace("%ALBUM%", album)
                .Replace("%FILE%", Path.GetFileName(file.Name));

            baseFormat = baseFormat
                .Replace("%EXT|BR%",
                    Path.GetExtension(file.Name).ToLower().Contains("flac") ? "FLAC" : $"{bitRate}");

            return Path.Combine(_outputPath, baseFormat);
        }

    }
}
