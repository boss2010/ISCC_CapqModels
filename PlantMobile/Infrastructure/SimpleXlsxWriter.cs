using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace PlantMobile.Infrastructure;

/// <summary>
/// Minimal, dependency-free XLSX writer: one sheet, inline strings, numeric cells,
/// RTL direction, frozen header row and landscape/fit-to-width printing.
/// Used by the committee inspection result export so the mobile project needs no Excel package.
/// </summary>
public sealed class SimpleXlsxWriter
{
    private readonly List<List<XlsxCell>> _rows = new();
    private readonly List<double> _widths = new();
    private readonly string _sheetName;

    public SimpleXlsxWriter(string sheetName)
    {
        var name = string.IsNullOrWhiteSpace(sheetName) ? "Sheet1" : sheetName.Trim();
        foreach (var invalid in new[] { ':', '\\', '/', '?', '*', '[', ']' })
            name = name.Replace(invalid, ' ');
        _sheetName = name.Length > 31 ? name[..31] : name;
    }

    public void SetColumnWidths(IEnumerable<double> widths)
    {
        _widths.Clear();
        _widths.AddRange(widths);
    }

    public void AddHeader(IReadOnlyList<string> headers) =>
        _rows.Add(headers.Select(XlsxCell.OfText).ToList());

    public void AddRow(IReadOnlyList<XlsxCell> cells) => _rows.Add(cells.ToList());

    public byte[] Build()
    {
        using var buffer = new MemoryStream();
        using (var archive = new ZipArchive(buffer, ZipArchiveMode.Create, leaveOpen: true))
        {
            Write(archive, "[Content_Types].xml", ContentTypesXml);
            Write(archive, "_rels/.rels", RootRelationshipsXml);
            Write(archive, "xl/workbook.xml", WorkbookXml());
            Write(archive, "xl/_rels/workbook.xml.rels", WorkbookRelationshipsXml);
            Write(archive, "xl/styles.xml", StylesXml);
            Write(archive, "xl/worksheets/sheet1.xml", SheetXml());
        }

        return buffer.ToArray();
    }

    private string WorkbookXml() =>
        "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
        "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" " +
        "xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
        "<sheets><sheet name=\"" + Escape(_sheetName) + "\" sheetId=\"1\" r:id=\"rId1\"/></sheets>" +
        "</workbook>";

    private string SheetXml()
    {
        var maxColumns = Math.Max(1, _rows.Count == 0 ? 1 : _rows.Max(r => r.Count));
        var lastColumn = ColumnName(maxColumns - 1);
        var lastRow = Math.Max(1, _rows.Count);

        var sb = new StringBuilder();
        sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
        sb.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
        sb.Append("<sheetPr><pageSetUpPr fitToPage=\"1\"/></sheetPr>");
        sb.Append("<dimension ref=\"A1:").Append(lastColumn).Append(lastRow).Append("\"/>");
        sb.Append("<sheetViews><sheetView rightToLeft=\"1\" tabSelected=\"1\" workbookViewId=\"0\">");
        sb.Append("<pane ySplit=\"1\" topLeftCell=\"A2\" activePane=\"bottomLeft\" state=\"frozen\"/>");
        sb.Append("</sheetView></sheetViews>");
        sb.Append("<sheetFormatPr defaultRowHeight=\"18\"/>");

        if (_widths.Count > 0)
        {
            sb.Append("<cols>");
            for (var i = 0; i < _widths.Count; i++)
            {
                sb.Append("<col min=\"").Append(i + 1).Append("\" max=\"").Append(i + 1)
                  .Append("\" width=\"").Append(_widths[i].ToString(CultureInfo.InvariantCulture))
                  .Append("\" customWidth=\"1\"/>");
            }
            sb.Append("</cols>");
        }

        sb.Append("<sheetData>");
        for (var r = 0; r < _rows.Count; r++)
        {
            var row = _rows[r];
            sb.Append(r == 0 ? "<row r=\"1\" ht=\"26\" customHeight=\"1\">" : "<row r=\"" + (r + 1) + "\">");
            for (var c = 0; c < row.Count; c++)
            {
                var cell = row[c];
                var reference = ColumnName(c) + (r + 1);
                if (cell.Number.HasValue)
                {
                    sb.Append("<c r=\"").Append(reference).Append("\" s=\"3\"><v>")
                      .Append(cell.Number.Value.ToString(CultureInfo.InvariantCulture))
                      .Append("</v></c>");
                }
                else
                {
                    sb.Append("<c r=\"").Append(reference).Append("\" s=\"").Append(r == 0 ? 1 : 2)
                      .Append("\" t=\"inlineStr\"><is><t xml:space=\"preserve\">")
                      .Append(Escape(cell.Text))
                      .Append("</t></is></c>");
                }
            }
            sb.Append("</row>");
        }
        sb.Append("</sheetData>");

        if (_rows.Count > 0)
            sb.Append("<autoFilter ref=\"A1:").Append(lastColumn).Append("1\"/>");

        sb.Append("<printOptions horizontalCentered=\"1\"/>");
        sb.Append("<pageMargins left=\"0.25\" right=\"0.25\" top=\"0.5\" bottom=\"0.5\" header=\"0.3\" footer=\"0.3\"/>");
        sb.Append("<pageSetup orientation=\"landscape\" paperSize=\"9\" fitToWidth=\"1\" fitToHeight=\"0\"/>");
        sb.Append("</worksheet>");
        return sb.ToString();
    }

