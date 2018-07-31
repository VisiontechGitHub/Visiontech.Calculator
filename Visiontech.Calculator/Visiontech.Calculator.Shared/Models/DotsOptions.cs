using Org.Visiontech.Compute;
using System;
using System.Collections.Generic;
using Urho;

namespace CalcolatoreXamarin.Shared.Models
{
    public class DotsOptions : ApplicationOptions
    {
        public ICollection<threeDimensionalPointDTO> Points { get; set; }

        public Func<threeDimensionalPointDTO, double> Mapping { get; set; }

        public DotsOptions()
        {
            Mapping = point => point.z;
        }

    }
}
