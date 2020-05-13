using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace FileProcessConfiguration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempleteController : Controller
    {
        // GET: api/GetDataElements
        [HttpGet]
        public JsonResult Get()
        {
            string txt = System.IO.File.ReadAllText(System.Environment.CurrentDirectory + "\\wwwroot\\FileParserConfig.xml");
            //txt= txt.Replace(" ", "&nbsp;");
            //new  System.Environment.CurrentDirectory + "\\wwwroot\\FileParserConfig.xml", FileMode.Open);
            //fs.re
            //txt = "<a href='FileParserConfig.xml'>asdfasdf</a>" + txt;
            //txt.Replace("<?xml version=\"1.0\" encoding=\"utf - 8\" ?>", "");
            //txt = "</div>" + txt + "<div>";
            //txt = txt.Replace("\"?\"", "</div>\"<select>< option * ngFor = \"let xmlitem of items\"[value] = \"xmlitem\" >{ { xmlitem.xPath} }</ option ></ select > \"<div>");
            //txt = System.Web.HttpUtility.HtmlEncode(txt);
            txt = txt.Replace("\r\n", "LINE_LINE");
            txt = txt.Replace(" ", "SPACE_SPACE");
            txt = System.Web.HttpUtility.HtmlEncode(txt);
            //txt = txt.Replace(System.Web.HttpUtility.HtmlEncode("\r\n"), "<p/>");
            //Response.WriteAsync(txt);
            return Json( ( txt));
            //return new List<XMLItem>();
        }
    }
}