using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Text;

namespace Backend.RuletaMasivian.Utilities
{
    public static class ConvertTypes
    {
        public static List<dynamic> ToDynamic(this DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }

        /// <summary>
        /// Converts to dynamic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static List<T> ToDynamic<T>(this DataTable dt)
        {
            var dynamicDt = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }

        /// <summary>
        /// Converts to stringbase64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToStringBase64(this string value)
        {
            if (value.IsBase64())
            {
                byte[] data = Convert.FromBase64String(value);
                return Encoding.ASCII.GetString(data);
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Determines whether this instance is base64.
        /// </summary>
        /// <param name="base64String">The base64 string.</param>
        /// <returns>
        ///   <c>true</c> if the specified base64 string is base64; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBase64(this string base64String)
        {
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
            {
                return false;
            }

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether this instance has value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value has value; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Converts to xmlstring.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="root">The root.</param>
        /// <returns></returns>
        public static string ToXmlString<T>(this List<T> list, string objectName, string root)
        {
            var value = list.Serialize(new JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.None });
            var valueConvert = new StringBuilder();
            valueConvert.Append("{");
            valueConvert.Append($"\"{objectName}\":");
            valueConvert.Append(value);
            valueConvert.Append("}");
            var doc = JsonConvert.DeserializeXmlNode(valueConvert.ToString(), root).OuterXml;
            return doc;
        }

        /// <summary>
        /// Gets the value or null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compare">The compare.</param>
        /// <returns></returns>
        public static object GetValueOrNull(this string value, string compare = null)
        {
            if (string.IsNullOrEmpty(value) || value.Equals(compare, StringComparison.InvariantCultureIgnoreCase))
            {
                return DBNull.Value;
            }

            return value;
        }

        /// <summary>
        /// Gets the value or null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static object GetValueOrNull(this double? value)
        {
            if (!value.HasValue)
            {
                return DBNull.Value;
            }

            return value.Value;
        }
    }
}
