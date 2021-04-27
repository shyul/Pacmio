/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************
using System;
using Xu;

namespace Pacmio
{
    public class SignalColumn : DatumColumn
    {
        public SignalColumn(SignalAnalysis source, Type datumType)
            : base(source.Name + "_Signal", datumType)
        {
            Source = source;
        }

        public SignalAnalysis Source { get; }
    }
}
