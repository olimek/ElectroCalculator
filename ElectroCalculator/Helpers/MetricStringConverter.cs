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
            var match = Regex.Match(standardizedInput, @"^(?<value>-?\d+(?:,\d+)?)(?<prefix>[a-zA-Z]?)$");

            if (!match.Success)
                throw new FormatException("Input string is not in a valid format.");

            // Parse the numeric part
            string valuePart = match.Groups["value"].Value;
            string prefixPart = match.Groups["prefix"].Value;

            if (!float.TryParse(valuePart, NumberStyles.Float, CultureInfo.InvariantCulture, out float numericValue))
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
                    return $"{scaledValue:F2}{prefix.Value}";
                }
            }

            return value.ToString("F2"); // If no prefix applies, return plain value
        }
    }

}


