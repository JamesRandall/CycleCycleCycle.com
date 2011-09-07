using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;
using CycleCycleCycle.Services.Utilities;

namespace CycleCycleCycle.Services.Implementation
{
    public class YahooGeocodeService : IGeocodeService
    {
        private const string ServiceUrl = @"http://where.yahooapis.com/geocode?q={0}&appid={1}";
        private const string YahooAppId = @"";

        public Location Geocode(string searchString)
        {
            try
            {
                WebClient client = new WebClient();
                string encodedSearchString = HttpUtility.HtmlEncode(searchString);
                string url = String.Format(ServiceUrl, encodedSearchString, YahooAppId);
                string results = client.DownloadString(url);

                XDocument resultsXml = XDocument.Load(new StringReader(results));
                XElement resultXml = resultsXml.Root.Elements("Result").FirstOrDefault();
                if (resultXml == null) return null;

                List<string> lines = new List<string>();
                if (resultXml.Element("line1") != null) lines.Add(resultXml.Element("line1").Value);
                if (resultXml.Element("line2") != null) lines.Add(resultXml.Element("line2").Value);
                if (resultXml.Element("line3") != null) lines.Add(resultXml.Element("line3").Value);
                if (resultXml.Element("line4") != null) lines.Add(resultXml.Element("line4").Value);

                bool addedLine = false;
                StringBuilder sb = new StringBuilder();
                foreach(string line in lines)
                {
                    if (!String.IsNullOrWhiteSpace(line))
                    {
                        if (addedLine)
                        {
                            sb.Append(", ");
                        }
                        else
                        {
                            addedLine = true;
                        }
                        sb.Append(line);
                    }
                }

                return new Location
                           {
                               Latitude = double.Parse(resultXml.Element("latitude").Value),
                               Longitude = double.Parse(resultXml.Element("longitude").Value),
                               Name = sb.ToString()
                           };
            }
            catch (Exception ex)
            {
                throw new GeocodingException("Unable to geocode location.", ex);
            }
            
        }
    }
}