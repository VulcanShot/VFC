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
        protected GenericFlag() { } // For inheritance

        public GenericFlag(string sourcePath, HttpClient client)
        {
            if (File.Exists(sourcePath))
            {
                SourceImage = Image.FromFile(sourcePath);
                return;
            }

            // Validity of the URI has already been checked
            HttpRequestMessage request = new(HttpMethod.Get, new Uri(sourcePath));
            var response = client.Send(request);
            SourceImage = Image.FromStream(response.Content.ReadAsStream());
        }

        public GenericFlag(string sourcePath, HttpClient client, string outputDirectory, string name, string suffix)
            : this(sourcePath, client)
        {
            OutputDirectory = Directory.CreateDirectory(outputDirectory);
            Name = name;
            Suffix = suffix;
        }

        /// <summary>
        /// The image from which to base on.
        /// </summary>
        protected Image SourceImage { get; set; }
        /// <summary>
        /// The directory in which the .tga files will be saved.
        /// </summary>
        protected DirectoryInfo OutputDirectory { get; set; }
        /// <summary>
        /// The base name of the files.
        /// </summary>
        protected string Name { get; set; }
        /// <summary>
        /// The optional suffix of the file names.
        /// </summary>
        protected string Suffix { get; set; }
        /// <summary>
        /// The width of the final flag.
        /// </summary>
        protected static int Width { get; } = 93;
        /// <summary>
        /// The height of the final flag.
        /// </summary>
        protected static int Height { get; } = 64;

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
                {
                    FlagSize.Large,
                    ImageUtils.BitmapToBitmapSource(ImageUtils.ResizeImage(SourceImage, Width, Height))
                }
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