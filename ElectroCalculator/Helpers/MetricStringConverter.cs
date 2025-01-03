using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ElectroCalculator.Helpers
{
    public class MetricStringConverter
    {
        private static readonly Dictionary<string, float> MetricPrefixes = new()
    {
        { "k", 1_000f },    // kilo
        { "M", 1_000_000f }, // Mega
        { "G", 1_000_000_000f }, // Giga
        { "m", 0.001f },    // mili
        { "u", 0.000_001f }, // mikro
        { "n", 0.000_000_001f }, // nano
        { "p", 0.000_000_000_001f }, // pico
    };


        public static float ConvertToFloat(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be null or empty.");

            // Replace dot with a comma for decimal separator
            string standardizedInput = input.Replace('.', ',');

            // Regular expression to extract numeric value and optional metric prefix
            var match = Regex.Match(standardizedInput, @"^(?<value>-?\d+(?:,\d+)?)(?<prefix>[a-zA-Z]*)$|^(?<value1>-?\d+(?:,\d+)?)(?<prefix1>[a-zA-Z])(?<value2>\d+(?:,\d+)?)$");

            if (match.Success)
            {
                if (match.Groups["value"].Success)
                {
                    Console.WriteLine($"Value: {match.Groups["value"].Value}, Prefix: {match.Groups["prefix"].Value}");
                }
                else if (match.Groups["value1"].Success && match.Groups["prefix1"].Success && match.Groups["value2"].Success)
                {
                    Console.WriteLine($"Value1: {match.Groups["value1"].Value}, Prefix: {match.Groups["prefix1"].Value}, Value2: {match.Groups["value2"].Value}");
                }
            }
            else
            {
                Console.WriteLine("No match found.");
            }

            if (!match.Success)
                throw new FormatException("Input string is not in a valid format.");

            // Parse the numeric part
            string valuePart = "0";
            string prefixPart = match.Groups["prefix"].Value;
            if (match.Groups["value"].Success)
            {
                valuePart = match.Groups["value"].Value;
            }
            else if (match.Groups["value1"].Success && match.Groups["prefix1"].Success && match.Groups["value2"].Success)
            {
                valuePart = match.Groups["value1"].Value+ match.Groups["value2"].Value;
            }
            if (!float.TryParse(valuePart.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out float numericValue))
                throw new FormatException("Unable to parse the numeric part of the input string.");

            // Apply metric prefix multiplier if applicable
            if (!string.IsNullOrEmpty(prefixPart))
            {
                if (MetricPrefixes.TryGetValue(prefixPart, out float multiplier))
                {
                    numericValue *= multiplier;
                }
                else
                {
                    throw new ArgumentException($"Unrecognized metric prefix: '{prefixPart}'");
                }
            }

            return numericValue;
        }

        public static string ConvertToMetricString(double value)
        {
            
            if (value >= 1)
            {
                int Intvalue = (int)Math.Round(value, 0);
                if (Intvalue - value < 0.001d)
                {
                    value = Intvalue;
                }
                if (1000000 > Intvalue && Intvalue >= 1000)
                {
                    value /= 1000;
                    string OUT = value.ToString("0.##");
                    return $"{(OUT)}k";
                }
                else if (1000000000 > Intvalue && Intvalue >= 1000000) //Mega
                {
                    value /= 1000000;
                    string OUT = value.ToString("0.##");
                    return $"{(OUT)}Meg";
                }
                else if (Intvalue >= 1000000000) //Giga
                {
                    value /= 1000000000;
                    string OUT = value.ToString("0.##");
                    return $"{(OUT)}G";
                }
                else
                {
                    string OUT = value.ToString("0.##");
                    return $"{OUT}";
                }
            }
            else
            { 

                if (0.999d >= value && value > 0.0001d) //mili
                {
                    value *= 1000;
                    string OUT = value.ToString("0.##");
                    return $"{(OUT)}m";
                }
                else if (0.0001d >= value && value > 0.0000001d) //micro
                {
                    value *= 1000000;
                    string OUT = value.ToString("0.##");
                    return $"{(OUT)}u";
                }
                else if (0.0000001d >= value && value > 0.0000000001d) //nano
                {
                    value *= 1000000000;
                    string OUT = value.ToString("0.##");
                    return $"{(OUT)}n";
                }
                else if (0.0000000001d >= value) //pico
                {
                    value *= 1000000000000;
                    string OUT = value.ToString("0.##");
                    return $"{(OUT)}p";
                }
                else
                {
                    string OUT = value.ToString("0.##");
                    return $"{OUT}";
                }
            }
        }
    }

}


