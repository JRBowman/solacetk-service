using Microsoft.AspNetCore.Http;
using SolaceTK.Core.Models.Files;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SolaceTK.Core.Services
{
    public static class FileService
    {
        #region Directory Methods

        /// <summary>
        /// Establish a Directory, Creates the Directory by Full Path if it doesn't exist.
        /// </summary>
        /// <param name="paths">Path Params as Strings to build the full directory path.</param>
        public static void EstablishDirectory(params string[] paths) 
        {
            var fullPath = string.Join("/", paths);
            if (!DirectoryExists(fullPath)) CreateDirectory(fullPath);
        }

        /// <summary>
        /// Create a Directory.
        /// </summary>
        /// <param name="fullPath">Full Path of the Directory to Create.</param>
        public static void CreateDirectory(string fullPath)
        {
            Directory.CreateDirectory(fullPath);
        }

        /// <summary>
        /// Check if the Directory Exists.
        /// </summary>
        /// <param name="paths">Path Params as Strings to build the full path.</param>
        /// <returns>Returns true/false whether the directory exists.</returns>
        public static bool DirectoryExists(params string[] paths)
        {
            var fullPath = string.Join("/", paths);
            return DirectoryExists(fullPath);
        }

        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

       /// <summary>
       /// Delete a Directory.
       /// </summary>
       /// <param name="path">Path to delete.</param>
       /// <param name="recursive">Defaults: true, Deletes all sub-directories.</param>
       /// <returns></returns>
        public static bool DeleteDirectory(string path, bool recursive = true)
        {
            try
            {
                Directory.Delete(path, recursive);
            } 
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                throw;
            }

            return true;
        }

        #endregion


        #region File Methods

        // Establish File:
        public static async Task<AseFile> EstablishFile(IFormFile file, params string[] paths)
        {
            var fullPath = string.Join("/", paths);
            EstablishDirectory(fullPath);

            return await CreateFile(file, fullPath);
        }

        // Create File:
        public static async Task<AseFile> CreateFile(IFormFile file, string fullPath)
        {
            using var stream = File.OpenWrite($"{fullPath}/{file.Name}");

            await file.CopyToAsync(stream);

            await stream.FlushAsync();
            stream.Close();

            return new AseFile { Name = file.Name.Split('.')[0], Directory = fullPath };
        }

        // Check File:
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        // Delete File:
        public static bool DeleteFile(string fileName, string fullPath)
        {
            try
            {
                File.Delete($"{fullPath}/{fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return true;
        }

        #endregion
    }
}
