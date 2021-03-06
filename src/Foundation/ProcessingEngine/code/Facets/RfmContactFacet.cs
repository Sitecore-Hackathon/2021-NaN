﻿using System;
using Sitecore.XConnect;
using Facet = Sitecore.XConnect.Facet;

namespace Hackathon.NaN.MLBox.Foundation.ProcessingEngine.Facets
{
    [Serializable]
    [FacetKey(DefaultFacetKey)]
    public class RfmContactFacet : Facet
    {
        public const string DefaultFacetKey = "RfmContactFacet";

        public RfmContactFacet()
        {

        }
        public int R { get; set; }
        public int F { get; set; }
        public int M { get; set; }
        public double Recency { get; set; }
        public double Frequency { get; set; }
        public double Monetary { get; set; }
    }
}
