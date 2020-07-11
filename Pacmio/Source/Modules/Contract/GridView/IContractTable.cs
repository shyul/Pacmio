/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using Xu;

namespace Pacmio
{
    public interface IContractTable : ITable
    {
        Contract this[int i, ContractColumn column] { get; }
    }
}
