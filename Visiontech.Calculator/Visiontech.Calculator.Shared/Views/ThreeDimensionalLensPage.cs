using CalcolatoreXamarin.Shared.Models;
using Org.Visiontech.Compute;
using System;
using System.Collections.Generic;
using Urho;
using Urho.Forms;
using Xamarin.Forms;

namespace CalcolatoreXamarin.Shared.Views
{
    public class ThreeDimensionalLensPage : ContentPage
    {

        private UrhoSurface urhoSurface = new UrhoSurface();

        public ICollection<threeDimensionalPointDTO> Points { get; }

        public Func<threeDimensionalPointDTO, double> Mapping { get; }

        public ThreeDimensionalLensPage(ICollection<threeDimensionalPointDTO> Points, Func<threeDimensionalPointDTO, double> Mapping) {
            Content = urhoSurface;
            this.Points = Points;
            this.Mapping = Mapping;

            urhoSurface.Show<Dots>(new DotsOptions()
            {
                Orientation = ApplicationOptions.OrientationType.LandscapeAndPortrait,
                ResizableWindow = true,
                Points = Points,
                Mapping = Mapping
            });
        }

    }
}
