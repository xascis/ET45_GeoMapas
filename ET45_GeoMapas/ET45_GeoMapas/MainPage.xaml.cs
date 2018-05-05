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
        Dictionary<string, Xamarin.Forms.Maps.MapType> dicTiposMapa = new Dictionary<string, MapType>
        {
            {"Hibrido", Xamarin.Forms.Maps.MapType.Hybrid },
            {"Satélite", Xamarin.Forms.Maps.MapType.Satellite },
            {"Callejero", Xamarin.Forms.Maps.MapType.Street },
        };

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

            foreach (string mapType in dicTiposMapa.Keys)
            {
                pickerTipoMapa.Items.Add(mapType);
            }
            pickerTipoMapa.SelectedIndexChanged += PickerTipoMapa_SelectedIndexChanged;

            botonLocalizacion.Clicked += BotonLocalizacion_Clicked;
        }

        private void PickerTipoMapa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerTipoMapa.SelectedIndex != -1)
            {
                string tipoMapa = pickerTipoMapa.Items[pickerTipoMapa.SelectedIndex];
                miMapa.MapType = dicTiposMapa[tipoMapa];
            }
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

                    Pin pin = new Pin
                    {
                        Type = PinType.Place,
                        Position = posicionMapa,
                        Label = "Estas aquí",
                        Address = "Más información"
                    };
                    miMapa.Pins.Add(pin);
                }

            }
            catch (Exception ex)
            {
                locationData.Text = "Error de geolocalización: " + ex.Message;
            }
        }
    }
}
