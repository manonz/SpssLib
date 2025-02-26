using System;
using System.Collections.Generic;
using System.Linq;
using Spss.FileStructure;

namespace Spss.SpssMetadata;

public abstract class Variable
{
    /// <summary>
    ///     Spss Name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    ///     Spss type
    /// </summary>
    public FormatType FormatType { get; set; } = FormatType.A;

    /// <summary>
    ///     Spss Width
    ///     string: Max valueLength in bytes
    ///     number: number of digit
    ///     date: length of date format
    /// </summary>
    public int SpssWidth { get; set; } = 1;

    /// <summary>
    ///     Number of decimal places
    /// </summary>
    public int DecimalPlaces { get; set; }

    /// <summary>
    ///     Spss label
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    ///     Spss Values (The labels for different values)
    /// </summary>
    public Dictionary<object, string>? ValueLabels { get; set; }

    /// <summary>
    ///     Spss Missing (type).
    /// </summary>
    public MissingValueType MissingValueType { get; set; } = MissingValueType.NoMissingValues;

    /// <summary>
    ///     Spss Missing (Values)
    /// </summary>
    public object[] MissingValues { get; set; } = Array.Empty<object>();

    /// <summary>
    ///     Spss Columns  (The display width)
    /// </summary>
    public int Columns { get; set; } = 5;

    /// <summary>
    ///     Spss alignment <see cref="Alignment" />
    /// </summary>
    public Alignment Alignment { get; set; } = Alignment.Left;

    /// <summary>
    ///     Spss Measure <see cref="MeasurementType" />
    /// </summary>
    public MeasurementType MeasurementType { get; set; } = MeasurementType.Nominal;

    /// <summary>
    ///     Is this set as the  weighting variable?
    /// </summary>
    public bool IsWeight { get; set; } = false;

    public static Variable Create(string name, string? label, FormatType formatType, int spssWidth, int decimalPlaces)
    {
        if (formatType == FormatType.A) return new Variable<string>(name, spssWidth, decimalPlaces) { Label = label };

        if (formatType.IsDate()) return new Variable<DateTime>(name, spssWidth, decimalPlaces) { Label = label };

        return new Variable<double>(name, spssWidth, decimalPlaces) { Label = label };
    }
}

#pragma warning disable SA1402

public class Variable<T> : Variable where T : notnull
#pragma warning restore SA1402
{
    public Variable(string name, int spssWidth, int decimalPlaces = 0)
    {
        Name = name;
        if (typeof(string) != typeof(T)) Alignment = Alignment.Right;

        if (typeof(DateTime) == typeof(T) || typeof(DateTime?) == typeof(T)) FormatType = FormatType.DATE;

        if (typeof(double) == typeof(T) || typeof(double?) == typeof(T)) FormatType = FormatType.F;

        SpssWidth = spssWidth;
        if (typeof(double) == typeof(T) || typeof(double?) == typeof(T)) DecimalPlaces = decimalPlaces;
    }

    public new Dictionary<T, string>? ValueLabels
    {
        get => base.ValueLabels?.ToDictionary(x => (T)x.Key, x => x.Value);
        set => base.ValueLabels = value?.ToDictionary(x => typeof(T) == typeof(DateTime) ? Convert.ToDateTime(x.Key).SpssDate() : (object)x.Key, x => x.Value);
    }

    public Variable<T> UserMissingValues(T[] missingValues)
    {
        var length = missingValues.Length;
        if (length > 3) length = 3;

        return UserMissingValues((MissingValueType)length, missingValues[..length]);
    }

    public Variable<T> UserMissingValues(MissingValueType missingValueType, T[] missingValues)
    {
        if (Math.Abs((int)missingValueType) != missingValues.Length) throw new InvalidOperationException($"Expected number of missing {Math.Abs((int)missingValueType)}!={missingValues.Length}");

        MissingValueType = missingValueType;
        if (typeof(DateTime) == typeof(T) || typeof(DateTime?) == typeof(T))
            MissingValues = missingValues.Cast<DateTime>().Select(x => (object)x.SpssDate()).ToArray();
        else
            MissingValues = missingValues.Cast<object>().ToArray();

        return this;
    }
}
