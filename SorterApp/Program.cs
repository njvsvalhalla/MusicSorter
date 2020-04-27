using System;
using System.Drawing;
using System.Threading.Tasks;
using MusicSorterCore;
using NConsoler;
using Serilog;

namespace SorterApp
{
    public class Program
    {
        private static IMusicSorter _sorter;

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Minute)
                .CreateLogger();

            _sorter = new MusicSorter();

            Log.Information("Music Sorter v0.1a");
            Log.Information("--------------------------------------");
            Consolery.Run(typeof(Program), args);
        }

        [Action]
        public static void Sort(
            [Required] string path,
            [Required] string outputPath,
            [Optional("%ARTIST%\\%YEAR% - %ALBUM%\\%FILE%")] string format,
            [Optional(false)] bool actuallyMove)
        {
            Log.Information($"Input path: {path}, output path: {outputPath}");
            Log.Information($"Actually move files? {actuallyMove}, folder format: {format}");
            _sorter.Sort(path, outputPath, format, actuallyMove);
        }
    }
}
