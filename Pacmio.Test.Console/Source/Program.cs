/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using Pacmio;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Xu;

namespace TestConsole
{
    [Serializable, DataContract]
    public abstract class Test
    {
        [DataMember]
        public virtual string Name { get; set; } = string.Empty;
    }

    [Serializable, DataContract]
    public class Test2 : Test
    {
        [DataMember]
        public virtual string Age { get; set; } = string.Empty;
    }


    [Serializable, DataContract]
    public class Test3 : Test
    {
        [DataMember]
        public virtual string Address { get; set; } = string.Empty;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Test: ");

            Time t1 = new Time(9, 30, 12);//, 599);
            Time t2 = new Time(4, 30, 30);//, 599);
            //t1 = t1.AddHours(24);
            //t1 = t1.AddMinutes(1440);
            //t1 = t1.AddSeconds(55403);

            DateTime dt = new DateTime(2020, 7, 16, 9, 30, 12);

            Console.WriteLine((t1 == dt) + " " + t1.ToString());

            TimePeriod tpd = new TimePeriod(t1, t2);

            Console.WriteLine(tpd.ToString());



            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
