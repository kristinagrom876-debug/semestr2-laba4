using System;
using System.IO;
using System.Collections.Generic;

namespace TextEditorApp {
  public class FileIndexer {
    private string indexDirectory;
    private List<string> indexedFiles;

    private int maxIndexedFiles;
    private int initialCount;
    private int fileNumberOffset;

    public FileIndexer()
    {
      indexDirectory = ".";
      indexedFiles = new List<string>();

      maxIndexedFiles = 1000;
      initialCount = 0;
      fileNumberOffset = 1;
    }

    public FileIndexer(string directory)
    {
      indexDirectory = directory;
      indexedFiles = new List<string>();

      maxIndexedFiles = 1000;
      initialCount = 0;
      fileNumberOffset = 1;
    }

    public void SetIndexDirectory(string directory)
    {
      indexDirectory = directory;
    }

    public string GetIndexDirectory()
    {
      return indexDirectory;
    }

    public void IndexFilesByKeywords(string[] keywords, int keywordCount)
    {
      indexedFiles.Clear();

      Console.WriteLine("Indexing files in: " + indexDirectory);
      Console.Write("Keywords: ");

      for (int keywordIndex = 0; keywordIndex < keywordCount; keywordIndex++)
      {
        Console.Write(keywords[keywordIndex]);

        if (keywordIndex < keywordCount - 1)
        {
          Console.Write(", ");
        }
      }
      Console.WriteLine();

      string[] allFiles = Directory.GetFiles(indexDirectory, "*.*", SearchOption.AllDirectories);

      int fileCounter = 0;

      for (int fileIndex = 0; fileIndex < allFiles.Length; fileIndex++)
      {
        if (fileCounter >= maxIndexedFiles)
        {
          break;
        }

        string filePath = allFiles[fileIndex];
        string extension = Path.GetExtension(filePath).ToLower();

        if (extension == ".txt" || extension == ".cs" || extension == ".xml")
        {
          using (StreamReader reader = new StreamReader(filePath))
          {
            string content = reader.ReadToEnd();

            bool fileMatches = false;

            for (int keywordIndex = 0; keywordIndex < keywordCount; keywordIndex++)
            {
              if (content.Contains(keywords[keywordIndex]))
              {
                fileMatches = true;
                break;
              }
            }

            if (fileMatches)
            {
              indexedFiles.Add(filePath);
              Console.WriteLine("  Found: " + filePath);
            }
          }
        }

        fileCounter++;
      }

      Console.WriteLine("Indexing complete. Files found: " + indexedFiles.Count);
    }

    public void PrintIndexedFiles()
    {
      if (indexedFiles.Count == initialCount)
      {
        Console.WriteLine("No indexed files");
        return;
      }

      Console.WriteLine("Indexed files: " + indexedFiles.Count);

      for (int fileIndex = 0; fileIndex < indexedFiles.Count; fileIndex++)
      {
        Console.WriteLine("  " + (fileIndex + fileNumberOffset) + ". " + indexedFiles[fileIndex]);
      }
    }

    public bool SaveIndexResults(string outputFilePath)
    {
      using (StreamWriter outputFile = new StreamWriter(outputFilePath))
      {
        outputFile.WriteLine("Indexing results for: " + indexDirectory);
        outputFile.WriteLine("Files found: " + indexedFiles.Count);

        for (int fileIndex = 0; fileIndex < indexedFiles.Count; fileIndex++)
        {
          outputFile.WriteLine((fileIndex + fileNumberOffset) + ". " + indexedFiles[fileIndex]);
        }
      }

      Console.WriteLine("Results saved to: " + outputFilePath);
      return true;
    }

    public List<string> GetIndexedFiles()
    {
      return indexedFiles;
    }
  }
}