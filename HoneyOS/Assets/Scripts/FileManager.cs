using System.Collections.Generic;
using UnityEngine;
using TMPro;


// Define the FileManager class that extends the App class
public class FileManager : App
{   
    private Node home;
    private Node currentDirectory;

    [SerializeField] private TMP_Text _currentDirectoryAddress;


    public void Start(){
        home = new Node (new FileDescriptor("Home", "home"));
        currentDirectory = home;

        UpdateCurrentDirectoryText();
    }

    public void UpdateCurrentDirectoryText(){
        _currentDirectoryAddress.text = currentDirectory.FileDescriptor.Path;
    }


}

public class FileDescriptor {
    public string Path { get; private set; }
    public string Contents { get; set; }
    public string Name { get; private set; }

    public FileDescriptor(string path, string name)
    {
        Path = path;
        Contents = "";
        Name = name;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetPath(string path)
    {
        Path = path;
    }

    public string GetContent()
    {
        return Contents;
    }

    public void SetContents(string contents)
    {
        Contents = contents;
    }
}

public class Node {
    public FileDescriptor FileDescriptor { get; private set; }
    public List<Node> Children { get; private set; }
    public Node Parent { get; private set; }

    public Node(FileDescriptor fileDescriptor)
    {
        FileDescriptor = fileDescriptor;
        Children = new List<Node>();
    }

    public void SetParent(Node parent)
    {
        Parent = parent;
    }

    public void AddChild(Node child)
    {
        child.SetParent(this);
        Children.Add(child);
    }

    public void RemoveChild(Node child)
    {
        Children.Remove(child);
    }

    
}