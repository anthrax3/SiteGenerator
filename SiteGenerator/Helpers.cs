using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace SiteGenerator
{
    public static class Helpers
    {
        /// <summary>
        /// Transform XML from a file and return as string
        /// </summary>
        /// <param name="xct">Object</param>
        /// <param name="infilename">Path to XML file</param>
        /// <param name="arguments">Arguments for XSLT</param>
        /// <returns>Transformed XML</returns>
        public static string TransformFileToString(this XslCompiledTransform xct, string infilename, XsltArgumentList arguments)
        {
            string result = "";

            StringWriter writer = new StringWriter();
            xct.Transform(infilename, arguments, writer);
            
            result = writer.ToString();

            return result;
        }

        /// <summary>
        /// Transform XML from a file and write to file
        /// </summary>
        /// <param name="xct">Object</param>
        /// <param name="infilename">Path to XML file</param>
        /// <param name="arguments">Arguments for XSLT</param>
        /// <param name="outfilename">Path to output file</param>
        public static void TransformFileToFile(this XslCompiledTransform xct, string infilename, XsltArgumentList arguments, string outfilename)
        {
            File.WriteAllText(outfilename, xct.TransformFileToString(infilename, arguments), Encoding.UTF8);
        }

        public static void CopyDirectoryFiles(string sourcepath, string targetpath, bool recurse)
        {
            DirectoryInfo di = Directory.CreateDirectory(targetpath);

            if (Directory.Exists(sourcepath))
            {
                Console.WriteLine("Copying files from {0}\n to {1}", sourcepath, targetpath);

                var files = Directory.EnumerateFiles(sourcepath);
                foreach (string file in files)
                {
                    Console.WriteLine("{0}", Path.GetFileName(file));
                    File.Copy(file, Path.Combine(targetpath, Path.GetFileName(file)),true);
                }

                if (recurse)
                {
                    var dirs = Directory.EnumerateDirectories(sourcepath);

                    foreach (string dir in dirs)
                    {
                        CopyDirectoryFiles(dir, Path.Combine(targetpath, Path.GetDirectoryName(dir)), true);
                    }
                }
            }
        }

    }
}
