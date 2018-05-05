using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ET45_GeoMapas.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            Xamarin.FormsMaps.Init("FEvkLtJ0NbMlSNXlwJcN~RIqebzr3X2j2zbzMXSwmvQ~AgSWKcHoPos5Tvwsf_8tQHPT8izNKKTOVaMpwFL4g5AKX8iMgOVNHzZEwRh7bOOJ");

            LoadApplication(new ET45_GeoMapas.App());
        }
    }
}
