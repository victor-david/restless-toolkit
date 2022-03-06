using Restless.Toolkit.Core.Resources;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Restless.Toolkit.Core.Utility
{
    /// <summary>
    /// Provides static helper methods to open files and web sites.
    /// </summary>
    public static class OpenHelper
    {
        /// <summary>
        /// Opens the specified web site.
        /// </summary>
        /// <param name="browserPath">The path to the browser executable, or null/empty to use the system default</param>
        /// <param name="urlToOpen">The url of the web site to open. Adds http if needed.</param>
        public static void OpenWebSite(string browserPath, string urlToOpen)
        {
            try
            {
                string url = Format.MakeHttp(urlToOpen);
                Process process = new Process();
                if (!string.IsNullOrEmpty(browserPath))
                {
                    process.StartInfo.FileName = browserPath;
                    process.StartInfo.Arguments = url;
                }
                else
                {
                    process.StartInfo.FileName = url;
                    process.StartInfo.UseShellExecute = true;
                }
                process.Start();
            }
            catch (Exception ex)
            {
                throw new Exception(Strings.InvalidOperationCannotOpenWebSite, ex);
            }
        }

        /// <summary>
        /// Opens a file.
        /// </summary>
        /// <param name="fileName">The name of the file to open.</param>
        /// <param name="args">The arguments to the file.</param>
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> is null or empty</exception>
        /// <exception cref="Exception">The file cannot be opened.</exception>
        public static void OpenFile(string fileName, string args = null)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentNullException(nameof(fileName));
                }

                Process.Start(new ProcessStartInfo()
                {
                    FileName = fileName,
                    UseShellExecute = true,
                    Arguments = args
                });
            }

            catch (Win32Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(fileName);
                sb.AppendLine();
                sb.AppendLine(Strings.InvalidOperationCannotOpenFile);
                throw new Exception(sb.ToString(), ex);
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Opens explorer to the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public static void Explore(string path)
        {
            OpenFile("explorer.exe", string.Format(@"/e /root,{0}", path));
        }

        /// <summary>
        /// Opens explorer to the specified path and selects the specified file.
        /// </summary>
        /// <param name="fullPath">The full path that indcludes the file name.</param>
        public static void ExploreToFile(string fullPath)
        {
            OpenFile("explorer.exe", string.Format(@"/select,{0}", fullPath));
        }
    }
}