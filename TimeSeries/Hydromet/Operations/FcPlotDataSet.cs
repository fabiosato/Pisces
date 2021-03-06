﻿using Reclamation.Core;
using System.IO;
using System.Data;
using System.Collections.Generic;
using Reclamation.TimeSeries;
using System;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Hydromet.Operations
{
    
    
    public partial class FcPlotDataSet {


        

        static string xlsFileName()
         {

             string s_xlsFileName = FileUtility.GetFileReference("RuleCurves.xlsx");

           return s_xlsFileName;

        }


        internal static PeriodicSeries GetPeriodicSeries(string sheetName)
        {
            var Table = ExcelUtility.Read(xlsFileName(), sheetName);
            var rval = new PeriodicSeries(Table);
            return rval;
        }

        internal static DataTable GetTable(string sheetName)
        {
            return ExcelUtility.Read(xlsFileName(), sheetName);
        }

        internal static System.Data.DataTable ControlPointTableFromName(string text, string lookupColumnName)
        {
            var tbl = ExcelUtility.Read(xlsFileName(), "ControlPoints");
            tbl = DataTableUtility.Select(tbl, lookupColumnName+" = '" + text + "'", "");
            return tbl;
        }

        public static double[] GetVariableForecastLevels(string curveName)
        {
            var rval = new List<double>();
            var tbl = ExcelUtility.Read(xlsFileName(), "VariableForecastLevels");
              tbl = DataTableUtility.Select(tbl," RuleCurve = '" + curveName + "'","");

            if (tbl.Rows.Count > 0)
            {
                var s = tbl.Rows[0][1].ToString();
                var tokens = s.Split(',');
                for (int i = 0; i < tokens.Length; i++)
                {
                    double v=0;
                    if( double.TryParse(tokens[i],out v))
                        rval.Add(v);
                }
            }

            return rval.ToArray();
        }

        /// <summary>
        /// Extracts DateTime[] used for label positions
        /// </summary>
        /// <param name="curveName"></param>
        /// <returns>list of dates using year 2000</returns>
        public static DateTime[] GetVariableForecastLabelDates(string curveName)
        {
            var rval = new List<DateTime>();
            var tbl = ExcelUtility.Read(xlsFileName(), "VariableForecastLevels");
                tbl = DataTableUtility.Select(tbl," RuleCurve = '" + curveName + "'","");
            //stored in excel as single string for example:
            /// 6/15,5/15,5/15,4/15,4/15, 3/15, 2/15, 1/15
            if (tbl.Rows.Count > 0)
            {
                var s = tbl.Rows[0][2].ToString();
                var tokens = s.Split(',');
                for (int i = 0; i < tokens.Length; i++)
                {
                    var x  = tokens[i].Split('/');
                    if( x.Length != 2)
                        break;
                    int m, d;
                    if(   int.TryParse(x[0],out m)
                        && int.TryParse(x[1],out d) )
                    {
                        rval.Add(new DateTime(2000,m,d));
                    }
                }
            }

            return rval.ToArray();
        }

        public static string[] GetNames()
        {
            var tbl = ExcelUtility.Read(xlsFileName(), "ControlPoints");
                tbl = DataTableUtility.Select(tbl," Enabled = true","");
            return DataTableUtility.Strings(tbl, "", "Name");
        }
    }
}
