using System;
using System.Xml;

namespace GoPS.Classes
{
    public class getGoogleGeocode
    {
        public string getGoogleGCInverse(string latlng)
        {
            string apikey = "AIzaSyAaxU4-c1ifle2YqKr6NHGQLoPncjq7fWY";
            string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?latlng={0}&result_type=street_address&key={1}", Uri.EscapeDataString(latlng), Uri.EscapeDataString(apikey));
            string FullAddress = "";
            XmlDocument xdoc = new XmlDocument();
            requestUri.Replace("||", "|");
            requestUri.Replace("|||", "|");
            xdoc.Load(requestUri);
            if (xdoc.GetElementsByTagName("status").Item(0).InnerText=="OK")
            {
                XmlNodeList xnl = xdoc.GetElementsByTagName("result");
                XmlNode xNode = xnl.Item(0);
                FullAddress = xNode.SelectSingleNode("formatted_address").InnerText;
            }            
            return FullAddress;
        }        
    }
}