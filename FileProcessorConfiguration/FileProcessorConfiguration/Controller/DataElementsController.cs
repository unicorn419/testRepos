using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Xml;
using System.Text;

namespace FileProcessorConfiguration
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataElementsController : Controller
    {
        // GET: api/GetDataElements
        [HttpGet]
        public IEnumerable<DataItem> Get(string datafileName)
        {
            return getElement(datafileName);
        }


        private List<DataItem> getElement(string datafile)
        {
            XmlDocument doc = new XmlDocument();
            List<DataItem> list = new List<DataItem>();
            if (string.IsNullOrEmpty(datafile)) return list;
            string filepath = System.Environment.CurrentDirectory + "\\wwwroot\\upload\\" + datafile;
            if (!System.IO.File.Exists(filepath)) return list;
            doc.Load(filepath);
            Dictionary<string, DataItem> dic = new Dictionary<string, DataItem>();

            foreach (XmlNode n in doc.DocumentElement.ChildNodes)
            {
                getObjects(n, ref dic);
            }

            foreach (DataItem x in dic.Values)
            {
                list.Add(x);
            }
            list.Sort();
            return list ;

        }

        private void getObjects(XmlNode node, ref Dictionary<string, DataItem> dic)
        {
            Queue<XmlNode> queue = new Queue<XmlNode>();
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                
                XmlNode node1 = queue.Dequeue();
                string xNodePath = FindXPath(node1);
                if (string.IsNullOrEmpty(xNodePath)) continue;

                DataItem xi = new DataItem();
                xi.XPath = xNodePath;
                //xi.HasValue = node1.Value != null;
                xi.Name = node1.Name;
                xi.IsList = FindElementList((XmlElement)node1);

                if (!dic.ContainsKey(xNodePath)) dic.Add(xNodePath, xi);
                else if (xi.IsList) dic[xNodePath].IsList = true;



                foreach (XmlAttribute xa in node1.Attributes)
                {

                    string xPath = FindXPath(xa);
                    if (string.IsNullOrEmpty(xPath)) continue;
                    if (!dic.Keys.Contains(xPath))
                    {
                        xi = new DataItem();
                        xi.XPath = xPath;
                        xi.IsList = false;
                        xi.Name = xa.Name;
                        xi.HasValue = xa.Value != null;
                        dic.Add(xPath, xi);
                    }

                }

                foreach (XmlNode n in node1.ChildNodes)
                {
                    string xPath = FindXPath(n);
                    if (string.IsNullOrEmpty(xPath)) continue;
                    xi = new DataItem();
                    xi.XPath = xPath;
                    xi.Name = n.Name;
                    xi.HasValue = !string.IsNullOrEmpty(n.InnerText);
                    xi.IsList = FindElementList((XmlElement)n);
                    if (!dic.Keys.Contains(xPath))
                    {
                        dic.Add(xPath, xi);
                    }
                    else if (xi.IsList) dic[xPath].IsList = true;


                    queue.Enqueue(n);                    
                }
            }
        }

        private string FindXPath( XmlNode node)
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



    public class DataItem:IComparable    
    {
        public string Name { get; set; }
        public string XPath { get; set; }

        public bool IsList { get; set; }
        public bool HasValue { get; set; }

        public override string ToString()
        {
            return XPath;
        }


        public int CompareTo(object other)
        {
            if (null == other) return 1;
            return this.XPath.CompareTo(((DataItem)other).XPath);
        }
    }



        //public XmlNodeType Type { get; set; }
        //public XMLItem Parent { get; set; }
        //public List<XMLItem> Childs = new List<XMLItem>();

    


}
