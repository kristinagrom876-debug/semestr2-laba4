using System;
using System.IO;
using System.Collections.Generic;

namespace TextEditorApp {
  public class FileSearcher {
    private string _searchDirectory;
    private List<string> _foundFiles;

    private int _maxFoundFiles;
    private int _initialCount;
    private int _fileNumberOffset;

    public FileSearcher()
    {
      _searchDirectory = ".";
      _foundFiles = new List<string>();

      _maxFoundFiles = 1000;
      _initialCount = 0;
      _fileNumberOffset = 1;
    }

    public FileSearcher(string directory)
    {
      _searchDirectory = directory;
      _foundFiles = new List<string>();

      _maxFoundFiles = 1000;
      _initialCount = 0;
      _fileNumberOffset = 1;
    }

    public void SetSearchDirectory(string directory)
    {
      _searchDirectory = directory;
    }

    public string GetSearchDirectory()
    {
      return _searchDirectory;
    }

    public void SearchByName(string keyword)
    {
      string[] files;
      int fileCounter;
      string filePath;
      string fileName;

      _foundFiles.Clear();

      files = Directory.GetFiles(_searchDirectory);
      fileCounter = 0;

      if (files != null)
      {
        for (int fileIndex = 0; fileIndex < files.Length; ++fileIndex)
        {
          if (fileCounter >= _maxFoundFiles)
          {
            break;
          }

          filePath = files[fileIndex];
          fileName = Path.GetFileName(filePath);

          if (fileName.IndexOf(keyword) >= 0)
          {
            _foundFiles.Add(filePath);
          }

          ++fileCounter;
        }
      }
    }

    public void SearchInContent(string keyword)
    {
      string[] files;
      int fileCounter;
      string filePath;
      string extension;
      string content;

      _foundFiles.Clear();

      files = Directory.GetFiles(_searchDirectory);
      fileCounter = 0;

      if (files != null)
      {
        for (int fileIndex = 0; fileIndex < files.Length; fileIndex++)
        {
          if (fileCounter >= _maxFoundFiles)
          {
            break;
          }

          filePath = files[fileIndex];
          extension = Path.GetExtension(filePath).ToLower();

          if (extension == ".txt" || extension == ".cs" || extension == ".xml")
          {
            content = File.ReadAllText(filePath);

            if (content.IndexOf(keyword) >= 0)
            {
              _foundFiles.Add(filePath);
            }
          }

          fileCounter++;
        }
      }
    }

    public void PrintFoundFiles()
    {
      if (_foundFiles.Count == _initialCount)
      {
        Console.WriteLine("No files found");
        return;
      }

      Console.WriteLine("Found files: " + _foundFiles.Count);

      for (int fileIndex = 0; fileIndex < _foundFiles.Count; ++fileIndex)
      {
        Console.WriteLine("  " + (fileIndex + _fileNumberOffset) + ". " + _foundFiles[fileIndex]);
      }
    }

    public List<string> GetFoundFiles()
    {
      return _foundFiles;
    }
  }
}