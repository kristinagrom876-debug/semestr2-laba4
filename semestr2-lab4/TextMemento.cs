using System;

namespace TextEditorApp {
  public class TextMemento {
    public string Content { get; set; }
    public string FilePath { get; set; }

    public TextMemento()
    {
    }

    public TextMemento(string content, string path)
    {
      Content = content;
      FilePath = path;
    }

    public string GetSavedContent()
    {
      return Content;
    }

    public string GetSavedFilePath()
    {
      return FilePath;
    }
  }

  public interface IOriginator {
    object GetMemento();
    void SetMemento(object memento);
  }

  public class Caretaker {
    private object memento;

    public void SaveState(IOriginator originator)
    {
      memento = originator.GetMemento();
      Console.WriteLine("State saved!");
    }

    public void RestoreState(IOriginator originator)
    {
      if (memento != null)
      {
        originator.SetMemento(memento);
        Console.WriteLine("State restored!");
      }
      else
      {
        Console.WriteLine("No saved state available!");
      }
    }
  }
}