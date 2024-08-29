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

    public float interval = 1f;
    public GameObject prefab;

    //It compose activeNotes
    public void SetKey(int sumKey = 4)
    {
        while(activeNotes.Count < sumKey) {
            activeNotes.Add(new Queue<GameObject>());
        }
    }

    public GameObject GenerateNote(int line, int length = 1)
    {
        if (deactiveNotes.Count == 0)
        {
            return CopyGenerateNote(line, length);
        }

        else
        {
            return RecycleGenerateNote(deactiveNotes.Dequeue(), line, length);
        }

    }

    private GameObject CopyGenerateNote(int line, int length = 1)
    {
        GameObject newNote = Instantiate(prefab);

        activeNotes[line].Enqueue(newNote);

        newNote.AddComponent<Note>()
               .Set(newNote, line, length);

        return newNote;
    }

    private GameObject RecycleGenerateNote(GameObject noteObject, int line, int length = 1)
    {
        activeNotes[line].Enqueue(noteObject);

        noteObject.GetComponent<Note>()
                  .Set(line, length);

        return noteObject;
    }

    //It will delete. It just test code.
    IEnumerator wait()
    {
        yield return new WaitForSeconds(interval);

        GenerateNote(Random.Range(0,22));

        StartCoroutine(wait());
    }


    private void Awake()
    {
        if(instance == null) 
            instance = this;
    }

    void Start()
    {
        Note.MakeNoteFolder();
        SetKey(22);
        StartCoroutine(wait());
    }
}
