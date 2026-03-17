using System;
using System.Collections.Generic;

namespace TextEditorApp {
  public class TextEditor {
    private TextFile currentFile;
    private List<TextMemento> historyList;
    private int currentStateIndex;
    private int maxHistorySize;
    private int shiftIndex;
    private int zeroValue;
    private int emptyStateIndex;

    public TextEditor()
    {
      currentFile = new TextFile();
      historyList = new List<TextMemento>();

      maxHistorySize = 10;
      shiftIndex = 1;
      zeroValue = 0;
      emptyStateIndex = -1;

      currentStateIndex = emptyStateIndex;
    }

    public void CreateNewFile(string path)
    {
      currentFile.SetFilePath(path);           
      currentFile.SetFileContent("");        
      SaveToHistory();
      Console.WriteLine("New file created: " + path);
    }

    public void SaveToHistory()
    {
      TextMemento newMemento = new TextMemento(
        currentFile.GetFileContent(),             
        currentFile.GetFilePath()               
      );
      historyList.Add(newMemento);

      if (historyList.Count > maxHistorySize)
      {
        historyList.RemoveAt(0);
      }

      currentStateIndex = historyList.Count - shiftIndex;
      Console.WriteLine("State saved. History size: " + historyList.Count);
    }

    public bool Undo()
    {
      if (currentStateIndex <= zeroValue)
      {
        Console.WriteLine("No states available for undo");
        return false;
      }

      TextMemento previousState;

      currentStateIndex = currentStateIndex - 1;
      previousState = historyList[currentStateIndex];

      if (previousState != null)
      {
        currentFile.SetFileContent(previousState.GetSavedContent());  
        currentFile.SetFilePath(previousState.GetSavedFilePath());    
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

      currentFile.SetFileContent(newContent);  
      SaveToHistory();
      Console.WriteLine("Content updated");
    }

    public bool SaveAsXml()
    {
      string xmlPath;
      xmlPath = currentFile.GetFilePath() + ".xml";   
      return currentFile.SaveToXml(xmlPath);
    }

    public bool SaveAsBinary()
    {
      string binaryPath;
      binaryPath = currentFile.GetFilePath() + ".bin";  
      return currentFile.SaveToBinary(binaryPath);
    }

    public bool LoadFromXml(string xmlPath)
    {
      TextFile loadedFile;
      bool loadResult;

      loadedFile = new TextFile();
      loadResult = loadedFile.LoadFromXml(xmlPath);

      if (loadResult)
      {
        currentFile = loadedFile;
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
        currentFile = loadedFile;
        SaveToHistory();
        Console.WriteLine("File loaded from binary file");
      }

      return loadResult;
    }
  }
}