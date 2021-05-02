/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio
{
    public static class StrategyManager
    {


        private static Dictionary<Strategy, AccountInfo> StrategyToAccountLUT { get; } = new();

        public static void Assign(Strategy s, AccountInfo ac) => StrategyToAccountLUT[s] = ac;

        public static AccountInfo GetAccount(Strategy s) => StrategyToAccountLUT.ContainsKey(s) ? StrategyToAccountLUT[s] : null;


        // Generate and Tune Strategies from here. 
        // Generate a list of Strategies with different parameter...
    }
}
