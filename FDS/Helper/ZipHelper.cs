using FDS.Data;
using System.IO;
using System.IO.Compression;

public static class ZipHelper
{
    public static byte[] CreateZipFile(string fileName, byte[] fileContent)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var entry = archive.CreateEntry($"{fileName}.pdf", CompressionLevel.Fastest);
                using (var entryStream = entry.Open())
                {
                    entryStream.Write(fileContent, 0, fileContent.Length);
                }
            }

            return memoryStream.ToArray();
        }
    }
    public static byte[] CreateZipFileFromDocuments(List<Document> documents, string zipFileName)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var document in documents.Where(d => d.DocFile != null && d.DocFile.Length > 0))
                {
                    var entry = archive.CreateEntry($"{document.Name}.pdf", CompressionLevel.Fastest);
                    using (var entryStream = entry.Open())
                    {
                        entryStream.Write(document.DocFile, 0, document.DocFile.Length);
                    }
                }
            }

            return memoryStream.ToArray();
        }
    }
}
