using System;
using System.Collections.Generic;

namespace TextEditorApp {
  public class TextEditor : IOriginator {
    private TextFile _currentFile;
    private List<TextMemento> _historyList;
    private int _currentStateIndex;
    private int _maxHistorySize;
    private int _shiftIndex;
    private int _zeroValue;
    private int _emptyStateIndex;
    private int _undoStep;

    public TextEditor()
    {
      _currentFile = new TextFile();
      _historyList = new List<TextMemento>();

      _maxHistorySize = 10;
      _shiftIndex = 1;
      _zeroValue = 0;
      _emptyStateIndex = -1;
      _undoStep = 1;

      _currentStateIndex = _emptyStateIndex;
    }

    public void CreateNewFile(string path)
    {
      _currentFile.SetFilePath(path);
      _currentFile.SetFileContent("");
      SaveToHistory();
      Console.WriteLine("New file created: " + path);
    }

    public void SaveToHistory()
    {
      TextMemento newMemento;
      newMemento = new TextMemento(
        _currentFile.GetFileContent(),
        _currentFile.GetFilePath()
      );
      _historyList.Add(newMemento);

      if (_historyList.Count > _maxHistorySize)
      {
        _historyList.RemoveAt(0);
      }

      _currentStateIndex = _historyList.Count - _shiftIndex;
      Console.WriteLine("State saved. History size: " + _historyList.Count);
    }

    public bool Undo()
    {
      TextMemento previousState;

      if (_currentStateIndex <= _zeroValue)
      {
        Console.WriteLine("No states available for undo");
        return false;
      }

      _currentStateIndex = _currentStateIndex - _undoStep;
      previousState = _historyList[_currentStateIndex];

      if (previousState != null)
      {
        _currentFile.SetFileContent(previousState.GetSavedContent());
        _currentFile.SetFilePath(previousState.GetSavedFilePath());
        Console.WriteLine("Undo performed");
        return true;
      }

      return false;
    }

    public void EditContent()
    {
      string newContent;
      string inputLine;

      newContent = "";

      Console.WriteLine("Enter new text (empty line to finish):");

      while (true)
      {
        inputLine = Console.ReadLine();

        if (inputLine == "")
        {
          break;
        }

        newContent = newContent + inputLine + Environment.NewLine;
      }

      _currentFile.SetFileContent(newContent);
      SaveToHistory();
      Console.WriteLine("Content updated");
    }

    public bool SaveAsXml()
    {
      string xmlPath;
      xmlPath = _currentFile.GetFilePath() + ".xml";
      return _currentFile.SaveToXml(xmlPath);
    }

    public bool SaveAsBinary()
    {
      string binaryPath;
      binaryPath = _currentFile.GetFilePath() + ".bin";
      return _currentFile.SaveToBinary(binaryPath);
    }

    public bool LoadFromXml(string xmlPath)
    {
      TextFile loadedFile;
      bool loadResult;

      loadedFile = new TextFile();
      loadResult = loadedFile.LoadFromXml(xmlPath);

      if (loadResult)
      {
        _currentFile = loadedFile;
        SaveToHistory();
        Console.WriteLine("File loaded from XML");
      }

      return loadResult;
    }

    public bool LoadFromBinary(string binaryPath)
    {
      TextFile loadedFile;
      bool loadResult;

      loadedFile = new TextFile();
      loadResult = loadedFile.LoadFromBinary(binaryPath);

      if (loadResult)
      {
        _currentFile = loadedFile;
        SaveToHistory();
        Console.WriteLine("File loaded from binary file");
      }

      return loadResult;
    }

    public object GetMemento()
    {
      return new TextMemento
      {
        Content = _currentFile.GetFileContent(),
        FilePath = _currentFile.GetFilePath()
      };
    }

    public void SetMemento(object memento)
    {
      if (memento is TextMemento)
      {
        var mem = memento as TextMemento;
        _currentFile.SetFileContent(mem.Content);
        _currentFile.SetFilePath(mem.FilePath);
      }
    }
  }
}