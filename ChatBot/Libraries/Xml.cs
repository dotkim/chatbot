using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ChatBot.Libraries
{
  /// <summary>
  /// Class used for processing XML data
  /// </summary>
  public static class Xml
  {
    /// <summary>
    /// Deserializes XML data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static T Deserialize<T>(string content) where T : class
    {
      var serializer = new XmlSerializer(typeof(T));
      using (var reader = new StringReader(content))
        return (T)serializer.Deserialize(reader);
    }

    /// <summary>
    /// Serialize to an XML file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void Serialize<T>(T obj, string path) where T : class
    {
      var serializer = new XmlSerializer(typeof(T));
      using (StreamWriter writer = new StreamWriter(path, false, Encoding.GetEncoding("utf-8")))
      {
        serializer.Serialize(writer, obj);
      }
    }
  }
}