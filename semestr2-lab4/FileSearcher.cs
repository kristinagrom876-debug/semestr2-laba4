using System;
using System.IO;
using System.Collections.Generic;

namespace TextEditorApp {
  public class FileSearcher {
    private string searchDirectory;
    private List<string> foundFiles;

    private int MaxFoundFiles;
    private int InitialCount;
    private int FileNumberOffset;

    public FileSearcher()
    {
      searchDirectory = ".";
      foundFiles = new List<string>();

      MaxFoundFiles = 1000;
      InitialCount = 0;
      FileNumberOffset = 1;
    }

    public FileSearcher(string directory)
    {
      searchDirectory = directory;
      foundFiles = new List<string>();

      MaxFoundFiles = 1000;
      InitialCount = 0;
      FileNumberOffset = 1;
    }

    public void SetSearchDirectory(string directory)
    {
      searchDirectory = directory;
    }

    public string GetSearchDirectory()
    {
      return searchDirectory;
    }

    public void SearchByName(string keyword)
    {
      foundFiles.Clear();

      string[] files;
      int fileCounter;
      string filePath;
      string fileName;

      files = Directory.GetFiles(searchDirectory);
      fileCounter = 0;

      if (files != null)
      {
        for (int fileIndex = 0; fileIndex < files.Length; fileIndex++)
        {
          if (fileCounter >= MaxFoundFiles)
          {
            break;
          }

          filePath = files[fileIndex];
          fileName = Path.GetFileName(filePath);

          if (fileName.IndexOf(keyword) >= 0)
          {
            foundFiles.Add(filePath);
          }

          fileCounter++;
        }
      }
    }

    public void SearchInContent(string keyword)
    {
      foundFiles.Clear();

      string[] files;
      int fileCounter;
      string filePath;
      string extension;
      string content;

      files = Directory.GetFiles(searchDirectory);
      fileCounter = 0;

      if (files != null)
      {
        for (int fileIndex = 0; fileIndex < files.Length; fileIndex++)
        {
          if (fileCounter >= MaxFoundFiles)
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
              foundFiles.Add(filePath);
            }
          }

          fileCounter++;
        }
      }
    }

    public void PrintFoundFiles()
    {
      if (foundFiles.Count == InitialCount)
      {
        Console.WriteLine("No files found");
        return;
      }

      Console.WriteLine("Found files: " + foundFiles.Count);

      for (int fileIndex = 0; fileIndex < foundFiles.Count; fileIndex++)
      {
        Console.WriteLine("  " + (fileIndex + FileNumberOffset) + ". " + foundFiles[fileIndex]);
      }
    }

    public List<string> GetFoundFiles()
    {
      return foundFiles;
    }
  }
}