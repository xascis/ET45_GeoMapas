using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Plugin.Geolocator.Abstractions;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;

namespace ET45_GeoMapas
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            if (CrossGeolocator.IsSupported)
            {
                if (CrossGeolocator.Current.IsGeolocationAvailable)
                {
                    locationData.Text = "Geolocalización disponible";
                    if (CrossGeolocator.Current.IsGeolocationEnabled)
                    {
                        locationData.Text += " y está habilitada";
                    }
                    else
                    {
                        locationData.Text += " y está desabilitada";
                    }
                }
            }
            else
            {
                locationData.Text = "Localización no soportada por este dispositivo o platadorma.";
                botonLocalizacion.IsEnabled = false;
            }

            botonLocalizacion.Clicked += BotonLocalizacion_Clicked;
        }

        private async void BotonLocalizacion_Clicked(object sender, EventArgs e)
        {
            Plugin.Geolocator.Abstractions.Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                position = await locator.GetLastKnownLocationAsync();
                if (position != null)
                {
                    locationData.Text =
                        "Última posición conocida: Lat: " + position.Latitude.ToString("0.0000") +
                        " Lon: " + position.Longitude.ToString("0.0000") +
                        " - Alt; " + position.Altitude.ToString() +
                        " Error: " + position.Accuracy.ToString();
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
                if (position != null)
                {
                    locationData.Text =
                        "Posición actual: Lat: " + position.Latitude.ToString("0.0000") +
                        " Lon: " + position.Longitude.ToString("0.0000") +
                        " Alt; " + position.Altitude.ToString() +
                        " Error: " + position.Accuracy.ToString();

                    var posicionMapa = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
                    miMapa.MoveToRegion(MapSpan.FromCenterAndRadius(posicionMapa, Distance.FromMeters(1000)));
                }

            }
            catch (Exception ex)
            {
                locationData.Text = "Error de geolocalización: " + ex.Message;
            }
        }
    }
}
