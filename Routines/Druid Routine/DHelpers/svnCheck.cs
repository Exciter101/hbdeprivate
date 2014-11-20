using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Styx.Common;
using System.Windows.Forms;

namespace Druid.DHelpers
{
    class svnCheck
    {

        private const string SvnUrl = "http://dudup.googlecode.com/svn/trunk/";
        private static string _savePathDir = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                        @"Routines\Settings\Druid\SVN\");
        private static string _savePath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                        @"Routines\Settings\Druid\SVN\revision.xml");
        private static string _downloadPath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                        @"Routines\Druid");

        private static readonly Regex LinkPattern = new Regex(@"<li><a href="".+"">(?<ln>.+(?:..))</a></li>",
            RegexOptions.CultureInvariant);

        public static string NewString;

        private static readonly Regex ChangelogPattern =
            new Regex(
                "<h4 style=\"margin-top:0\">Log message</h4>\r?\n?<pre class=\"wrap\" style=\"margin-left:1em\">(?<log>.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?)</pre>",
                RegexOptions.CultureInvariant);

        public static int CcRevision
        {
            get
            {
                int revision = 0;

                try
                {
                    bool folderExists = Directory.Exists(_savePathDir);
                    if (!folderExists)
                    {
                        Directory.CreateDirectory(_savePathDir);
                    }
                    if (!File.Exists(_savePath))
                    {
                        using (StreamWriter writer = new StreamWriter(_savePathDir + "revision.xml"))
                        {
                            writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                            writer.WriteLine("<RevInformation>");
                            writer.WriteLine("<Revision>0</Revision>");
                            writer.WriteLine("</RevInformation>");
                        }
                    }
                    
                    var reader = new FileStream(_savePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(reader);
                    XmlNodeList nodeList = xmlDocument.GetElementsByTagName("RevInformation");
                    revision = Convert.ToInt16(nodeList[0].FirstChild.ChildNodes[0].InnerText);
                }

                catch
                {
                }

                return revision;
            }

            set
            {
                var reader = new FileStream(_savePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(reader);

                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("RevInformation");
                nodeList[0].FirstChild.ChildNodes[0].InnerText = value.ToString(CultureInfo.InvariantCulture);

                var writer = new FileStream(_savePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                xmlDocument.Save(writer);
            }
        }

        public static void CheckForUpdate()
        {
            try
            {
                int revision = CcRevision;
                int onlineRevision = GetOnlineRevision();
                if (revision == 0)
                {
                    revision = onlineRevision;
                    using (StreamWriter writer = new StreamWriter(_savePathDir + "revision.xml"))
                    {
                        writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                        writer.WriteLine("<RevInformation>");
                        writer.WriteLine("<Revision>"+ onlineRevision + "</Revision>");
                        writer.WriteLine("</RevInformation>");
                    }
                }
                if (revision < onlineRevision)
                {
                    MessageBox.Show("Your Current SVN version : " + revision + "\r\n"
                        + "Latest SVN Revision : " + onlineRevision + "\r\n"
                        + "Downloading latest revision now ...", "Revision Check for Druid CombatRoutine");

                    DownloadFilesFromSvn(new WebClient(), SvnUrl);
                    CcRevision = onlineRevision;
                }
            }
            catch
            {
            }
        }

        private static int GetOnlineRevision()
        {
            var client = new WebClient();
            string html = client.DownloadString(SvnUrl);
            var pattern = new Regex(@" - Revision (?<rev>\d+):", RegexOptions.CultureInvariant);
            Match match = pattern.Match(html);
            if (match.Success && match.Groups["rev"].Success) return int.Parse(match.Groups["rev"].Value);
            throw new Exception("Unable to retreive revision!");
        }

        private static void DownloadFilesFromSvn(WebClient client, string url)
        {
            string basePath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                                           @"Routines\Druid\");
            string html = client.DownloadString(url);
            MatchCollection results = LinkPattern.Matches(html);

            IEnumerable<Match> matches = from match in results.OfType<Match>()
                                         where match.Success && match.Groups["ln"].Success
                                         select match;
            foreach (Match match in matches)
            {
                string file = RemoveXmlEscapes(match.Groups["ln"].Value);
                string newUrl = url + file;
                if (newUrl[newUrl.Length - 1] == '/') // it's a directory...
                {
                    DownloadFilesFromSvn(client, newUrl);
                }
                else // its a file.
                {
                    string filePath, dirPath;
                    if (url.Length > SvnUrl.Length)
                    {
                        string relativePath = url.Substring(SvnUrl.Length);
                        dirPath = Path.Combine(basePath, relativePath);
                        filePath = Path.Combine(dirPath, file);
                    }
                    else
                    {
                        dirPath = Environment.CurrentDirectory;
                        filePath = Path.Combine(basePath, file);
                    }
                    Logging.Write("Downloading {0}", filePath);
                    if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                    client.DownloadFile(newUrl, filePath);
                }
            }
        }

        private static string RemoveXmlEscapes(string xml)
        {
            return
                xml.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace(
                    "&apos;", "'");
        }
    }
}