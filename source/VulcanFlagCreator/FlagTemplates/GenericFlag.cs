using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Windows.Media.Imaging;

using VulcanFlagCreator.Utils;

namespace VulcanFlagCreator.FlagTemplates
{
    public class GenericFlag
    {
        public GenericFlag() { } // For inheritance

        public GenericFlag(string sourcePath, HttpClient client, string outputDirectory, string name, string suffix) : this(sourcePath, client)
        {
            OutputDirectory = Directory.CreateDirectory(outputDirectory);
            Name = name;
            Suffix = suffix;
        }

        public GenericFlag(string sourcePath, HttpClient client)
        {
            if (File.Exists(sourcePath))
            {
                SourceImage = Image.FromFile(sourcePath);
            }

            if (Uri.IsWellFormedUriString(sourcePath, UriKind.Absolute))
            {
                HttpRequestMessage request = new(HttpMethod.Get, new Uri(sourcePath));
                var response = client.Send(request);
                SourceImage = Image.FromStream(response.Content.ReadAsStream());
            }
        }

        /// <summary>
        /// The image from which to base on.
        /// </summary>
        public Image SourceImage { get; set; }
        /// <summary>
        /// The directory in which the .tga files will be saved.
        /// </summary>
        public DirectoryInfo OutputDirectory { get; set; }
        /// <summary>
        /// The base name of the files.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The optional suffix of the file names.
        /// </summary>
        public string Suffix { get; set; }
        /// <summary>
        /// The width of the final flag.
        /// </summary>
        public static int Width { get; } = 93;
        /// <summary>
        /// The height of the final flag.
        /// </summary>
        public static int Height { get; } = 64;

        protected string GetOutputPath(string subDirectoriesPath = "")
        {
            var directory = Directory.CreateDirectory(Path.Combine(OutputDirectory.FullName, subDirectoriesPath));
            return Path.Combine(directory.FullName, Name + Suffix + ".tga");
        }

        /// <summary>
        /// Saves the flag to a path returned by <see cref="GetOutputPath(string)"/>
        /// </summary>
        public void CreateFlag()
        {
            var resizedImage = ImageUtils.ResizeImage(SourceImage, Width, Height);
            resizedImage.Save(GetOutputPath());
        }

        public Dictionary<FlagSize, BitmapSource> GetFlagBitmapSources()
        {
            return new Dictionary<FlagSize, BitmapSource>()
            {
                { FlagSize.Large, ImageUtils.BitmapToBitmapSource(ImageUtils.ResizeImage(SourceImage, Width, Height)) }
            };
        }
    }

    public enum FlagSize
    {
        Large,
        Medium,
        Small
    }
}