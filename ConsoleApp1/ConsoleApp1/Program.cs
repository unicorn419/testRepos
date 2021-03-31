using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Core;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsEngineSwitcher = JsEngineSwitcher.Current;
            jsEngineSwitcher.EngineFactories.Add(new ChakraCoreJsEngineFactory());
            jsEngineSwitcher.DefaultEngineName = ChakraCoreJsEngine.EngineName;

            ServiceRow b = new ServiceRow();
            b.Add("test", "test1");

            ServiceRow a = new ServiceRow(b);
            a.Add("c1", "c2");
            b.Add("arr", new List<ServiceRow>() { a });

            IJsEngine jsEngine = JsEngineSwitcher.Current.CreateDefaultEngine();
            jsEngine.EmbedHostType("ServiceRow", typeof(ServiceRow));
            jsEngine.EmbedHostObject("root", a);
            jsEngine.Execute(@" var a = root.GetParent(); var t= a.GetListItem('arr',0); a.SetItem('test',t.GetItem('c1'));");


        }
    }
}
