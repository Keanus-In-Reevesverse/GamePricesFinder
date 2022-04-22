using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace RevolutionaryLib
{
    public class Handler
    {
        public void GetGames(string html)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            
            var divs = doc.DocumentNode.Descendants("div");

            var values = new List<string>();

            foreach (var div in divs)
            {
                try
                {
                    if (div.Attributes["class"].Value == "product-card--grid")
                    {
                        var newDoc = new HtmlAgilityPack.HtmlDocument();
                        newDoc.LoadHtml(div.InnerHtml);
                        //var insideDivs = newDoc.DocumentNode.Descendants("h3");
                        var price = newDoc.DocumentNode.SelectSingleNode("//span[@class='product-price--val']").InnerText.Trim();
                        var name = newDoc.DocumentNode.SelectSingleNode("//h3[@class='product-title double-line-name']").InnerText.Trim();
                        //foreach (var insideDiv in insideDivs)
                        //{
                        //    if (insideDiv.Attributes["class"].Value == "product-title double-line-name")
                        //    {
                        //        var a = newDoc.DocumentNode.SelectSingleNode("//h3[@class='product-title double-line-name']");
                        //    }
                        //}
                        //values.Add(div.Attributes["value"].Value);
                    }
                }
                catch (Exception)
                {
                    //ignored
                }
            }
        }
    }
}
