using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace QuizMaster.HelperClass
{
    public static class ConvertData
    {
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        try
                        {
                            var propType = Nullable.GetUnderlyingType(pro.PropertyType) ?? pro.PropertyType;
                            var safeValue = dr[pro.Name] == null ? null : Convert.ChangeType(dr[pro.Name], propType);

                            pro.SetValue(obj, safeValue, null);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    //pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
