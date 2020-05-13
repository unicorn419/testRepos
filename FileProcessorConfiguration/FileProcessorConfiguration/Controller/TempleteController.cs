using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity.Infrastructure;
using FileProcessEntityLibrary.Data;
using FileProcessEntityLibrary.Data.Models.FileProcess;
using FileProcessEntityLibrary.Data.Models.FileProcess.Lookup;

namespace FileProcessorConfiguration
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempleteController : Controller
    {

        private readonly FileProcessContext _context;

        public TempleteController(FileProcessContext context)
        {
            _context = context;
        }

        // GET: api/Templete
        [HttpGet]
        public IEnumerable<string> Get(string clientId,string fileExtension)
        {

            
            if(string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(fileExtension))
                return System.IO.File.ReadLines(System.Environment.CurrentDirectory + "\\wwwroot\\FileParserConfig.xml");



            IQueryable query = _context.LkpFileProcessConfigurations.Where(fpc => fpc.ClientId == int.Parse(clientId) && fpc.FileExtension == fileExtension);
            LkpFileProcessConfiguration lfpc = null;
            foreach (LkpFileProcessConfiguration c in query)
            {
                lfpc = c;
                break;
            }
            if (lfpc == null)
                return System.IO.File.ReadLines(System.Environment.CurrentDirectory + "\\wwwroot\\FileParserConfig.xml");



            return convertToLines(processXml(lfpc.ParserConfig,false));
        }


        // POST: api/Templete
        [HttpPost]
        public void Post()
        {

            LkpFileProcessConfiguration lfpc = null;
            int clicentId = int.Parse(Request.Form["clientId"]);
            string fileExtersion = Request.Form["fileExtension"];

            IQueryable query = _context.LkpFileProcessConfigurations.Where(c => c.ClientId == clicentId && c.FileExtension == fileExtersion);
            foreach (LkpFileProcessConfiguration c in query)
            {
                lfpc = c;
                break;
            }
            if (lfpc == null) lfpc = new LkpFileProcessConfiguration();

            lfpc.ClientId = clicentId;
            lfpc.FileExtension = fileExtersion;
            //lfpc.fileEncoding= Request.Form["fileExtension"];
            lfpc.ParserAssemble = Request.Form["parserAssemble"];
            lfpc.ParserClassName = Request.Form["parserClassName"];
            lfpc.ParserConfig = processXml(Request.Form["parserConfig"],true);
            lfpc.ParserSchema = System.IO.File.ReadAllText(System.Environment.CurrentDirectory + "\\wwwroot\\FileParserConfig.xsd");
            lfpc.ProcessorAssemble = Request.Form["processAssemble"];
            lfpc.ProcessorClassName = Request.Form["processClassName"];
            lfpc.ProcessConfig = System.IO.File.ReadAllText(System.Environment.CurrentDirectory + "\\wwwroot\\FileProcessConfig.xml");
            lfpc.ProcessSchema = System.IO.File.ReadAllText(System.Environment.CurrentDirectory + "\\wwwroot\\FileProcessConfig.xsd");

            if (lfpc.LkpFileProcessConfigurationId > 0)
            {
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry ee=  _context.LkpFileProcessConfigurations.Attach(lfpc);
                ee.State = EntityState.Modified;

            }
            else
            {
                _context.LkpFileProcessConfigurations.Add(lfpc);
            }


            _context.SaveChanges();



        }




        /// <summary>
        /// recovery the xml releate path to absolute path or 
        /// recovery the xml absolute path to releate path
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private string  processXml(string xml,bool isRecovery)
        {
            xml = xml.Replace("\"?\"", "\"\"");
            List<string> list = new List<string>();
            XmlDocument x = new XmlDocument();
            x.LoadXml(xml);

            foreach (XmlNode node in x.ChildNodes)
            {
                Queue<XmlNode> queue = new Queue<XmlNode>();
                queue.Enqueue(node);
                while (queue.Count > 0)
                {
                    XmlNode node1 = queue.Dequeue();
                    if (isRecovery)
                        replaseXpath(ref node1);
                    else
                        findAndSetXPath(ref node1);

                    foreach (XmlNode n in node1.ChildNodes)
                    {
                        queue.Enqueue(n);
                    }
                }
            }
            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter))
            {
                x.WriteTo(xmlWriter);
                xmlWriter.Flush();
                return stringWriter.ToString();
                
            }
        }

        private List<string> convertToLines(string strXml)
        {
            List<string> list = new List<string>();
            while (strXml.IndexOf(">") > 0)
            {
                list.Add(strXml.Substring(0, strXml.IndexOf(">") + 1));
                strXml = strXml.Substring(strXml.IndexOf(">") + 1);
            }

            return list;
        }

        private void findAndSetXPath(ref XmlNode x)
        {
            XmlNode tmp = x.SelectSingleNode("@source");
            if (tmp != null)
            {
                string attr = tmp.Value;
                if (string.IsNullOrEmpty(attr))
                    return;
                else if (attr.StartsWith("//"))
                    return;
                else
                {
                    XmlNode parent = x.ParentNode;
                    while (parent != null)
                    {
                        XmlNode source= parent.SelectSingleNode("@source");
                        parent = parent.ParentNode;
                        if (source != null)
                        {
                            attr = source.Value;
                            if (string.IsNullOrEmpty(attr)) continue;
                            else if (attr.StartsWith("//"))
                            {
                                x.Attributes["source"].Value = attr + "/" + x.Attributes["source"].Value;
                                break;
                            }
                            else
                            {
                                x.Attributes["source"].Value = attr + "/" + x.Attributes["source"].Value;
                                continue;
                            }
                        }
                    }
                }
            }
            else
            {
                return;
            }


        }


        private void replaseXpath(ref XmlNode x)
        {
            XmlNode tmp = x.SelectSingleNode("@source");
            if (tmp != null)
            {
                string attr = tmp.Value;
                if (string.IsNullOrEmpty(attr))
                    return;
                else if (attr.StartsWith("//"))
                {
                    if (attr.IndexOf("List/Debtor-Individual") >0)
                    {
                        Console.Write("asdfasdf");
                    }
                    XmlNode parent = x.ParentNode;
                    while (parent != null)
                    {
                        XmlNode source = parent.SelectSingleNode("@source");
                        parent = parent.ParentNode;
                        if (source != null)
                        {
                            string attrParent = source.Value;
                            //if (attr.IndexOf(attrParent) > 0)
                           // {
                                attr = attr.Replace(attrParent + "/", "");
                                x.Attributes["source"].Value = attr;
                           // }
                        }
                    }
                }
            }
            else
            {
                return;
            }

        }


    }
}