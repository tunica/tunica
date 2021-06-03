using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Tunica
{
    public class License
    {
        public String Type { get; set; }
        public String Path { get; set; }
        public Double Confidence { get; set; }

        public License (String type, String path, Double confidence)
        {
            Type = type;
            Path = path;
            Confidence = confidence;
        }
    }
}
