using System;
using System.IO;
using System.Collections.Generic;

namespace TextEditorApp {
  public class FileIndexer {
    private string _indexDirectory;
    private List<string> _indexedFiles;

    private int _maxIndexedFiles;
    private int _initialCount;
    private int _fileNumberOffset;

    public FileIndexer()
    {
      _indexDirectory = ".";
      _indexedFiles = new List<string>();

      _maxIndexedFiles = 1000;
      _initialCount = 0;
      _fileNumberOffset = 1;
    }

    public FileIndexer(string directory)
    {
      _indexDirectory = directory;
      _indexedFiles = new List<string>();

      _maxIndexedFiles = 1000;
      _initialCount = 0;
      _fileNumberOffset = 1;
    }

    public void SetIndexDirectory(string directory)
    {
      _indexDirectory = directory;
    }

    public string GetIndexDirectory()
    {
      return _indexDirectory;
    }

    public void IndexFilesByKeywords(string[] keywords, int keywordCount)
    {
      int fileCounter;
      string filePath;
      string extension;
      string content;
      string[] allFiles;
      bool fileMatches;
      int lastKeywordOffset;

      _indexedFiles.Clear();

      Console.WriteLine("Indexing files in: " + _indexDirectory);
      Console.Write("Keywords: ");

      lastKeywordOffset = 1;

      for (int keywordIndex = 0; keywordIndex < keywordCount; ++keywordIndex)
      {
        Console.Write(keywords[keywordIndex]);

        if (keywordIndex < keywordCount - lastKeywordOffset)
        {
          Console.Write(", ");
        }
      }
      Console.WriteLine();

      allFiles = Directory.GetFiles(_indexDirectory, "*.*", SearchOption.AllDirectories);

      fileCounter = 0;

      for (int fileIndex = 0; fileIndex < allFiles.Length; ++fileIndex)
      {
        if (fileCounter >= _maxIndexedFiles)
        {
          break;
        }

        filePath = allFiles[fileIndex];
        extension = Path.GetExtension(filePath).ToLower();

        if (extension == ".txt" || extension == ".cs" || extension == ".xml")
        {
          using (StreamReader reader = new StreamReader(filePath))
          {
            content = reader.ReadToEnd();

            fileMatches = false;

            for (int keywordIndex = 0; keywordIndex < keywordCount; ++keywordIndex)
            {
              if (content.Contains(keywords[keywordIndex]))
              {
                fileMatches = true;
                break;
              }
            }

            if (fileMatches)
            {
              _indexedFiles.Add(filePath);
              Console.WriteLine("  Found: " + filePath);
            }
          }
        }

        ++fileCounter;
      }

      Console.WriteLine("Indexing complete. Files found: " + _indexedFiles.Count);
    }

    public void PrintIndexedFiles()
    {
      if (_indexedFiles.Count == _initialCount)
      {
        Console.WriteLine("No indexed files");
        return;
      }

      Console.WriteLine("Indexed files: " + _indexedFiles.Count);

      for (int fileIndex = 0; fileIndex < _indexedFiles.Count; ++fileIndex)
      {
        Console.WriteLine("  " + (fileIndex + _fileNumberOffset) + ". " + _indexedFiles[fileIndex]);
      }
    }

    public bool SaveIndexResults(string outputFilePath)
    {
      using (StreamWriter outputFile = new StreamWriter(outputFilePath))
      {
        outputFile.WriteLine("Indexing results for: " + _indexDirectory);
        outputFile.WriteLine("Files found: " + _indexedFiles.Count);

        for (int fileIndex = 0; fileIndex < _indexedFiles.Count; ++fileIndex)
        {
          outputFile.WriteLine((fileIndex + _fileNumberOffset) + ". " + _indexedFiles[fileIndex]);
        }
      }

      Console.WriteLine("Results saved to: " + outputFilePath);
      return true;
    }

    public List<string> GetIndexedFiles()
    {
      return _indexedFiles;
    }
  }
}