    private static string ColumnName(int index)
    {
        var name = string.Empty;
        var value = index + 1;
        while (value > 0)
        {
            var remainder = (value - 1) % 26;
            name = (char)('A' + remainder) + name;
            value = (value - 1) / 26;
        }
        return name;
    }

    private static void Write(ZipArchive archive, string name, string content)
    {
        var entry = archive.CreateEntry(name, CompressionLevel.Optimal);
        using var stream = entry.Open();
        using var writer = new StreamWriter(stream, new UTF8Encoding(false));
        writer.Write(content);
    }

    private static string Escape(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        var sb = new StringBuilder(value.Length + 16);
        foreach (var ch in value)
        {
            switch (ch)
            {
                case '&': sb.Append("&amp;"); break;
                case '<': sb.Append("&lt;"); break;
                case '>': sb.Append("&gt;"); break;
                case '"': sb.Append("&quot;"); break;
                case '\r': break;
                case '\n': sb.Append("&#10;"); break;
                case '\t': sb.Append("&#9;"); break;
                default:
                    if (ch >= ' ')
                        sb.Append(ch);
                    break;
            }
        }
        return sb.ToString();
    }

    private const string ContentTypesXml =
        "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
        "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">" +
        "<Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>" +
        "<Default Extension=\"xml\" ContentType=\"application/xml\"/>" +
        "<Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>" +
        "<Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>" +
        "<Override PartName=\"/xl/styles.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml\"/>" +
        "</Types>";

    private const string RootRelationshipsXml =
        "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
        "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
        "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>" +
        "</Relationships>";

    private const string WorkbookRelationshipsXml =
        "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
        "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
        "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>" +
        "<Relationship Id=\"rId2\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" Target=\"styles.xml\"/>" +
        "</Relationships>";

    private const string StylesXml =
        "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
        "<styleSheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">" +
        "<fonts count=\"2\">" +
        "<font><sz val=\"11\"/><name val=\"Calibri\"/></font>" +
        "<font><b/><sz val=\"11\"/><color rgb=\"FFFFFFFF\"/><name val=\"Calibri\"/></font>" +
        "</fonts>" +
        "<fills count=\"3\">" +
        "<fill><patternFill patternType=\"none\"/></fill>" +
        "<fill><patternFill patternType=\"gray125\"/></fill>" +
        "<fill><patternFill patternType=\"solid\"><fgColor rgb=\"FF1F4E79\"/><bgColor indexed=\"64\"/></patternFill></fill>" +
        "</fills>" +
        "<borders count=\"2\">" +
        "<border><left/><right/><top/><bottom/><diagonal/></border>" +
        "<border><left style=\"thin\"><color rgb=\"FFBFBFBF\"/></left><right style=\"thin\"><color rgb=\"FFBFBFBF\"/></right>" +
        "<top style=\"thin\"><color rgb=\"FFBFBFBF\"/></top><bottom style=\"thin\"><color rgb=\"FFBFBFBF\"/></bottom><diagonal/></border>" +
        "</borders>" +
        "<cellStyleXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\"/></cellStyleXfs>" +
        "<cellXfs count=\"4\">" +
        "<xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\"/>" +
        "<xf numFmtId=\"0\" fontId=\"1\" fillId=\"2\" borderId=\"1\" xfId=\"0\" applyFont=\"1\" applyFill=\"1\" applyBorder=\"1\" applyAlignment=\"1\">" +
        "<alignment horizontal=\"center\" vertical=\"center\" wrapText=\"1\"/></xf>" +
        "<xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyBorder=\"1\" applyAlignment=\"1\">" +
        "<alignment vertical=\"top\" wrapText=\"1\"/></xf>" +
        "<xf numFmtId=\"4\" fontId=\"0\" fillId=\"0\" borderId=\"1\" xfId=\"0\" applyNumberFormat=\"1\" applyBorder=\"1\" applyAlignment=\"1\">" +
        "<alignment horizontal=\"center\" vertical=\"top\"/></xf>" +
        "</cellXfs>" +
        "<cellStyles count=\"1\"><cellStyle name=\"Normal\" xfId=\"0\" builtinId=\"0\"/></cellStyles>" +
        "</styleSheet>";
}

/// <summary>One spreadsheet cell: either inline text or a numeric value.</summary>
public sealed class XlsxCell
{
    public string? Text { get; private init; }
    public double? Number { get; private init; }

    public static XlsxCell OfText(string? value) => new() { Text = value };

    public static XlsxCell OfNumber(double? value) => new() { Number = value };
}
