using System;
using System.IO;
using System.Xml;

namespace TextEditorApp {
  public class TextFile {
    private string filePath;
    private string fileContent;
    private int maxFileSize;
    private int zeroValue;

    public TextFile()
    {
      filePath = string.Empty;
      fileContent = string.Empty;
      maxFileSize = 1000000;
      zeroValue = 0;
    }

    public TextFile(string path)
    {
      filePath = path;
      fileContent = string.Empty;
      maxFileSize = 1000000;
      zeroValue = 0;
    }

    public void SetFilePath(string path)
    {
      filePath = path;
    }

    public string GetFilePath()
    {
      return filePath;
    }

    public void SetFileContent(string content)
    {
      fileContent = content;
    }

    public string GetFileContent()
    {
      return fileContent;
    }

    public bool LoadFromTextFile()
    {
      if (filePath == string.Empty)
      {
        Console.WriteLine("Error: file path not set");
        return false;
      }

      if (File.Exists(filePath) == false)
      {
        Console.WriteLine("Error: file does not exist");
        return false;
      }

      StreamReader reader;
      reader = new StreamReader(filePath);
      fileContent = reader.ReadToEnd();
      reader.Close();

      return true;
    }

    public bool SaveToTextFile()
    {
      if (filePath == string.Empty)
      {
        Console.WriteLine("Error: file path not set");
        return false;
      }

      StreamWriter writer;
      writer = new StreamWriter(filePath);
      writer.Write(fileContent);
      writer.Close();

      return true;
    }

    public bool SaveToXml(string xmlFilePath)
    {
      XmlDocument xmlDocument;
      XmlDeclaration xmlDeclaration;
      XmlElement documentElement;
      XmlElement contentElement;

      xmlDocument = new XmlDocument();
      xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
      xmlDocument.AppendChild(xmlDeclaration);

      documentElement = xmlDocument.CreateElement("document");
      xmlDocument.AppendChild(documentElement);

      contentElement = xmlDocument.CreateElement("content");
      contentElement.InnerText = fileContent;
      documentElement.AppendChild(contentElement);

      xmlDocument.Save(xmlFilePath);

      return true;
    }

    public bool LoadFromXml(string xmlFilePath)
    {
      XmlDocument xmlDocument;
      XmlNode contentNode;

      if (File.Exists(xmlFilePath) == false)
      {
        Console.WriteLine("Error: XML file does not exist");
        return false;
      }

      xmlDocument = new XmlDocument();
      xmlDocument.Load(xmlFilePath);

      contentNode = xmlDocument.SelectSingleNode("//content");

      if (contentNode != null)
      {
        fileContent = contentNode.InnerText;
        return true;
      }

      return false;
    }

    public bool SaveToBinary(string binaryFilePath)
    {
      BinaryWriter binaryWriter;
      FileStream fileStream;
      int contentLength;

      fileStream = File.Open(binaryFilePath, FileMode.Create);
      binaryWriter = new BinaryWriter(fileStream);

      contentLength = fileContent.Length;
      binaryWriter.Write(contentLength);
      binaryWriter.Write(fileContent);

      binaryWriter.Close();
      fileStream.Close();

      return true;
    }

    public bool LoadFromBinary(string binaryFilePath)
    {
      BinaryReader binaryReader;
      FileStream fileStream;
      int contentLength;

      if (File.Exists(binaryFilePath) == false)
      {
        Console.WriteLine("Error: binary file does not exist");
        return false;
      }

      fileStream = File.Open(binaryFilePath, FileMode.Open);
      binaryReader = new BinaryReader(fileStream);

      contentLength = binaryReader.ReadInt32();

      if (contentLength <= zeroValue)
      {
        Console.WriteLine("Error: invalid content length");
        binaryReader.Close();
        fileStream.Close();
        return false;
      }

      if (contentLength > maxFileSize)
      {
        Console.WriteLine("Error: file too large");
        binaryReader.Close();
        fileStream.Close();
        return false;
      }

      fileContent = binaryReader.ReadString();
      binaryReader.Close();
      fileStream.Close();

      return true;
    }
  }
}