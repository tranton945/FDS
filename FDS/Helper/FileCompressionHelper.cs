using System.IO.Compression;
using System.IO;

namespace FDS.Helper
{
    public class FileCompressionHelper
    {
        //public static byte[] CompressFile(byte[] fileData, string zipFileName)
        //{
        //    using (MemoryStream compressedStream = new MemoryStream())
        //    {
        //        using (ZipArchive archive = new ZipArchive(compressedStream, ZipArchiveMode.Create, true))
        //        {
        //            var entry = archive.CreateEntry(zipFileName);
        //            using (var entryStream = entry.Open())
        //            {
        //                entryStream.Write(fileData, 0, fileData.Length);
        //            }
        //        }
        //        return compressedStream.ToArray();
        //    }
        //}
        public static byte[] CompressFiles(Dictionary<string, byte[]> files)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var entry = archive.CreateEntry(file.Key, CompressionLevel.Fastest);
                        using (var entryStream = entry.Open())
                        {
                            entryStream.Write(file.Value, 0, file.Value.Length);
                        }
                    }
                }

                return memoryStream.ToArray();
            }
        }
    }
}
