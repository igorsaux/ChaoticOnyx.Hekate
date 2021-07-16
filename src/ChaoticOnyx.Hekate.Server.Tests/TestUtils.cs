using System.Collections.Generic;
using System.IO;
using ChaoticOnyx.Hekate.Server.Services.Files;
using ChaoticOnyx.Hekate.Server.Services.Language;

namespace ChaoticOnyx.Hekate.Server.Tests
{
    internal static class TestUtils
    {
        private static List<FileInfo> s_files = new();

        public static List<FileInfo> CreateTestEnvironment()
        {
            List<FileInfo> files = new();

            File.WriteAllText("env.dme", @"#include ""test.dm""
#include ""test2.dm""");

            files.Add(new FileInfo("env.dme"));
            File.WriteAllText("test.dm", @"var/test_var = 0");
            files.Add(new FileInfo("test.dm"));

            File.WriteAllText("test2.dm", @"#include ""test3.dm""
var/another_var = 555");

            files.Add(new FileInfo("test2.dm"));
            File.WriteAllText("test3.dm", "#define DEBUG");
            files.Add(new FileInfo("test3.dm"));
            s_files = files;

            return files;
        }

        public static void DeleteTestEnvironment()
        {
            s_files.ForEach(f => f.Delete());
        }

        public static (IFileProvider, IDmLanguageService, IDmEnvironmentService) ProvideServices()
        {
            IFileProvider         fileProvider       = new FileProvider();
            IDmLanguageService    languageService    = new DmLanguageService(fileProvider);
            IDmEnvironmentService environmentService = new DmEnvironmentService(languageService);

            return (fileProvider, languageService, environmentService);
        }
    }
}
