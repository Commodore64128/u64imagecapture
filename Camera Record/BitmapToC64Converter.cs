using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public class BitmapToC64Converter
{
    private const int Width = 320;
    private const int Height = 200;

    public byte[] ConvertToC64Hires(Bitmap bitmap) //, string outputFilePath)
    {

        if (bitmap.Width != Width || bitmap.Height != Height || bitmap.PixelFormat != PixelFormat.Format1bppIndexed)
        {
            throw new InvalidOperationException("Invalid bitmap format. Expected 320x200 with 1-bit depth.");
        }

        byte[] c64Data = new byte[Width * Height / 8];

        for (int y = 0; y < Height; y += 8)
        {
            for (int x = 0; x < Width; x += 8)
            {
                byte[] byteBlock = ConvertBlockToByte(bitmap, x, y);
                int startIndex = (y / 8) * Width / 8 + (x / 8);
                Array.Copy(byteBlock, 0, c64Data, startIndex * 8, byteBlock.Length);
            }
        }

        return c64Data;
        //File.WriteAllBytes(outputFilePath, c64Data);
    }

    private byte[] ConvertBlockToByte(Bitmap bitmap, int startX, int startY)
    {
        byte[] byteBlock = new byte[8];

        for (int y = 0; y < 8; y++)
        {
            //for (int x = 0; x < 8; x++)
            //{
            //    Color pixelColor = bitmap.GetPixel(startX + x, startY + y);
            //    if (pixelColor.R == 0) // Assuming black pixel represents 'on' (1)
            //    {
            //        byteBlock[y] |= (byte)(1 << x); // Reversed bit order
            //    }
            //}

            for (int x = 0; x < 8; x++)
            {
                if (bitmap.GetPixel(startX + x, startY + y).R == 0)
                {
                    byteBlock[y] |= (byte)(1 << (7 - x));
                }
            }
        }

        return byteBlock;
    }
}