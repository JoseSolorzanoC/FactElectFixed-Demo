using System.Globalization;

namespace FactElectFixed.Api.Helpers;

public static class ValidationHelpers
{
    public static bool HasPrecisionAndScale(decimal value, int precision, int scale)
    {
        // Usa representación en forma positiva sin signo
        value = Math.Abs(value);

        // Convertir a string en formato invariable con punto decimal
        string s = value.ToString(CultureInfo.InvariantCulture);

        if (s.Contains('E') || s.Contains('e'))
        {
            s = value.ToString("0.############################", CultureInfo.InvariantCulture);
        }

        string[] parts = s.Split('.');
        string intPart = parts[0].TrimStart('0'); // quitar ceros a la izquierda para contar correctamente
        string fracPart = parts.Length > 1 ? parts[1] : string.Empty;

        int intDigits = intPart.Length == 0 ? 1 : intPart.Length;
        int fracDigits = fracPart.Length;

        int totalDigits = intDigits + fracDigits;

        return totalDigits <= precision && fracDigits <= scale;
    }

    public static bool IsDate_ddMMyyyy(string dateStr)
    {
        if (string.IsNullOrWhiteSpace(dateStr))
        {
            return false;
        }

        return DateTime.TryParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out _);
    }
    public static bool IsPeriod_MMyyyy(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return false;
        }

        return DateTime.TryParseExact("01/" + s, "dd/MM/yyyy", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out _);
    }

    public static bool IsDigitsOnly(string s)
    {
        return !string.IsNullOrEmpty(s) && s.All(char.IsDigit);
    }
}
