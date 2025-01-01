using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        private static readonly Dictionary<float, string> ReverseMetricPrefixes = new()
    {
        { 1_000_000_000_000f, "T" }, // Tera
        { 1_000_000_000f, "G" },     // Giga
        { 1_000_000f, "M" },         // Mega
        { 1_000f, "k" },             // kilo
        { 1f, "" },                  // no prefix
        { 0.001f, "m" },             // mili
        { 0.000_001f, "u" },         // mikro
        { 0.000_000_001f, "n" },     // nano
        { 0.000_000_000_001f, "p" }  // pico
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

        public static string ConvertToMetricString(float value)
        {
            foreach (var prefix in ReverseMetricPrefixes)
            {
                if (Math.Abs(value) >= prefix.Key)
                {
                    float scaledValue = value / prefix.Key;

                    // If the scaled value is an integer, remove the decimal part
                    if (scaledValue == (int)scaledValue)
                    {
                        return $"{(int)scaledValue}{prefix.Value}"; // Return as an integer without decimals
                    }

                    return $"{scaledValue:F2}{prefix.Value}"; // Otherwise, return with 2 decimal places
                }
            }

            return value.ToString("F2"); // If no prefix applies, return plain value
        }
    }

}


