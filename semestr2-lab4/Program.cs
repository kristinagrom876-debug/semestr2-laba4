using System;

namespace TextEditorApp {
  public class Program {
    private int _maxKeywords;
    private int _initialKeywordCount;
    private int _editFileChoice;
    private int _undoChoice;
    private int _saveAsXmlChoice;
    private int _saveAsBinaryChoice;
    private int _loadFromXmlChoice;
    private int _loadFromBinaryChoice;
    private int _searchFilesChoice;
    private int _indexFilesChoice;
    private int _exitChoice;
    private int _yesChoice;
    private int _searchByNameChoice;
    private int _keywordNumberOffset;

    public static void Main(string[] args)
    {
      Program program;
      program = new Program();
      program.Start();
    }

    public void Start()
    {
      _maxKeywords = 10;
      _initialKeywordCount = 0;
      _editFileChoice = 1;
      _undoChoice = 2;
      _saveAsXmlChoice = 3;
      _saveAsBinaryChoice = 4;
      _loadFromXmlChoice = 5;
      _loadFromBinaryChoice = 6;
      _searchFilesChoice = 7;
      _indexFilesChoice = 8;
      _exitChoice = 0;
      _yesChoice = 1;
      _searchByNameChoice = 1;
      _keywordNumberOffset = 1;

      TextEditor editor;
      FileSearcher searcher;
      FileIndexer indexer;

      editor = new TextEditor();
      searcher = new FileSearcher();
      indexer = new FileIndexer();

      string filePath;
      string searchKeyword;
      string[] keywordsForIndex;
      int keywordCount;
      int userChoice;
      int saveChoice;
      int searchType;
      bool programRunning;
      bool saveResult;

      keywordsForIndex = new string[_maxKeywords];
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

        if (userChoice == _editFileChoice)
        {
          editor.EditContent();
        }
        else if (userChoice == _undoChoice)
        {
          editor.Undo();
        }
        else if (userChoice == _saveAsXmlChoice)
        {
          saveResult = editor.SaveAsXml();

          if (saveResult)
          {
            Console.WriteLine("File saved as XML");
          }
        }
        else if (userChoice == _saveAsBinaryChoice)
        {
          saveResult = editor.SaveAsBinary();

          if (saveResult)
          {
            Console.WriteLine("File saved as binary");
          }
        }
        else if (userChoice == _loadFromXmlChoice)
        {
          Console.Write("Enter XML file path to load: ");
          filePath = Console.ReadLine();

          editor.LoadFromXml(filePath);
        }
        else if (userChoice == _loadFromBinaryChoice)
        {
          Console.Write("Enter binary file path to load: ");
          filePath = Console.ReadLine();

          editor.LoadFromBinary(filePath);
        }
        else if (userChoice == _searchFilesChoice)
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

          searchType = int.Parse(Console.ReadLine());

          if (searchType == _searchByNameChoice)
          {
            searcher.SearchByName(searchKeyword);
          }
          else
          {
            searcher.SearchInContent(searchKeyword);
          }

          searcher.PrintFoundFiles();
        }
        else if (userChoice == _indexFilesChoice)
        {
          Console.Write("Enter directory to index: ");
          filePath = Console.ReadLine();
          indexer.SetIndexDirectory(filePath);

          keywordCount = _initialKeywordCount;

          Console.WriteLine("Enter keywords for indexing (enter empty line to finish):");

          while (keywordCount < _maxKeywords)
          {
            Console.Write("    Keyword " + (keywordCount + _keywordNumberOffset) + ": ");
            searchKeyword = Console.ReadLine();

            if (searchKeyword == string.Empty)
            {
              break;
            }

            keywordsForIndex[keywordCount] = searchKeyword;
            ++keywordCount;
          }

          if (keywordCount > _initialKeywordCount)
          {
            indexer.IndexFilesByKeywords(keywordsForIndex, keywordCount);

            Console.Write("Save results to file? (1 - yes, 0 - no): ");
            saveChoice = int.Parse(Console.ReadLine());

            if (saveChoice == _yesChoice)
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
        else if (userChoice == _exitChoice)
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