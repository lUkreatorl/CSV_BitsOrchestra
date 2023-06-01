using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace CSV_Backend.Models;

public class CustomDateTimeConverter : ITypeConverter
{
    public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (DateTime.TryParseExact(text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            return result;
        
        // Handle conversion failure, return a default value, or throw an exception
        return default(DateTime);
    }

    public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        if (value is DateTime dateTime)
            return dateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        
        // Handle conversion failure, return an empty string, or throw an exception
        return string.Empty;
    }
}