/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using Pacmio.Analysis;

namespace Pacmio
{
    public class SimulationSet
    {
        public SimulationSet(IndicatorSet tr)
        {

            IndicatorSet = tr;


            //BarTableGroup = new BarTableGroup();
        }


        public IndicatorSet IndicatorSet { get; }

        //public BarTableGroup BarTableGroup { get; }




        // Total Result <--- From per Contract result.
        //public SimulationResult Result { get; } = new SimulationResult();






    }



    public class SimulationDatum
    {


    }
}
