using System;

namespace TextEditorApp {
  public class TextMemento {
    private string savedContent;
    private string savedFilePath;

    public TextMemento(string content, string path)
    {
      savedContent = content;
      savedFilePath = path;
    }

    public string GetSavedContent()
    {
      return savedContent;
    }

    public string GetSavedFilePath()
    {
      return savedFilePath;
    }
  }
}