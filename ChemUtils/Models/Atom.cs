// Atom.cs created on 05/25/2016 by HyunMi
// Copyright (c) HyunMi. All rights reserved
// Licensed under the GPL 3.0 license

using System;

namespace ChemUtils.Models
{
    [Serializable]
    public struct Atom
    {
        public int AtomicNumber { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double? Mass { get; set; }
        // Degrees C
        public int? MeltingPoint { get; set; }
        // Degrees C
        public int? BoilingPoint { get; set; }
        public double? Density { get; set; }
        // In the earths crust
        public double? Abundance { get; set; }
        public int? Discovery { get; set; }
        public string ElectronConfiguration { get; set; }
        // In eV
        public double? Energy { get; set; }
    }
}