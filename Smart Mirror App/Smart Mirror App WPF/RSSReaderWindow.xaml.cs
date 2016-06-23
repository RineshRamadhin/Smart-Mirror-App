using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using Smart_Mirror_App_WPF.Loaders;

namespace Smart_Mirror_App_WPF
{
    /// <summary>
    /// Interaction logic for RSSReaderWindow.xaml
    /// </summary>
    public partial class RssReaderWindow : Window
    {
        private string _defaultUrl = "http://www.ad.nl/home/rss.xml";
        private XmlLoader _xmlLoader = new XmlLoader();

        public RssReaderWindow()
        {
            InitializeComponent();
            UpdateUI(_xmlLoader.Load(_defaultUrl));
        }

        public void GetRssFeed()
        {
            Lable_Rss_Title.Content = "Loading RSS Feed (" + _defaultUrl + ")...";
            XmlDocument rssFeed = new XmlDocument();
            rssFeed.Load(_defaultUrl);

            UpdateUI(rssFeed);
        }

        private void UpdateUI(XmlDocument xml)
        {
            if (xml.DocumentElement != null)
            {
                var title = xml.DocumentElement.FirstChild.SelectSingleNode("title");
                if (title != null)
                    Lable_Rss_Title.Content = title.InnerText;

                XmlNodeList items = xml.DocumentElement.FirstChild.SelectNodes("item");
                if (items != null)
                    foreach (XmlNode item in items)
                    {
                        string itemTitle = item["title"]?.InnerText;
                        string itemlink = item["link"]?.InnerText;
                        string itemDescription = item["description"]?.InnerText;
                        string itemPubDate = item["pubDate"]?.InnerText;
                        string itemAuthor = item["author"]?.InnerText;
                    }
            }
        }


    }
}
