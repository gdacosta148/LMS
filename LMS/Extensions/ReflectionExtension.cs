using Microsoft.AspNetCore.Razor.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Extensions
{
    public static class ReflectionExtension
    {

        public static string GetPropertyValues<T>(this T item, string propertyName)
        {

            return item.GetType().GetProperty(propertyName).GetValue(item, null).ToString();

        }

    }
}
