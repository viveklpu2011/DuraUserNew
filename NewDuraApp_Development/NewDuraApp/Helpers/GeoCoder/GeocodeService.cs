using System;
using System.Globalization;
using System.Net;

namespace NewDuraApp.Helpers.GeoCoder
{
    public class GeocodeService
    {
        private WebClient _client;

        public GeocodeService()
        {
            _client = new WebClient();
        }

        public LocationGeoCoder GeocodeLocation(string address)
        {
            var url = "http://maps.googleapis.com/maps/api/geocode/xml?sensor=false&address=" + System.Web.HttpUtility.UrlEncode(address);

            var xmlString = _client.DownloadString(url);
            var xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(xmlString);

            var loc = new LocationGeoCoder();
            loc.Latitude = Double.Parse(xmlDoc.SelectSingleNode("//geometry/location/lat").InnerText, NumberFormatInfo.InvariantInfo);
            loc.Longitude = double.Parse(xmlDoc.SelectSingleNode("//geometry/location/lng").InnerText, NumberFormatInfo.InvariantInfo);

            return loc;
        }

        //public Task GeocodeLocationAsync(string address) {
        //  return null;
        //}
    }
}
