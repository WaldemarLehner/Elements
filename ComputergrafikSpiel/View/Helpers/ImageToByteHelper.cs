using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ComputergrafikSpiel.View.Helpers
{
    internal static class ImageToByteHelper
    {
        internal static byte[] ImageToByteArray(string filePath, (int width, int height) dimensions)
        {
            _ = filePath ?? throw new ArgumentNullException(nameof(filePath));
            Image<Rgba32> image = (Image<Rgba32>)Image.Load(filePath);

            if (dimensions.width != image.Width)
            {
                throw new ArgumentException("File-Width  does not match ITexture-Width");
            }

            if (dimensions.height != image.Height)
            {
                throw new ArgumentException("File-Height  does not match ITexture-Height");
            }

            // Sixlabors.ImageSharp fängt mit dem Laden der Pixel oben links ab, OpenTK jedoch unten links, spiegel
            // das Bild entlang y um dies zu korregieren.
            image.Mutate(img => img.Flip(FlipMode.Vertical));

            var pixelCount = image.Height * image.Width;
            return GenerateByteArrayFromPixelSpan(image.GetPixelSpan());
        }

        private static byte[] GenerateByteArrayFromPixelSpan(Span<Rgba32> span)
        {
            byte[] pixelArray = new byte[span.Length * 4];
            for (int i = 0; i < span.Length; i++)
            {
                pixelArray[(4 * i) + 0] = span[i].R;
                pixelArray[(4 * i) + 1] = span[i].G;
                pixelArray[(4 * i) + 2] = span[i].B;
                pixelArray[(4 * i) + 3] = span[i].A;
            }

            return pixelArray;
        }
    }
}
