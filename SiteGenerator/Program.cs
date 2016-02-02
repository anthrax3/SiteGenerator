using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace SiteGenerator
{


    class Program
    {
        static void Main(string[] args)
        {

            if ((args.Length < 2) || (args.Length > 2))
            {
                Console.WriteLine("Syntax:\r\n\r\nsitegenerator source-path target-path\r\n");
                return;
            }

            string sourceDirectory = args[0];
            string websiteDirectory = args[1];

            if (!Directory.Exists(sourceDirectory))
            {
                Console.WriteLine("Error: Source path not found\n\n");
            }

            if (!Directory.Exists(websiteDirectory))
            {
                Console.WriteLine("Error: Target path not found\n\n");
            }

            Console.WriteLine("Source: {0}\nTarget: {1}", sourceDirectory, websiteDirectory);

            string sourceConfigDirectory = Path.Combine(sourceDirectory,"config");
            string sourcePageDirectory = Path.Combine(sourceDirectory,"pages");

            Helpers.CopyDirectoryFiles(Path.Combine(sourceDirectory, "css"), Path.Combine(websiteDirectory, "css"),true);
            Helpers.CopyDirectoryFiles(Path.Combine(sourceDirectory, "js"), Path.Combine(websiteDirectory, "js"),true);
            Helpers.CopyDirectoryFiles(Path.Combine(sourceDirectory, "root"), websiteDirectory,false);

            const string DATAFILENAME = "data.xml";

            // Load style sheets
            XslCompiledTransform xsltArticle = new XslCompiledTransform();

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.DtdProcessing = DtdProcessing.Parse;

            XmlReader reader = XmlReader.Create(Path.Combine(sourceConfigDirectory, "data.xslt"), readerSettings);

            XsltSettings xsltSettings = new XsltSettings();
            xsltSettings.EnableDocumentFunction = true;
            xsltSettings.EnableScript = true;

            xsltArticle.Load(reader, xsltSettings, new XmlUrlResolver());

            XslCompiledTransform xsltNavigation = new XslCompiledTransform();
            xsltNavigation.Load(Path.Combine(sourceConfigDirectory, "navigation.xslt"));

            string navigationHtml = "";

            try
            {
                var articles = Directory.EnumerateDirectories(sourcePageDirectory);

                foreach (string currentDir in articles)
                {
                    Console.WriteLine("Processing {0}", currentDir);

                    string currentArticle = Path.GetFileName(currentDir);

                    string sourceArticleDirectory = Path.Combine(sourcePageDirectory, currentArticle);
                    string websiteArticleDirectory = Path.Combine(websiteDirectory, "pages", currentArticle);

                    DirectoryInfo di = Directory.CreateDirectory(websiteArticleDirectory);

                    string infilename = Path.Combine(sourceArticleDirectory, DATAFILENAME);

                    int numberOfPages = 1;


                    XmlDocument xmlfile = new XmlDocument();
                    xmlfile.Load(infilename);
                    numberOfPages = xmlfile.SelectNodes("//page").Count;
                    
                    XsltArgumentList argsListNavigation = new XsltArgumentList();
                    argsListNavigation.AddParam("selectedArticleName", "", currentArticle);

                    navigationHtml = xsltNavigation.TransformFileToString(Path.Combine(sourceConfigDirectory, "navigation.xml"), argsListNavigation);


                    for (int currentPageno = 1; currentPageno < (numberOfPages + 1); currentPageno++)
                    {
                        string outfilename;
                        outfilename = Path.Combine(websiteArticleDirectory, "page_" + currentPageno.ToString() + ".html");

                        XsltArgumentList argsListData = new XsltArgumentList();
                        argsListData.AddParam("selectedPageNo", "", currentPageno.ToString());
                        argsListData.AddParam("navigationContent", "", navigationHtml);

                        xsltArticle.TransformFileToFile(infilename, argsListData, outfilename);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
