# C# SPSS SAV file reader and writer library


This library enables to write SPSS data files (.sav). The library is UTF-8 safe.

It is available as a nuget package at  https://www.nuget.org/packages/manonz.SpssWriter, and can be installed using the package manager or by issuing:

```
Install-Package manonz.SpssCommon
Install-Package manonz.SpssWriter
```

It's a fork of https://github.com/Anderman/Medella.SPSS
adding the ability to handle multiple-response variables.



### To write a data file:

```C#
// Create Variable list
    var variables = new List<Variable>
    {
        new Variable<string>("var0", 8)
        {
            Label = "label for var0",
            ValueLabels = new Dictionary<string, string>
            {
                ["a"] = "valueLabel for a",
                ["b"] = "valueLabel for b"
            }
        }.UserMissingValues(MissingValueType.OneDiscreteMissingValue, new[] { "-" }),
        new Variable<double>("var2", 7, 3)
        {
            Label = "label for var2",
            ValueLabels = new Dictionary<double, string>
            {
                [15.5] = "valueLabel for 15.5",
                [16] = "valueLabel for 16"
            }
        }.UserMissingValues(new[] { 0d }),
        new Variable<DateTime>("var4", 20)
        {
            Label = "label for var4",
            ValueLabels = new Dictionary<DateTime, string>
            {
                [new DateTime(2020, 1, 2)] = "valueLabel for 2 jan 2020",
                [new DateTime(2020, 1, 1)] = "valueLabel for 1 jan 2020"
            }
        }
    };
    // create data
    var data = new List<object?>
    {
        "string", 15.5, new DateTime(2020, 1, 1),
        null, null, null
    };

    //Write variable and data to a stream
    var ms = new MemoryStream();
    SpssWriter.Write(variables, data, ms);
```

If you find any bugs or have issues, please open an issue on GitHub.

## SAV file format

Binary description of `*.sav` file format is available here: http://www.gnu.org/software/pspp/pspp-dev/html_node/System-File-Format.html.

