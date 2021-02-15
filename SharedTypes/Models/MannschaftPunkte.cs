using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharedTypes.Models
{
    public struct Punktestand
    {
        public Mannschaft[] Mannschaft { get; set; }
        public int[] Punkte { get; set; }
    }
}