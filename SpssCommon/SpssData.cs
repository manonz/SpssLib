using System.Collections.Generic;
using Spss.SpssMetadata;

namespace Spss;

public class SpssData
{
    public Metadata Metadata { get; set; } = null!;
    public List<object?> Data { get; set; } = new();
}