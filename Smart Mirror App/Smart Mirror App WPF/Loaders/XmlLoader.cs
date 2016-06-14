using System;
using System.Xml;

namespace Smart_Mirror_App_WPF.Loaders
{
    public class XmlLoader
    {
        public XmlDocument Load(String url)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(url);
                return xml;
            }
            catch (Exception e)
            {                 
                throw e;
            }
        }

    }
}