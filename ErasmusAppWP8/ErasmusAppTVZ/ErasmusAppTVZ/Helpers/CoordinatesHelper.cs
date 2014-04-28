using ErasmusAppTVZ.ViewModel.Geolocation;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Phone.Maps.Services;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Device.Location;


namespace ErasmusAppTVZ.Helpers
{
    class CoordinatesHelper
    {
        /// <summary>
        /// Gets country latitude and longitude using Google geocode API
        /// </summary>
        /// <param name="code"></param>
        /// <param name="value">Specifies country (1) or city(2)</param>
        /// <returns></returns>
        public static async Task<double[]> GetCoordinates(string code, int value)
        {
            double[] latLon = new double[2];
            string baseUrl = "http://maps.googleapis.com/maps/api/geocode/json?";

            if (value == 1)
                baseUrl += String.Format("components=country:{0}&sensor=false", code);
            else if (value == 2)
                baseUrl += String.Format("address={0}&sensor=false", code);

            using (HttpClient httpClient = new HttpClient())
            {
                string result = await httpClient.GetStringAsync(baseUrl);

                GeolocationData data = JsonConvert.DeserializeObject<GeolocationData>(result);

                latLon[0] = double.Parse(data.results[0].geometry.location.lat.ToString());
                latLon[1] = double.Parse(data.results[0].geometry.location.lng.ToString());
            }

            return latLon;
        }

        /// <summary>
        /// Sets the map center to a specified latitude and longitude and zooms in accordingly
        /// </summary>
        /// <param name="map"></param>
        /// <param name="coords"></param>
        /// <param name="zoom"></param>
        public static void SetMapCenter(ref Map map, double[] coords, double zoom)
        {
            //GeoCoordinate coord = new GeoCoordinate(coords[0], coords[1]);
            //map.Layers.Clear();

            //Polygon polygon = new Polygon();
            //polygon.Points.Add(new Point(-12, 0));
            //polygon.Points.Add(new Point(0, 55));
            //polygon.Points.Add(new Point(12, 0));
            //polygon.Fill = new SolidColorBrush(Colors.Green);

            //MapLayer mapLayer = new MapLayer();
            //MapOverlay overlay = new MapOverlay();

            //overlay.Content = polygon;
            //overlay.GeoCoordinate = coord;
            //overlay.PositionOrigin = new Point(0.0, 1.0);

            //mapLayer.Add(overlay);

            //map.Layers.Add(mapLayer);
            map.Center = new GeoCoordinate(coords[0], coords[1]);
            map.ZoomLevel = zoom;
        }
    }
}
