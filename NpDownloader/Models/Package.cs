using System.Xml.Serialization;

namespace NpDownloader.Models;

[XmlRoot(ElementName = "package")]
public class Package
{
    [XmlAttribute(AttributeName = "version")]
    public string Version { get; set; } = null!;

    [XmlAttribute(AttributeName = "size")]
    public string Size { get; set; } = null!;

    [XmlAttribute(AttributeName = "sha1sum")]
    public string Sha1Sum { get; set; } = null!;

    [XmlAttribute(AttributeName = "url")]
    public string Url { get; set; } = null!;

    [XmlAttribute(AttributeName = "ps3_system_ver")]
    public string Ps3SystemVer { get; set; } = null!;

    [XmlElement(ElementName = "paramsfo")]
    public ParamsSfo ParamSfo { get; set; } = new();
}