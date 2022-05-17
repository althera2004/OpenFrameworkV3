﻿// --------------------------------
// <copyright file="imagehelper.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    public static class ImageHelper
    {
        public const string ErrorMessage = "Could not recognize image format.";

        private static Dictionary<byte[], Func<BinaryReader, Size>> imageFormatDecoders = new Dictionary<byte[], Func<BinaryReader, Size>>()
        {
            { new byte[] { 0x42, 0x4D }, DecodeBitmap},
            { new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, DecodeGif },
            { new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }, DecodeGif },
            { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, DecodePng },
            { new byte[] { 0xff, 0xd8 }, DecodeJfif }
        };

        /// <summary>Gets the dimensions of an image.</summary>
        /// <param name="path">The path of the image to get the dimensions of.</param>
        /// <returns>The dimensions of the specified image.</returns>
        /// <exception cref="ArgumentException">The image was of an unrecognized format.</exception>
        public static Size GetDimensions(string path)
        {
            try
            {
                using (var binaryReader = new BinaryReader(File.OpenRead(path)))
                {
                    return GetDimensions(binaryReader);
                }
            }
            catch(DirectoryNotFoundException)
            {
                return new Size(1, 1);
            }
            catch (ArgumentException)
            {
                return new Size(1, 1);
            }
        }

        /// <summary>Gets the dimensions of an image.</summary>
        /// <param name="binaryReader">The path of the image to get the dimensions of.</param>
        /// <returns>The dimensions of the specified image.</returns>
        /// <exception cref="ArgumentException">The image was of an unrecognized format.</exception>    
        public static Size GetDimensions(BinaryReader binaryReader)
        {
            if (binaryReader != null)
            {
                int maxMagicBytesLength = imageFormatDecoders.Keys.OrderByDescending(x => x.Length).First().Length;
                var magicBytes = new byte[maxMagicBytesLength];
                for (int i = 0; i < maxMagicBytesLength; i += 1)
                {
                    magicBytes[i] = binaryReader.ReadByte();
                    foreach (var keyValuePair in imageFormatDecoders)
                    {
                        if (magicBytes.StartsWith(keyValuePair.Key))
                        {
                            return keyValuePair.Value(binaryReader);
                        }
                    }
                }
            }

            throw new ArgumentException(ErrorMessage, "binaryReader");
        }

        private static bool StartsWith(this byte[] thisBytes, byte[] thatBytes)
        {
            for (int i = 0; i < thatBytes.Length; i += 1)
            {
                if (thisBytes[i] != thatBytes[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static short ReadLittleEndianInt16(this BinaryReader binaryReader)
        {
            var bytes = new byte[sizeof(short)];
            for (int i = 0; i < sizeof(short); i += 1)
            {
                bytes[sizeof(short) - 1 - i] = binaryReader.ReadByte();
            }

            return BitConverter.ToInt16(bytes, 0);
        }

        private static int ReadLittleEndianInt32(this BinaryReader binaryReader)
        {
            byte[] bytes = new byte[sizeof(int)];
            for (int i = 0; i < sizeof(int); i += 1)
            {
                bytes[sizeof(int) - 1 - i] = binaryReader.ReadByte();
            }

            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>Gets the size of a bitmap image</summary>
        /// <param name="binaryReader">Binary reader of image</param>
        /// <returns>The size of the image</returns>
        private static Size DecodeBitmap(BinaryReader binaryReader)
        {
            binaryReader.ReadBytes(16);
            int width = binaryReader.ReadInt32();
            int height = binaryReader.ReadInt32();
            return new Size(width, height);
        }

        /// <summary>Gets the size of a gif image</summary>
        /// <param name="binaryReader">Binary reader of image</param>
        /// <returns>The size of the image</returns>
        private static Size DecodeGif(BinaryReader binaryReader)
        {
            int width = binaryReader.ReadInt16();
            int height = binaryReader.ReadInt16();
            return new Size(width, height);
        }

        /// <summary>Gets the size of a pgn image</summary>
        /// <param name="binaryReader">Binary reader of image</param>
        /// <returns>The size of the image</returns>
        private static Size DecodePng(BinaryReader binaryReader)
        {
            binaryReader.ReadBytes(8);
            int width = binaryReader.ReadLittleEndianInt32();
            int height = binaryReader.ReadLittleEndianInt32();
            return new Size(width, height);
        }

        /// <summary>Gets the size of a Jfif image</summary>
        /// <param name="binaryReader">Binary reader of image</param>
        /// <returns>The size of the image</returns>
        private static Size DecodeJfif(BinaryReader binaryReader)
        {
            while (binaryReader.ReadByte() == 0xff)
            {
                var marker = binaryReader.ReadByte();
                var chunkLength = binaryReader.ReadLittleEndianInt16();

                if (marker == 0xc0)
                {
                    binaryReader.ReadByte();

                    int height = binaryReader.ReadLittleEndianInt16();
                    int width = binaryReader.ReadLittleEndianInt16();
                    return new Size(width, height);
                }

                binaryReader.ReadBytes(chunkLength - 2);
            }

            throw new ArgumentException(ErrorMessage);
        }
    }
}