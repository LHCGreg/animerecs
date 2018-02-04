using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace AnimeRecs.UpdateStreams.UnitTests
{
    static class Helpers
    {
        private static string GetResourceName(string fileName)
        {
            return "AnimeRecs.UpdateStreams.UnitTests." + fileName;
        }

        public static StreamReader GetResourceStream(string fileName)
        {
            string resourceName = GetResourceName(fileName);
            return new StreamReader(typeof(Helpers).GetTypeInfo().Assembly.GetManifestResourceStream(resourceName), Encoding.UTF8);
        }

        public static string GetResourceText(string fileName)
        {
            using (StreamReader resourceStream = GetResourceStream(fileName))
            {
                return resourceStream.ReadToEnd();
            }
        }
    }
}
