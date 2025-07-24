using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.Base.Dto;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Identity.Users.Dtos
{
    public record   BaseUserDto :BaseDto
    {

        /// <summary>
        /// Email address (must be unique)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Country code for mobile number
        /// </summary>
       //public string? CountryCode { get; set; }  

        /// <summary>
        /// International Bank Account Number
        /// </summary>
        public string? IBAN { get; set; }

        /// <summary>
        /// User's nationality
        /// </summary>
        public string? Nationality { get; set; }

        /// <summary>
        /// Passport number
        /// </summary>
        public string? PassportNo { get; set; }

        // Basic Information
        public string FullName { get; set; } 
        public string UserName { get; set; }

        /// <summary>
        /// User's preferred language
        /// </summary>
     //   public string PreferredLanguage { get; set; } = "ar-EG";

    }
}
