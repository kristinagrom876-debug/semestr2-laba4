using System;

namespace TextEditorApp {
  public class TextMemento {
    private string _savedContent;
    private string _savedFilePath;

    public TextMemento(string content, string path)
    {
      _savedContent = content;
      _savedFilePath = path;
    }

    public string GetSavedContent()
    {
      return _savedContent;
    }

    public string GetSavedFilePath()
    {
      return _savedFilePath;
    }
  }
}