using System;
using System.Text.RegularExpressions;

namespace SaudiMobileValidationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Saudi Mobile Number Validation Test");
            Console.WriteLine("===================================");
            
            // Test the corrected regex pattern
            // 05XXXXXXXXX = 05 + telecom digit + 7 more digits (total 11)
            // +966XXXXXXXX = +966 + telecom digit + 7 more digits (total 12)
            // 966XXXXXXXX = 966 + telecom digit + 7 more digits (total 11)
            // 9665XXXXXXXX = 9665 + telecom digit + 7 more digits (total 12)
            // 009665XXXXXXXX = 009665 + telecom digit + 7 more digits (total 14)
            // 5XXXXXXXX = 5 + telecom digit + 7 more digits (total 9)
            var saudiMobilePattern = @"^(009665|9665|\+9665|05|5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$";
            
            // Valid test cases based on the gist pattern
            string[] validNumbers = {
                "0551234567",     // 05 + STC prefix 5 + 7 digits (10 total)
                "0501234567",     // 05 + STC prefix 0 + 7 digits
                "0531234567",     // 05 + STC prefix 3 + 7 digits
                "0561234567",     // 05 + Mobily prefix 6 + 7 digits
                "0541234567",     // 05 + Mobily prefix 4 + 7 digits
                "0591234567",     // 05 + Zain prefix 9 + 7 digits
                "0581234567",     // 05 + Zain prefix 8 + 7 digits
                "0571234567",     // 05 + MVNO prefix 7 + 7 digits
                "0511234567",     // 05 + Bravo prefix 1 + 7 digits
                "+9665551234567", // +9665 + STC prefix 5 + 7 digits
                "9665551234567",  // 9665 + STC prefix 5 + 7 digits
                "009665551234567", // 009665 + STC prefix 5 + 7 digits
                "551234567"       // 5 + STC prefix 5 + 7 digits (9 total)
            };
            
            // Invalid test cases
            string[] invalidNumbers = {
                "0522345678",     // Invalid telecom prefix (2)
                "0451234567",     // Wrong area code (04)
                "05123456789",    // Too long (extra digit)
                "051234567",      // Too short (missing digit)
                "0512345abc",     // Contains letters
                "05-123-4567",    // Contains dashes
                "05 123 4567",    // Contains spaces
                "+96622345678",   // Invalid telecom prefix in international
                "123456789",      // Random number
                ""                // Empty
            };
            
            Console.WriteLine("\nTesting Valid Numbers:");
            Console.WriteLine("=====================");
            foreach (var number in validNumbers)
            {
                bool isValid = Regex.IsMatch(number, saudiMobilePattern);
                Console.WriteLine($"{number,-18} -> {(isValid ? "✓ VALID" : "✗ INVALID")}");
                if (!isValid)
                {
                    Console.WriteLine($"  ERROR: Expected valid but got invalid!");
                }
            }
            
            Console.WriteLine("\nTesting Invalid Numbers:");
            Console.WriteLine("=======================");
            foreach (var number in invalidNumbers)
            {
                bool isValid = Regex.IsMatch(number, saudiMobilePattern);
                Console.WriteLine($"{number,-18} -> {(isValid ? "✗ VALID (ERROR)" : "✓ INVALID")}");
                if (isValid)
                {
                    Console.WriteLine($"  ERROR: Expected invalid but got valid!");
                }
            }
            
            Console.WriteLine("\nTest completed!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
