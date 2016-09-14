using System;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;
using System.IO;

namespace Pisces.NunitTests.Database
{
    [TestFixture]
    public class TestSiteInfo
    {

        [Test]
        public void TestSQLite()
        {
            var fn = FileUtility.GetTempFileName(".pdb"); 

            SQLiteServer svr = new SQLiteServer(fn);
            var db = new TimeSeriesDatabase(svr,false);

            var s = new Series();

            var si = db.SiteInfo("BOII");
            bool idaho = si.state == "ID"; //idaho
            var timezone = si.timezone;

            TimeSeriesDatabaseDataSet.SeriesCatalogDataTable d = si.SeriesList();

            TimeSeriesDatabaseDataSet.SeriesCatalogRow row = d[0];
            row.Parameter = "Asce ET #5";

            d.Save();

            Console.WriteLine(si.SeriesList()[0].Parameter);

            var goodStats = (si.Parameters()[0].statistic == "Avg");


        }
    }
}