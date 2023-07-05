using System.IO.Compression;

namespace UnitTests.Helpers
{
    public static class ZipFileFixtureGetter
    {
        private const string Path = "\\Fixture\\ZipFiles\\";
        public static Stream GetZipFile(string fileName)
        {
            var path = Directory.GetCurrentDirectory() + Path + fileName;
            
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public static Stream GetAnyFile()
        {
            var path = Directory.GetCurrentDirectory() + Path + ZipFileReferenceConstants.InvalidZipFileImagesFormat;

            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public static string GetFullPath(string fileName) => $"{Directory.GetCurrentDirectory() + Path + fileName}";
    }
}
