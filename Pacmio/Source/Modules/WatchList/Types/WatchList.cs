/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Xu;

namespace Pacmio
{
    public abstract class WatchList : IEquatable<WatchList>
    {
        public virtual string Name { get; set; }

        public virtual List<Contract> Contracts { get; protected set; }

        public virtual int NumberOfRows { get => Contracts.Count(); set { } }

        public abstract string ConfigurationString { get; }

        public virtual string Description => Name + " | " + ConfigurationString;

        public override string ToString() => Description;


        public void Update() 
        {
            UpdateTime = DateTime.Now;
            OnUpdateHandler?.Invoke(0, UpdateTime, "");
        }

        public DateTime UpdateTime { get; protected set; }

        public event StatusEventHandler OnUpdateHandler;

        #region Equality

        public virtual bool Equals(WatchList other) => other is WatchList wt && GetType() == wt.GetType() && Description == wt.Description;

        public static bool operator ==(WatchList s1, WatchList s2) => s1.Equals(s2);
        public static bool operator !=(WatchList s1, WatchList s2) => !s1.Equals(s2);

        public override bool Equals(object other) => other is WatchList wt ? Equals(wt) : false;

        public override int GetHashCode() => Description.GetHashCode();

        #endregion Equality
    }
}
