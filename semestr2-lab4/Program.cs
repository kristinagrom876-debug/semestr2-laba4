using System;

namespace TextEditorApp {
  public class Program {
    private int maxKeywords;
    private int initialKeywordCount;
    private int editFileChoice;
    private int undoChoice;
    private int saveAsXmlChoice;
    private int saveAsBinaryChoice;
    private int loadFromXmlChoice;
    private int loadFromBinaryChoice;
    private int searchFilesChoice;
    private int indexFilesChoice;
    private int exitChoice;
    private int yesChoice;

    public static void Main(string[] args)
    {
      Program program = new Program();
      program.Start();
    }

    public void Start()
    {
      maxKeywords = 10;
      initialKeywordCount = 0;
      editFileChoice = 1;
      undoChoice = 2;
      saveAsXmlChoice = 3;
      saveAsBinaryChoice = 4;
      loadFromXmlChoice = 5;
      loadFromBinaryChoice = 6;
      searchFilesChoice = 7;
      indexFilesChoice = 8;
      exitChoice = 0;
      yesChoice = 1;

      TextEditor editor = new TextEditor();
      FileSearcher searcher = new FileSearcher();
      FileIndexer indexer = new FileIndexer();

      string filePath;
      string searchKeyword;
      string[] keywordsForIndex = new string[maxKeywords];
      int keywordCount;
      int userChoice;
      bool programRunning;

      programRunning = true;

      Console.WriteLine();
      Console.WriteLine("TEXT EDITOR WITH FILE SEARCH");
      Console.WriteLine();

      Console.WriteLine("First, enter a file path to work with:");
      Console.Write("File path: ");
      filePath = Console.ReadLine();

      editor.CreateNewFile(filePath);

      while (programRunning)
      {
        Console.WriteLine();
        Console.WriteLine("MENU:");
        Console.WriteLine("1. Edit file content");
        Console.WriteLine("2. Undo changes (Memento pattern)");
        Console.WriteLine("3. Save as XML (serialization)");
        Console.WriteLine("4. Save as binary (serialization)");
        Console.WriteLine("5. Load from XML (deserialization)");
        Console.WriteLine("6. Load from binary (deserialization)");
        Console.WriteLine("7. Search files by keyword");
        Console.WriteLine("8. Index files in directory");
        Console.WriteLine("0. Exit");
        Console.Write("Choice: ");

        userChoice = int.Parse(Console.ReadLine());

        if (userChoice == editFileChoice)
        {
          editor.EditContent();
        }
        else if (userChoice == undoChoice)
        {
          editor.Undo();
        }
        else if (userChoice == saveAsXmlChoice)
        {
          bool saveResult = editor.SaveAsXml();

          if (saveResult)
          {
            Console.WriteLine("File saved as XML");
          }
        }
        else if (userChoice == saveAsBinaryChoice)
        {
          bool saveResult = editor.SaveAsBinary();

          if (saveResult)
          {
            Console.WriteLine("File saved as binary");
          }
        }
        else if (userChoice == loadFromXmlChoice)
        {
          Console.Write("Enter XML file path to load: ");
          filePath = Console.ReadLine();

          editor.LoadFromXml(filePath);
        }
        else if (userChoice == loadFromBinaryChoice)
        {
          Console.Write("Enter binary file path to load: ");
          filePath = Console.ReadLine();

          editor.LoadFromBinary(filePath);
        }
        else if (userChoice == searchFilesChoice)
        {
          Console.Write("Enter directory to search in: ");
          filePath = Console.ReadLine();

          searcher.SetSearchDirectory(filePath); 

          Console.Write("Enter keyword to search: ");
          searchKeyword = Console.ReadLine();

          Console.WriteLine();
          Console.WriteLine("1. Search in file names");
          Console.WriteLine("2. Search in file content");
          Console.Write("Choice: ");

          int searchType = int.Parse(Console.ReadLine());

          if (searchType == 1)
          {
            searcher.SearchByName(searchKeyword);
          }
          else
          {
            searcher.SearchInContent(searchKeyword);
          }

          searcher.PrintFoundFiles();
        }
        else if (userChoice == indexFilesChoice)
        {
          Console.Write("Enter directory to index: ");
          filePath = Console.ReadLine();
          indexer.SetIndexDirectory(filePath);

          keywordCount = initialKeywordCount;

          Console.WriteLine("Enter keywords for indexing (enter empty line to finish):");

          while (keywordCount < maxKeywords)
          {
            Console.Write("    Keyword " + (keywordCount + 1) + ": ");
            searchKeyword = Console.ReadLine();

            if (searchKeyword == string.Empty)
            {
              break;
            }

            keywordsForIndex[keywordCount] = searchKeyword;
            keywordCount++;
          }

          if (keywordCount > initialKeywordCount)
          {
            indexer.IndexFilesByKeywords(keywordsForIndex, keywordCount);

            Console.Write("Save results to file? (1 - yes, 0 - no): ");
            int saveChoice = int.Parse(Console.ReadLine());

            if (saveChoice == yesChoice)
            {
              Console.Write("Enter filename for results: ");
              filePath = Console.ReadLine();
              indexer.SaveIndexResults(filePath);
            }
          }
          else
          {
            Console.WriteLine("No keywords specified");
          }
        }
        else if (userChoice == exitChoice)
        {
          programRunning = false;
          Console.WriteLine("Program finished");
        }
        else
        {
          Console.WriteLine("Invalid choice. Try again.");
        }
      }
    }
  }
}