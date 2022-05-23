using ScopoHR.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ScopeHR.WebUI.Reports
{
    public static class ReportHelper
    {



        /// <summary>
        /// 
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetPropertyValue(this object Object, string propertyName)
        {
            var propertyValue = Object.GetType().GetProperties()
                                .SingleOrDefault(c => c.Name == propertyName)
                                .GetValue(Object, null);

            if (propertyValue == null)
                return string.Empty;

            return propertyValue.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            object[] values = new object[props.Count];

            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }

            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static MemoryStream ConvertToCSV<T>(string column, List<T> data)
        {
            string[] columnList = column.Split(',');

            MemoryStream output = new MemoryStream();
            StreamWriter writer = new StreamWriter(output);

            foreach (var Properties in data[0].GetType().GetProperties())
            {
                if (!columnList.Contains(Properties.Name) && !Properties.Name.ToLower().Contains("id"))
                {
                    writer.Write(Properties.Name + ",");
                }
            }

            writer.WriteLine();

            object propertyValue;

            foreach (var row in data)
            {
                foreach (var Properties in row.GetType().GetProperties())
                {
                    if (!columnList.Contains(Properties.Name) && !Properties.Name.ToLower().Contains("id"))
                    {
                        propertyValue = Properties.GetValue(row, null);

                        if (propertyValue != null)
                        {
                            propertyValue = propertyValue.ToString().Replace(',', ' ');
                            propertyValue = propertyValue.ToString().Replace("'", " ");
                            propertyValue = propertyValue.ToString().Trim();
                        }

                        writer.Write(propertyValue + ",");
                    }
                }
                writer.WriteLine();
            }

            writer.Flush();
            output.Position = 0;

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rdlcName"></param>
        /// <param name="ds"></param>
        /// <param name="reportFilteringVM"></param>
        public static void SetData(string rdlcName, DataSet ds, ReportFilteringViewModel reportFilteringVM)
        {
            HttpContext.Current.Session["RdlcName"] = rdlcName;
            HttpContext.Current.Session["DataSet"] = ds;
            HttpContext.Current.Session["ReportFilteringViewModel"] = reportFilteringVM;
        }
    }
    
}