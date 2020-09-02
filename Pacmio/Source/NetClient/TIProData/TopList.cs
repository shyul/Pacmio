/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Trade-Ideas API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeIdeas.TIProData;
using TradeIdeas.TIProData.Configuration;
using TradeIdeas.ServerConnection;

namespace Pacmio.TIProData
{
    public static partial class Client
    {
        public static TopListGroup tlg { get; private set; }

        public static void TestTopList() 
        {
            tlg = new TopListGroup("form=1&sort=MaxGUP&count=100&MinGUP=5&MinPrice=1.5&MinTV=50000&MaxGUP=-5&MaxPrice=25&X_NYSE=on&XN=on&WN=Small+Gappers&show0=D_Symbol&show1=Price&show2=Float&show3=SFloat&show4=GUP&show5=TV&show6=EarningD&show7=Vol5&show8=STP&show9=RV&show10=D_Name&show11=RD&show12=FCP&show13=D_Sector&show14=&col_ver=1&omh=1");


            if (Connected) 
            {


            }
        }
    }

    public class TopListGroup
    {

        public TopListGroup(string config)
        {
            Configuration = config;
            TopList = Client.Connection.TopListManager.GetTopList(Configuration);
            TopList.TopListStatus += new TopListStatus(_topList_TopListStatus);
            TopList.TopListData += new TopListData(_topList_TopListData);
            TopList.Start();
        }

        public string Configuration { get; private set; }

        private TopList TopList { get; set; }

        void _topList_TopListStatus(TopList sender)
        {

        }

        void _topList_TopListData(List<RowData> rows, DateTime? start, DateTime? end, TopList sender)
        {
            foreach (RowData row in rows)
            {
                Console.WriteLine(row.ToString());
            }
        }
    }
}
