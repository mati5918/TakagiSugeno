﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakagiSugeno.Model.ViewModels
{
    public class OutputCalcVM
    {
        public int SystemId { get; set; }
        public List<InputVM> Inputs { get; set; }
        public List<OutputVM> Outputs { get; set; }
        public AndMethod AndMethod { get; set; }
        public OrMethod OrMethod { get; set; }

    }

    public class OutputCalcData
    {
        public int SystemId { get; set; }
        public Dictionary<string, double> InputsValues { get; set; }
        public AndMethod AndMethod { get; set; }
        public OrMethod OrMethod { get; set; }
    }

    public class OutputCalcResults
    {
        public Dictionary<int, double> CalculatedValues { get; set; }
        public string InfoLog { get; set; }
    }
}
