using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValveServerPicker.Helpers
{
    public static class SteamGameFinder
    {
        public static async Task<string> FindExeAsync(string gameFolderName, string exeFileName)
        {
            List<string> searchPaths = GetAllDrivePaths();
            string exePath = await FindExeInPathsAsync(searchPaths, gameFolderName, exeFileName);

            return exePath;
        }

        private static List<string> GetAllDrivePaths()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            return drives.Select(drive => drive.RootDirectory.FullName).ToList();
        }

        private static async Task<string> FindExeInPathsAsync(List<string> searchPaths, string gameFolderName, string exeFileName)
        {
            foreach (var path in searchPaths)
            {
                string[] possibleSteamPaths = {
                Path.Combine(path, "Program Files (x86)", "Steam", "steamapps", "common", gameFolderName),
                Path.Combine(path, "SteamLibrary", "steamapps", "common", gameFolderName)
            };

                foreach (var steamPath in possibleSteamPaths)
                {
                    string exePath = Path.Combine(steamPath, exeFileName);

                    if (Directory.Exists(steamPath) && File.Exists(exePath))
                    {
                        return exePath;
                    }
                }
            }

            return null;
        }
    }
}
