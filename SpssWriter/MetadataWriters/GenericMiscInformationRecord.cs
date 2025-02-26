namespace Spss.MetadataWriters;

class GenericMiscInformationRecord
{
    public int Subtype { get; set; }

    public int Size { get; set; }

    public int Count { get; set; }

    public byte[] Data { get; set; } = null!;
}
