/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using Xu;

namespace Pacmio
{
    public abstract class BarAnalysis : IDependable, IEquatable<BarAnalysis>, IDisposable
    {
        public string Label { get; protected set; }

        #region Ctor

        public void Cancel() { Dispose(false); }

        public void Dispose() { Dispose(true); }

        protected virtual void Dispose(bool disposing)
        {
            // Also distroy all dependent analysis

            if (disposing) Remove(true);

            //GC.Collect();
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        #endregion Ctor

        #region Methods

        public virtual void Remove(bool recursive = false)
        {
            if (recursive || Children.Count == 0)
            {
                foreach (IDependable child in Children)
                    child.Remove(true);

                foreach (IDependable parent in Parents)
                    parent.CheckRemove(this);

                if (Children.Count > 0)
                    throw new Exception("Still have children in this setup");
            }
            else
            {
                if (Children.Count > 0)
                {
                    foreach (var child in Children)
                        child.Enabled = false;
                }
                Enabled = false;
            }
        }

        public virtual void Update(BarAnalysisPointer bap) // Cancellation Token should be used
        {
            if (!bap.IsUpToDate)
            {
                bap.StopPt = bap.Count;

                if (bap.StartPt < 0)
                    bap.StartPt = 0;
                else if (bap.StartPt > bap.StopPt)
                    bap.StartPt = bap.StopPt - 1;

                Calculate(bap);
                bap.StartPt = bap.Count;
            }
        }

        protected abstract void Calculate(BarAnalysisPointer bap);

        #endregion Methods

        #region Identification

        /// <summary>
        /// example: SMA(16), EMA(32)-SMA(5)
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Legend group name
        /// </summary>
        public virtual string GroupName { get; set; } = "DefaultBarAnalysisGroup";

        public virtual string Description { get; protected set; } = "DefaultBarAnalysisDescription";

        #endregion Identification

        #region Priority and Dependency

        /// <summary>
        /// Depending on how many level of the Associated Analysis
        /// </summary>
        public virtual int Order { get; set; } = 0;

        /// <summary>
        /// May not be calculated for performance optimization.
        /// Set during the clean up process
        /// Stop but keep the data
        /// When it is not deleting this analysis entirely
        /// 
        /// Also set the depending (downstream) analysis
        /// 
        /// </summary>
        public virtual bool Enabled
        {
            get
            {
                return m_Enabled;
            }
            set
            {
                if (value)
                {
                    var parents = this.GetParents();
                    foreach (var p in parents)
                        p.Enabled = true;
                }
                else
                {
                    var children = this.GetChildren();
                    foreach (var c in children)
                        c.Enabled = false;
                }
                m_Enabled = value;
            }
        }
        protected bool m_Enabled = true;

        public virtual ICollection<IDependable> Children { get; } = new HashSet<IDependable>();

        public virtual ICollection<IDependable> Parents { get; } = new HashSet<IDependable>();

        #endregion Priority and Dependency

        #region Equality

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode();
        public bool Equals(BarAnalysis other) => GetType() == other.GetType() && Name == other.Name;
        public static bool operator !=(BarAnalysis s1, BarAnalysis s2) => !s1.Equals(s2);
        public static bool operator ==(BarAnalysis s1, BarAnalysis s2) => s1.Equals(s2);
        public override bool Equals(object other) => other is BarAnalysis ba && Equals(ba);

        #endregion Equality
    }
}
