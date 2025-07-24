using Abstraction.Contract.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Application.Common.Helpers
{
    public class AppLang
    {
         
        public static bool IsArabic => CultureInfo.CurrentUICulture.Name.Contains("ar");

        
    }
}
