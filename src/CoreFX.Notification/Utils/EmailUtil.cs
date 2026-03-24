using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace CoreFX.Notification.Utils
{
    public static class EmailUtil
    {
        public static string ToHtmlTable<T>(this List<T> records)
        {
            var ret = string.Empty;
            return records == null || !records.Any()
                ? ret
                : "<table cellspacing='0' cellpadding='1' border='1'>" +
                records.First().GetType().GetProperties()
                    .Where(o => !IsJsonIgnored(o))
                    .Select(p => (string.IsNullOrEmpty(GetPropertyValue(typeof(DisplayAttribute), "Description", p))) ? p.Name : GetPropertyValue(typeof(DisplayAttribute), "Description", p)).ToList().ToColumnHeaders() +
                  records.Aggregate(ret, (current, t) => current + t.ToHtmlTableRow()) +
                  "</table>";
        }

        public static bool IsJsonIgnored(PropertyInfo prop)
        {
            return prop.GetCustomAttributes(true)
                .Where(o =>
                    o.GetType() == typeof(Newtonsoft.Json.JsonIgnoreAttribute) ||
                    o.GetType() == typeof(System.Text.Json.Serialization.JsonIgnoreAttribute)
                ).Any();
        }

        private static string GetPropertyValue(Type type, string propName, PropertyInfo prop)
        {
            string value = null;
            var d = (from q in prop.GetCustomAttributesData()
                     where q.ToString().Contains(type.FullName)
                     select q).FirstOrDefault();

            if (null != d)
            {
                var d2 = (from q in d.NamedArguments
                          where q.MemberInfo.Name == propName
                          select q.TypedValue.Value).FirstOrDefault();
                if (null != d2)
                {
                    value = d2.ToString();
                }
            }

            return value;
        }

        public static string ToColumnHeaders<T>(this List<T> props)
        {
            var ret = string.Empty;
            return props == null || !props.Any()
                ? ret
                : "<tr>" +
                  props
                    .Aggregate(ret,
                      (current, propValue) =>
                          current +
                          ("<th style='font-size: 11pt; font-weight: bold; background-color:blue; color:white'>" +
                           (Convert.ToString(propValue).Length <= 100
                               ? Convert.ToString(propValue)
                               : Convert.ToString(propValue).Substring(0, 100)) + "</th>")) +
                  "</tr>";
        }

        private static string ToHtmlTableRow<T>(this T model)
        {
            var ret = string.Empty;
            return model == null
                ? ret
                : "<tr style='text-align:left'>" +
                  model.GetType()
                      .GetProperties()
                      .Where(o => !IsJsonIgnored(o))
                      .Aggregate(ret,
                          (current, prop) =>
                              current + ("<td style='font-size: 11pt; font-weight: normal;'>" +
                                         ((Convert.ToString(prop.GetValue(model, null)).Length <= 100
                                             ? Convert.ToString(prop.GetValue(model, null))
                                             : Convert.ToString(prop.GetValue(model, null)).Substring(0, 100))) +
                                         "</td>")) + "</tr>";
        }

    }
}
