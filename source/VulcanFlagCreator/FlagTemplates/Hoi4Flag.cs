using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Windows.Media.Imaging;

using VulcanFlagCreator.Utils;

namespace VulcanFlagCreator.FlagTemplates
{
    public class Hoi4Flag : GenericFlag
    {
        public Hoi4Flag(string sourcePath, HttpClient client, string outputDirectory, string name, string suffix)
            : base(sourcePath, client, outputDirectory, name, suffix)
        {

        }

        public Hoi4Flag(string sourcePath, HttpClient client) : base(sourcePath, client)
        {

        }

        /// <summary>
        /// The width of the medium-sized flag.
        /// </summary>
        public static int MediumWidth { get; } = 41;
        /// <summary>
        /// The height of the medium-sized flag.
        /// </summary>
        public static int MediumHeight { get; } = 26;
        /// <summary>
        /// The width of the small-sized flag.
        /// </summary>
        public static int SmallWidth { get; } = 10;
        /// <summary>
        /// The height of the small-sized flag.
        /// </summary>
        public static int SmallHeight { get; } = 7;

        public new void CreateFlag()
        {
            var largeImage = ImageUtils.ResizeImage(SourceImage, Width, Height);
            largeImage.Save(GetOutputPath());

            var mediumImage = ImageUtils.ResizeImage(SourceImage, MediumWidth, MediumHeight);
            mediumImage.Save(GetOutputPath("medium"));

            var smallImage = ImageUtils.ResizeImage(SourceImage, SmallWidth, SmallHeight);
            smallImage.Save(GetOutputPath("small"));
        }

        public new Dictionary<FlagSize, BitmapSource> GetFlagBitmapSources()
        {
            return new Dictionary<FlagSize, BitmapSource>()
            {
                { FlagSize.Large, ImageUtils.BitmapToBitmapSource(ImageUtils.ResizeImage(SourceImage, Width, Height)) },
                { FlagSize.Medium, ImageUtils.BitmapToBitmapSource(ImageUtils.ResizeImage(SourceImage, MediumWidth, MediumHeight)) },
                { FlagSize.Small, ImageUtils.BitmapToBitmapSource(ImageUtils.ResizeImage(SourceImage, SmallWidth, SmallHeight)) }
            };
        }
    }
}
