using System;
using System.Linq;
using System.Web.Mvc;

namespace EventSite.Infrastructure.Helpers {
    public static class ExtensionMethods {
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
            where TEnum : struct, IComparable, IFormattable, IConvertible {
            if(!typeof(TEnum).IsEnum) {
                throw new ArgumentException("TEnum must be an enumerated type.");
            }

            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                select new {
                    Id = e,
                    Name = e.ToString()
                };

            return new SelectList(values, "Id", "Name", enumObj);
        }
    }
}