using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGenerate : MonoBehaviour
{
    public static NoteGenerate instance;
    public static NoteGenerate Instance { get { return instance; } }

    //object pooling
    public List<Queue<GameObject>> activeNotes = new();
    public Queue<GameObject> deactiveNotes = new();

    public GameObject prefab;

    //It compose activeNotes
    public void SetKey(int sumKey = 4)
    {
        while(activeNotes.Count < sumKey) {
            activeNotes.Add(new Queue<GameObject>());
        }
    }

    public GameObject GenerateNote(int line, Note.NoteType notetype = Note.NoteType.SINGLE_NOTE)
    {
        if (deactiveNotes.Count == 0)
        {
            return CopyGenerateNote(line);
        }

        else
        {
            return RecycleGenerateNote(deactiveNotes.Dequeue(), line);
        }

    }

    private GameObject CopyGenerateNote(int line)
    {
        GameObject newNote = Instantiate(prefab);

        activeNotes[line].Enqueue(newNote);

        newNote.AddComponent<Note>()
               .Set(newNote, line);

        return newNote;
    }

    private GameObject RecycleGenerateNote(GameObject noteObject, int line)
    {
        activeNotes[line].Enqueue(noteObject);

        noteObject.GetComponent<Note>()
                  .Set(line);

        return noteObject;
    }

    private void Awake()
    {
        if(instance == null) 
            instance = this;
    }
}
