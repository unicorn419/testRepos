using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Text;
using FileProcessEntityLibrary.Data.Models.Middleware;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FileProcessEntityLibrary.Data.Models.Middleware;
using FileProcessEntityLibrary.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace configNewDB.Controllers
{
    [Route("api/[controller]")]
    public class DataItemController : Controller
    {
        private string[] listField = new string[] { "Id", "IsCurrentData", "CreatedDateTime", "UpdatedDateTime"};
        private MiddlewareContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public DataItemController(MiddlewareContext context, IHostingEnvironment hostingEnvironment)
        { _context = context; _hostingEnvironment = hostingEnvironment; }

        [HttpGet("[action]")]
        public ConfigurationModel GetTemplate(int clientInfoId,string fileExtension)
        {
            ConfigurationModel configurationModel;
            ClientFileParserInfo fp = _context.ClientFileParserInfos.Where(c => c.ClientInfoId == clientInfoId && c.FileExtension == fileExtension).FirstOrDefault();
            if (fp == null)
            {
                if (string.Equals(fileExtension, ".uif", StringComparison.OrdinalIgnoreCase))
                {
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string newPath = Path.Combine(webRootPath, "UifFileParserConfig.xml");
                    configurationModel = ConfigurationModel.LoadFromXml(System.IO.File.ReadAllText(newPath));
                }
                else
                    configurationModel = new ConfigurationModel();
                configurationModel.ClientInfoId = clientInfoId;
                configurationModel.FileExtension = fileExtension;

            }
            else
            {
                configurationModel = ConfigurationModel.LoadFromXml(processXml(fp.ParserConfig,false));
                configurationModel.FileEncoding = fp.FileEncoding;
                configurationModel.FileExtension = fp.FileExtension;
                configurationModel.ParserClassName = fp.ParserClassName;
                configurationModel.IsEnabled = fp.IsEnabled;
                configurationModel.ClientInfoId = clientInfoId;

            }

            return configurationModel;
            
        }

        [HttpGet("[action]")]
        public IEnumerable<ServiceModel> GetSubServices()
        {
            List<ServiceModel> list = new List<ServiceModel>();
            
            //individal
            ServiceModel serviceModel = ConfigurationModel.GetServiceByType(typeof(RegistrationDebtor));
            serviceModel.ServiceType = "RegistrationDebtor";
            serviceModel.Name = "RegistrationDebtor";
            list.Add(serviceModel);

            serviceModel = ConfigurationModel.GetServiceByType(typeof(RegistrationDealer));
            serviceModel.ServiceType = "RegistrationDealer";
            serviceModel.Name = "RegistrationDealer";
            list.Add(serviceModel);

            serviceModel = ConfigurationModel.GetServiceByType(typeof(RegistrationSecuredParty));
            serviceModel.ServiceType = "RegistrationSecuredParty";
            serviceModel.Name = "RegistrationSecuredParty";
            list.Add(serviceModel);

            serviceModel = ConfigurationModel.GetServiceByType(typeof(RegistrationSerialCollateral));
            serviceModel.ServiceType = "RegistrationSerialCollateral";
            serviceModel.Name = "RegistrationSerialCollateral";
            list.Add(serviceModel);

            serviceModel = ConfigurationModel.GetServiceByType(typeof(RegistrationGeneralCollateral));
            serviceModel.ServiceType = "RegistrationGeneralCollateral";
            serviceModel.Name = "RegistrationGeneralCollateral";
            list.Add(serviceModel);

            serviceModel = ConfigurationModel.GetServiceByType(typeof(RegistrationBillingInfo));
            serviceModel.ServiceType = "RegistrationBillingInfo";
            serviceModel.Name = "RegistrationBillingInfo";
            list.Add(serviceModel);

            serviceModel = ConfigurationModel.GetServiceByType(typeof(RegistrationJurisdictionSpecificInfo));
            serviceModel.ServiceType = "RegistrationJurisdictionSpecificInfo";
            serviceModel.Name = "RegistrationJurisdictionSpecificInfo";
            list.Add(serviceModel);



            return list;

        }

        [HttpPost("[action]")]
        public IActionResult UpdateConfiguration([FromBody]ConfigurationModel configurationModel)
        {

            ClientFileParserInfo fp = _context.ClientFileParserInfos.Where(c => c.ClientInfoId == configurationModel.ClientInfoId && c.FileExtension == configurationModel.FileExtension).FirstOrDefault();
            if (fp == null)
            {
                fp = new ClientFileParserInfo();
                fp.ClientInfoId = configurationModel.ClientInfoId;
                fp.IsEnabled = configurationModel.IsEnabled;
                fp.FileExtension = configurationModel.FileExtension;
                fp.FileEncoding = configurationModel.FileEncoding;
                fp.ParserAssembly = configurationModel.ParserAssembly;
                fp.ParserClassName = configurationModel.ParserClassName;
                fp.ParserConfig = processXml( configurationModel.ToXML(),true);
                
                fp.CreatedDateTime = DateTime.Now;
                fp.LastUpdatedBy = "";
                _context.Add(fp);
                _context.SaveChanges();
            }
            else
            {
                fp.IsEnabled = configurationModel.IsEnabled;
                fp.FileExtension = configurationModel.FileExtension;
                fp.FileEncoding = configurationModel.FileEncoding;
                fp.ParserAssembly = configurationModel.ParserAssembly;
                fp.ParserClassName = configurationModel.ParserClassName;
                fp.ParserConfig = processXml(configurationModel.ToXML(),true);
                fp.UpdatedDateTime = DateTime.Now;
                _context.Update(fp);
                _context.SaveChanges();

            }
            return Json(configurationModel);

        }

        [HttpGet("[action]")]
        public IEnumerable<ConfigDataItemModel> GetUIFElement(string datafile)
        {
            List<ConfigDataItemModel> list = new List<ConfigDataItemModel>();
            list.Add(new ConfigDataItemModel());
            if (string.IsNullOrEmpty(datafile)) return list;

            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName, datafile);

            char chrDelimiter = '^';
            string batchNumber = string.Empty;
            string batchDate = string.Empty;
            string errorMsg = string.Empty;
            ConfigDataItemModel cdim;
            string[] sources = { "SH0:SF0", "SH1:SF1",  "SH2:SF2", "DB1:", "SP1:", "SC1:" };

            foreach (string s in sources)
            {
                cdim = new ConfigDataItemModel();
                cdim.XPath =s;
                cdim.IsList = true;
                cdim.IsValue = false;
                list.Add(cdim);
            }


            string[] arrLines = System.IO.File.ReadAllLines(newPath).ToArray();
            if (arrLines.Length > 0) chrDelimiter = arrLines[0][3];
            string[]arrData = arrLines.Select(p => p.Split(chrDelimiter)).Select(p=>p[0]).Distinct().ToArray();
            foreach (string s in arrData)
            {
                cdim = new ConfigDataItemModel();
                cdim.Name = s;
                cdim.IsValue = true;
                cdim.IsList = false;
                cdim.XPath = s+"#";
                list.Add(cdim);
            }
            //var query = arrData.Select((item, index) => new { item, index });

            list.Sort();
            return list;

        }

        [HttpGet("[action]")]
        public IEnumerable<ConfigDataItemModel> GetXMLElement(string datafile)
        {
            XmlDocument doc = new XmlDocument();
            List<ConfigDataItemModel> list = new List<ConfigDataItemModel>();
            list.Add(new ConfigDataItemModel());
            if (string.IsNullOrEmpty(datafile)) return list;

            //string filepath = Environment.CurrentDirectory  HttpContext.Current.Server.MapPath(@"~/Upload/"+ datafile);
            //if (!System.IO.File.Exists(filepath)) return list;
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName,datafile);

            doc.Load(newPath);
            System.IO.File.Delete(newPath);
            Dictionary<string, ConfigDataItemModel> dic = new Dictionary<string, ConfigDataItemModel>();

            foreach (XmlNode n in doc.DocumentElement.ChildNodes)
            {
                getObjects(n, ref dic);
            }

            foreach (ConfigDataItemModel x in dic.Values)
            {
                list.Add(x);
            }
            list.Sort();
            return list;

        }

        protected string processXml(string xml, bool isRecovery)
        {
            xml = System.Web.HttpUtility.HtmlDecode(xml);
            //xml = xml.Replace("\"?\"", "\"\"");
            //replace attribute search partern
            
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
                        XmlNode source = parent.SelectSingleNode("@source");
                        parent = parent.ParentNode;
                        if (source != null)
                        {
                            attr = source.Value;
                            //remove attribute 
                            while (attr.IndexOf("[") > 0)
                            {
                                int startIndex = attr.IndexOf('[');
                                int endIndex = attr.IndexOf(']');
                                string attrSearch = attr.Substring(startIndex, endIndex - startIndex + 1);
                                attr = attr.Replace(attrSearch, "");
                            }

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
                    if (attr.IndexOf("List/Debtor-Individual") > 0)
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


                            //remove attribute 
                            while (attrParent.IndexOf("[") > 0)
                            {
                                int startIndex = attrParent.IndexOf('[');
                                int endIndex = attrParent.IndexOf(']');
                                string attrSearch = attrParent.Substring(startIndex, endIndex - startIndex + 1);
                                attrParent = attrParent.Replace(attrSearch, "");
                            }
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

        private void getObjects(XmlNode node, ref Dictionary<string, ConfigDataItemModel> dic)
        {
            Queue<XmlNode> queue = new Queue<XmlNode>();
            queue.Enqueue(node);
            while (queue.Count > 0)
            {

                XmlNode node1 = queue.Dequeue();
                
                string xNodePath = FindXPath(node1);
                if (string.IsNullOrEmpty(xNodePath)) continue;

                ConfigDataItemModel xi = new ConfigDataItemModel();
                xi.XPath = xNodePath;

                if (node1.HasChildNodes)
                    if (node1.ChildNodes[0].NodeType == XmlNodeType.Text)
                    {
                        xi.IsValue = true;
                        xi.IsList = false;
                    }
                    else
                    {
                        xi.IsValue = false;
                        xi.IsList = true;
                    }
                else
                {
                    xi.IsList = false;
                    xi.IsValue = false;
                }
                
                xi.Name = node1.Name;

                if (!dic.ContainsKey(xNodePath) && (xi.IsValue || xi.IsList)) dic.Add(xNodePath, xi);


                //build attribute search and attribute value
                StringBuilder attrSearchParten = new StringBuilder(); 
                foreach (XmlAttribute xa in node1.Attributes)
                {
                    if (xi.IsList)
                    {
                        if (!string.IsNullOrEmpty(attrSearchParten.ToString()))
                        {
                            attrSearchParten.Append(" and ");
                        }
                        attrSearchParten.Append("@");
                        attrSearchParten.Append(xa.Name);
                        attrSearchParten.Append("='");
                        attrSearchParten.Append(xa.Value);
                        attrSearchParten.Append("'");

                    }


                    string xPath = FindXPath(xa);
                    if (string.IsNullOrEmpty(xPath)) continue;
                    if (!dic.Keys.Contains(xPath))
                    {
                        xi = new ConfigDataItemModel();
                        xi.XPath = xPath;
                        xi.IsList = false;
                        xi.Name = xa.Name;
                        xi.IsValue = true;
                        dic.Add(xPath, xi);
                    }
                }
                if (!string.IsNullOrEmpty(attrSearchParten.ToString()))
                {
                    attrSearchParten.Insert(0,"[");
                    attrSearchParten.Append("]");
                    attrSearchParten.Insert(0, xNodePath);
                    xi = new ConfigDataItemModel();

                    xi.XPath = attrSearchParten.ToString();
                    xi.Name = xi.XPath;
                    xi.IsList = true;
                    xi.IsValue = false;
                    if (!dic.Keys.Contains(xi.XPath)) dic.Add(xi.XPath, xi);
                }

                foreach (XmlNode n in node1.ChildNodes)
                {
                 
                    queue.Enqueue(n);
                }
            }
        }

        private string FindXPath(XmlNode node)
        {
            StringBuilder builder = new StringBuilder();
            while (node != null)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Attribute:
                        builder.Insert(0, "/@" + node.Name);
                        node = ((XmlAttribute)node).OwnerElement;
                        break;
                    case XmlNodeType.Element:
                        int index = FindElementIndex((XmlElement)node);
                        builder.Insert(0, "/" + node.Name); //+ "[" + index + "]");
                        node = node.ParentNode;
                        break;
                    case XmlNodeType.Document:
                        return builder.Insert(0, "/").ToString();
                    //break;
                    //return builder.ToString();
                    default:
                        //break;
                        return string.Empty;
                        //throw new ArgumentException("Only elements and attributes are supported");
                }
            }

            return string.Empty;
            //throw new ArgumentException("Node was not in a document");
        }

        private bool FindElementList(XmlElement element)
        {
            XmlNode parentNode = element.ParentNode;
            if (parentNode is XmlDocument)
            {
                return false;
            }
            XmlElement parent = (XmlElement)parentNode;
            int count = 0;
            foreach (XmlNode candidate in parent.ChildNodes)
            {
                if (candidate is XmlElement && candidate.Name == element.Name)
                {
                    count++;
                }
            }
            return count > 1;
        }

        private int FindElementIndex(XmlElement element)
        {
            XmlNode parentNode = element.ParentNode;
            if (parentNode is XmlDocument)
            {
                return 1;
            }
            XmlElement parent = (XmlElement)parentNode;
            int index = 1;
            foreach (XmlNode candidate in parent.ChildNodes)
            {
                if (candidate is XmlElement && candidate.Name == element.Name)
                {
                    if (candidate == element)
                    {
                        return index;
                    }
                    index++;
                }
            }
            throw new ArgumentException("Couldn't find element within parent");
        }


    }

    public class ConfigurationModel
    {
        public ConfigurationModel()
        {

            Services = new List<ServiceModel>();


            ServiceModel sm = GetServiceByType(typeof(Registration));
            sm.ServiceType = "New";
            sm.ConfigItems.Where(c => c.Name == "ServiceTypeId").First().IsUpdated = true;
            sm.ConfigItems.Where(c => c.Name == "ServiceTypeId").First().Default = "1";
            //sm.ConfigItems.Where(c => c.Name == "ServiceTypeId").First().IsFixed =true;
            sm.Name = "New";
            Services.Add(sm);


            sm = GetServiceByType(typeof(Registration));
            sm.ServiceType = "Discharge";
            sm.Name = "Discharge";
            sm.ConfigItems.Where(c => c.Name == "ServiceTypeId").First().IsUpdated = true;
            sm.ConfigItems.Where(c => c.Name == "ServiceTypeId").First().Default = "2";
            //sm.ConfigItems.Where(c => c.Name == "ServiceTypeId").First().IsFixed = true;
            Services.Add(sm);

            sm = GetServiceByType(typeof(Registration));
            sm.ServiceType = "Renewal";
            sm.Name = "Renewal";
            sm.ConfigItems.Where(c => c.Name == "ServiceTypeId").First().IsUpdated = true;
            sm.ConfigItems.Where(c => c.Name == "ServiceTypeId").First().Default = "3";
            //sm.ConfigItems.Where(c => c.Name == "ServiceTypeId").First().IsFixed = true;
            Services.Add(sm);

        }

        public int ClientInfoId { get; set; }
        public bool IsEnabled { get; set; }
        public string FileExtension { get; set; }
        public string FileEncoding { get; set; }
        public string ParserAssembly = "FileProcess.dll";
        public string ParserClassName { get; set; }

        public static ServiceModel GetServiceByType(Type type)
        {
            
            ServiceModel sm = new ServiceModel();
            PropertyInfo[] pis = type.GetProperties();
            TemplateItemModel ti;
            foreach (PropertyInfo pi in pis)
            {


                if (pi.Name!="Id" && pi.Name != "IsDeleted" && pi.Name != "IsCurrentData" && pi.Name != "CreatedDateTime" && pi.Name != "UpdatedDateTime"
                    && ((Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType).IsValueType || pi.PropertyType == typeof(string)))
                {


                    ti = new TemplateItemModel();
                    ti.Name = pi.Name;
                    if (ti.Name == "ServiceTypeId") ti.IsFixed = true;
                    if (ti.Name == "ContractType" ) ti.IsUpdated = true;
                    sm.ConfigItems.Add(ti);

                }
                else
                {
                    //exclude the foreign key 
                    if (pi.PropertyType.IsClass)
                    {
                        string key;
                        ForeignKeyAttribute attForeignKey = pi.GetCustomAttribute(typeof(ForeignKeyAttribute)) as ForeignKeyAttribute;
                        if (attForeignKey != null && attForeignKey.Name!= "ServiceTypeId")
                        {
                            key = attForeignKey.Name;
                            sm.ConfigItems.Remove(sm.ConfigItems.Where(c => c.Name == key).First());
                        }
                    }

                }
            }
            //add one TemporaryVariable for process temporary data
            ti = new TemplateItemModel();
            ti.Name = "TemporaryVariable0";
            sm.ConfigItems.Add(ti);
            ti = new TemplateItemModel();
            ti.Name = "TemporaryVariable1";
            sm.ConfigItems.Add(ti);
            ti = new TemplateItemModel();
            ti.Name = "TemporaryVariable2";
            sm.ConfigItems.Add(ti);


            sm.ConfigItems.Sort();


            return sm;
        }

        public static ServiceModel GetServiceByType(string serviceType)
        {
            Type t;
            switch (serviceType)
            {
                case "New":
                    t = typeof(Registration);
                    break;
                case "Renew":
                    t = typeof(Registration);
                    break;
                case "Discharge":
                    t = typeof(Registration);
                    break;
                case "Amendment":
                    t = typeof(Registration);
                    break;
                case "Expire":
                    t = typeof(Registration);
                    break;

                case "RegistrationDebtor":

                    t = typeof(RegistrationDebtor);
                    break;
                case "RegistrationDealer":
                    t = typeof(RegistrationDealer);
                    break;
                case "RegistrationSerialCollateral":
                    t = typeof(RegistrationSerialCollateral);
                    break;
                case "RegistrationGeneralCollateral":
                    t = typeof(RegistrationGeneralCollateral);
                    break;
                case "RegistrationSecuredParty":
                    t = typeof(RegistrationSecuredParty);
                    break;
                case "RegistrationJurisdictionSpecificInfo":
                    t = typeof(RegistrationJurisdictionSpecificInfo);
                    break;
                default:
                    t = typeof(RegistrationDebtor);
                    break;

            }
            return GetServiceByType(t);
        }
        public List<ServiceModel> Services { get; set; }

        public string ToXML()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<Configurations>");
            sb.AppendLine("<FileProcessFlow>");
            sb.AppendLine("<Parser>");
            foreach (ServiceModel sm in Services)
            {
                //for root service ,if source is null ,ignore it
                if(!string.IsNullOrEmpty(sm.Source))
                    sb.Append(sm.ToXML());
            }
            sb.AppendLine("</Parser>");
            sb.AppendLine(" </FileProcessFlow>");
            sb.AppendLine("</Configurations>");

            return sb.ToString();
        }

        public static ConfigurationModel LoadFromXml(string xml)
        {
            ConfigurationModel cm = new ConfigurationModel();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xml);
            XmlNodeList parsers = xDoc.SelectNodes("//Configurations/FileProcessFlow/Parser/ServiceRow");

            foreach (XmlNode pNode in parsers)
            {
                foreach (ServiceModel sm in cm.Services)
                {
                    if (pNode.Attributes["ServiceType"].Value == sm.ServiceType )
                    {
                        sm.Populate(pNode);
                        break;
                    }
                }
              
            }
            return cm;
        }

        

    }

    public class ServiceModel
    {
        public  ServiceModel()
        {
            ConfigItems = new List<TemplateItemModel>();
            Services = new List<ServiceModel>();
        }

        public bool IsUpdated { get; set; }

        public string ServiceType { get; set; }
        public string Source { get; set; }

        public string Name { get; set; }

        public List<TemplateItemModel> ConfigItems { get; set; }

        public List<ServiceModel> Services { get; set; }


        public void Populate(XmlNode pNode)
        {
            IsUpdated = true;
            Source = pNode.Attributes["source"].Value;
            ServiceType = pNode.Attributes["ServiceType"].Value;

            foreach (XmlNode item in pNode.ChildNodes)
            {
                if (item.Name == "item")
                {
                    foreach (TemplateItemModel templateItemModel in ConfigItems)
                    {
                        if (item.Attributes["name"].Value == templateItemModel.Name)
                        {
                            templateItemModel.Source = item.Attributes["source"].Value;
                            templateItemModel.Default = item.Attributes["default"].Value;
                            templateItemModel.IsNeedProcess = item.Attributes["isNeedProcess"].Value;
                            if (item.Attributes["convert"] != null)
                                templateItemModel.ConvertFunc = item.Attributes["convert"].Value;
                            if (templateItemModel.IsNeedProcess == "true")
                                templateItemModel.JavaScript = item.FirstChild.InnerText;
                            templateItemModel.IsUpdated = true;
                            break;
                        }
                    }
                }
                else if (item.Name == "ServiceRow")
                {
                    bool bFound = false;
                    foreach (ServiceModel sm in Services)
                    {
                        if (sm.ServiceType == item.Attributes["ServiceType"].Value &&
                        !sm.IsUpdated)
                        {
                            bFound = true;
                            sm.Populate(item);
                            break;

                        }
                    }
                    if (!bFound)
                    {
                        ServiceModel sm = ConfigurationModel.GetServiceByType(item.Attributes["ServiceType"].Value);
                        sm.Name = item.Attributes["ServiceType"].Value;
                        sm.ServiceType = item.Attributes["ServiceType"].Value;
                        sm.Populate(item);
                        Services.Add(sm);
                    }
                }

            }
        }

        public string ToXML()
        {
            bool isRet = false;
            //todo build xml 
            StringBuilder sb = new StringBuilder();
            sb.Append("<ServiceRow ServiceType=\"");
            sb.Append(ServiceType);
            sb.Append("\" source = \"");
            sb.Append(Source);
            sb.Append("\" > ");
            //very import step, dont remove, make sure the item which contains js is in the bottom node
            ConfigItems.Sort();
            foreach (TemplateItemModel ti in ConfigItems)
            {
                //if no any configitems,return empty
                if (ti.IsUpdated)
                {
                    isRet = true;
                    IsUpdated = true;
                }
                sb.Append(ti.ToXML());
            }
            foreach (ServiceModel sm in Services)
            {
                //if has any sub-services ,return 
                isRet = true;
                sb.Append(sm.ToXML());
            }

            sb.Append("</ServiceRow>");


            if (isRet)
                return sb.ToString();
            else
                return string.Empty;
        }




    }

    public class TemplateItemModel:IComparable
    {
        public TemplateItemModel()
        {
            IsNeedProcess = "false";
            Source = string.Empty;
            Name = string.Empty;
            Default = string.Empty;

        }
        public bool IsUpdated { get; set; }

        public bool IsFixed { get; set; }

        public string Name { get; set; }
        public string Source { get; set; }

        public string Default { get; set; }

        public string IsNeedProcess { get; set; }

        public string ConvertFunc { get; set; }

        public string JavaScript { get; set; }


        public string ToXML()
        {
            StringBuilder sb = new StringBuilder();
            if (IsUpdated)
            {
                sb.Append(" <item name=\"");
                sb.Append(Name);
                sb.Append("\" source = \"");
                sb.Append(Source);

                sb.Append("\" default=\"");
                sb.Append(Default);
                sb.Append("\" isNeedProcess = \"");
                sb.Append(IsNeedProcess);
                if (!string.IsNullOrEmpty(ConvertFunc) && !ConvertFunc.StartsWith(";"))
                {
                    sb.Append("\" convert = \"");
                    sb.Append(ConvertFunc);
                }
                
                sb.Append("\" > ");
                if (string.Equals(IsNeedProcess, "true", StringComparison.OrdinalIgnoreCase))
                {
                    sb.AppendLine();
                    sb.Append("<script>");
                    sb.Append(JavaScript);
                    sb.Append("</script>");
                }
                sb.AppendLine();
                sb.Append(" </item>");
            }

            return sb.ToString();
        }


        public int CompareTo(object other)
        {
            if (null == other) return 1;
            if(IsNeedProcess==null) return this.Name.CompareTo(((TemplateItemModel)other).Name);
            //make sure the item is on the bottom node when IsNeedProcess is true
            if (IsNeedProcess.ToLower() == "true") return 1;
            else
            {
                if (IsNeedProcess == ((TemplateItemModel)other).IsNeedProcess)
                    return this.Name.CompareTo(((TemplateItemModel)other).Name);
                else return -1;
            }
        }

    }



    public class ConfigDataItemModel:IComparable
    {
        public string Name { get; set; }
        public string XPath { get; set; }

        public bool IsList { get; set; }
        public bool IsValue { get; set; }

        public override string ToString()
        {
            return XPath;
        }


        public int CompareTo(object other)
        {
            if (null == other|| XPath==null) return 1;
            return this.XPath.CompareTo(((ConfigDataItemModel)other).XPath);
        }
    }

}
