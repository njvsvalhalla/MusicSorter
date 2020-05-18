using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using MusicSorterCore.Helpers;
using Serilog;
using TagLib;
using TagLib.Mpeg;

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
                    Log.Information($"Found file {file}..");
                    try
                    {
                        var newFile = GetFilePath(TagLib.File.Create(file));

                        Log.Information($"Moving to {newFile}");

                        if (!new DirectoryInfo(Path.GetDirectoryName(newFile)).Exists)
                            Directory.CreateDirectory(Path.GetDirectoryName(newFile));

                        var fileInfo = new FileInfo(file);

                        if (new FileInfo(newFile).Exists)
                        {
                            Log.Information("File exists, skipping");
                            continue;
                        }

                        fileInfo.CopyTo(newFile);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Could not parse data from {file} - or couldn't copy - {ex}");
                    }
                }
            }
        }

        private string GetFilePath(TagLib.File file)
        {
            //default %ARTIST%/[%EXT|BR%] %YEAR% - %ALBUM%/%FILE%

            var artist = GetString(file.Tag.JoinedAlbumArtists ?? file.Tag.JoinedPerformers).Replace("/","_");
            var year = file.Tag.Year;
            var album = GetString(file.Tag.Album);
            var bitRate = "";

            if (file.Name.ToLower().Contains("mp3"))
                bitRate = GetBitrate(file);

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

        private string GetString(string input) => (string.IsNullOrWhiteSpace(input) ? "UNKNOWN" : StripIllegalCharacters(input)).TrimEnd().TrimEnd('.');

        private string StripIllegalCharacters(string input)
        {
            var invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            return invalid.Aggregate(input, (current, c) => current.Replace(c.ToString(), ""));
        }

        private string GetBitrate(TagLib.File file)
        {
            if (file.Name.ToLower().Contains(".mp4"))
                return "MP4";

            var bitRate = "";
            foreach (var codec in file.Properties.Codecs)
            {
                if (codec.Description.ToLower().Contains("vbr"))
                    return "MP3-VBR";
            }
            return $"{file.Properties.AudioBitrate}";

        }

    }
}
