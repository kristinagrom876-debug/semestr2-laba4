using System;
using System.IO;
using System.Xml;

namespace TextEditorApp {
  public class TextFile {
    private string _filePath;
    private string _fileContent;
    private int _maxFileSize;
    private int _zeroValue;

    public TextFile()
    {
      _filePath = string.Empty;
      _fileContent = string.Empty;
      _maxFileSize = 1000000;
      _zeroValue = 0;
    }

    public TextFile(string path)
    {
      _filePath = path;
      _fileContent = string.Empty;
      _maxFileSize = 1000000;
      _zeroValue = 0;
    }

    public void SetFilePath(string path)
    {
      _filePath = path;
    }

    public string GetFilePath()
    {
      return _filePath;
    }

    public void SetFileContent(string content)
    {
      _fileContent = content;
    }

    public string GetFileContent()
    {
      return _fileContent;
    }

    public bool LoadFromTextFile()
    {
      if (_filePath == string.Empty)
      {
        Console.WriteLine("Error: file path not set");
        return false;
      }

      if (File.Exists(_filePath) == false)
      {
        Console.WriteLine("Error: file does not exist");
        return false;
      }

      StreamReader reader;
      reader = new StreamReader(_filePath);
      _fileContent = reader.ReadToEnd();
      reader.Close();

      return true;
    }

    public bool SaveToTextFile()
    {
      if (_filePath == string.Empty)
      {
        Console.WriteLine("Error: file path not set");
        return false;
      }

      StreamWriter writer;
      writer = new StreamWriter(_filePath);
      writer.Write(_fileContent);
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
      contentElement.InnerText = _fileContent;
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
        _fileContent = contentNode.InnerText;
        return true;
      }

      return false;
    }

    public bool SaveToBinary(string binaryFilePath)
    {
      FileStream fileStream;
      BinaryWriter binaryWriter;
      int contentLength;

      fileStream = File.Open(binaryFilePath, FileMode.Create);
      binaryWriter = new BinaryWriter(fileStream);

      contentLength = _fileContent.Length;
      binaryWriter.Write(contentLength);
      binaryWriter.Write(_fileContent);

      binaryWriter.Close();
      fileStream.Close();

      return true;
    }

    public bool LoadFromBinary(string binaryFilePath)
    {
      FileStream fileStream;
      BinaryReader binaryReader;
      int contentLength;

      if (File.Exists(binaryFilePath) == false)
      {
        Console.WriteLine("Error: binary file does not exist");
        return false;
      }

      fileStream = File.Open(binaryFilePath, FileMode.Open);
      binaryReader = new BinaryReader(fileStream);

      contentLength = binaryReader.ReadInt32();

      if (contentLength <= _zeroValue)
      {
        Console.WriteLine("Error: invalid content length");
        binaryReader.Close();
        fileStream.Close();
        return false;
      }

      if (contentLength > _maxFileSize)
      {
        Console.WriteLine("Error: file too large");
        binaryReader.Close();
        fileStream.Close();
        return false;
      }

      _fileContent = binaryReader.ReadString();
      binaryReader.Close();
      fileStream.Close();

      return true;
    }
  }
}