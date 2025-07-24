using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Helpers
{
    public static class EnumHelper
    {
        public static string GetDescription(Enum enumValue)
        {
            var member = enumValue.GetType().GetMember(enumValue.ToString());
            var attribute = member[0].GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? enumValue.ToString();
        }
    }
}
