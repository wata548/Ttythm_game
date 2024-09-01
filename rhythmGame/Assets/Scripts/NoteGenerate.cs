using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NoteGenerate : MonoBehaviour
{
    public static NoteGenerate instance;
    public static NoteGenerate Instance { get { return instance; } }

    //object pooling
    public List<Queue<GameObject>> activeNotes = new();
    public Queue<GameObject> deactiveNotes = new();

    //object pooling
    public Queue<GameObject> activeVirtualNotes = new();
    public Queue<GameObject> deactiveVirtualNotes = new();

    public GameObject prefab;

    //It compose activeNotes
    public void SetKey(int sumKey = 4)
    {
        while(activeNotes.Count < sumKey) {
            activeNotes.Add(new Queue<GameObject>());
        }
    }

    public GameObject GenerateNote(int line, float secondPerBeat, Note.NoteType noteType = Note.NoteType.SINGLE_NOTE)
    {
        switch (noteType) 
        { 
            case Note.NoteType.PRESS_NOTE:
                return GenerateVirtualNote(line, secondPerBeat, noteType);

            default:
                return GenerateRealNote(line, secondPerBeat, noteType);

        }
        GameObject GenerateVirtualNote(int line, float secondPerBeat, Note.NoteType noteType = Note.NoteType.PRESS_NOTE)
        {
            if (deactiveVirtualNotes.Count == 0)
            {
                return CopyGenerateVirtualNote(line, secondPerBeat, noteType);
            }

            else
            {
                return RecycleGenerateVirtualNote(deactiveVirtualNotes.Dequeue(), line, secondPerBeat, noteType);
            }

            GameObject CopyGenerateVirtualNote(int line, float secondPerBeat, Note.NoteType noteType = Note.NoteType.PRESS_NOTE)
            {
                GameObject newNote = Instantiate(prefab);
                newNote.name = "LongNote";

                activeVirtualNotes.Enqueue(newNote);

                newNote.AddComponent<Note>()
                       .Set(newNote, line, secondPerBeat, noteType);

                return newNote;
            }

            GameObject RecycleGenerateVirtualNote(GameObject noteObject, int line, float secondPerBeat, Note.NoteType noteType = Note.NoteType.PRESS_NOTE)
            {
                activeVirtualNotes.Enqueue(noteObject);

                noteObject.GetComponent<Note>()
                          .Set(line, secondPerBeat, noteType);

                return noteObject;
            }

        }

        GameObject GenerateRealNote(int line, float secondPerBeat, Note.NoteType noteType = Note.NoteType.SINGLE_NOTE)
        {
            if (deactiveNotes.Count == 0)
            {
                return CopyGenerateNote(line, secondPerBeat, noteType);
            }

            else
            {
                return RecycleGenerateNote(deactiveNotes.Dequeue(), line, secondPerBeat, noteType);
            }

            GameObject CopyGenerateNote(int line, float secondPerBeat, Note.NoteType noteType = Note.NoteType.SINGLE_NOTE)
            {
                GameObject newNote = Instantiate(prefab);
                newNote.name = "Note";

                activeNotes[line].Enqueue(newNote);

                newNote.AddComponent<Note>()
                       .Set(newNote, line, secondPerBeat, noteType);

                return newNote;
            }

            GameObject RecycleGenerateNote(GameObject noteObject, int line, float secondPerBeat, Note.NoteType noteType = Note.NoteType.SINGLE_NOTE)
            {
                activeNotes[line].Enqueue(noteObject);

                noteObject.GetComponent<Note>()
                          .Set(line, secondPerBeat, noteType);

                return noteObject;
            }

        }
    }
    private void Awake()
    {
        if(instance == null) 
            instance = this;
    }
}
