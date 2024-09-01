using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    public float deadLine = -60;
    public static float scrollPower = 10;

    Note note = null;

    private void Awake()
    {
        note = GetComponent<Note>();
    }

    void Update()
    {
        if (note != null && note.Active)
        {
            NoteFalling(scrollPower);

            if(transform.position.y + this.transform.localScale.y / 2 <= deadLine)
            {
                note.Active = false;

                if (note.thisNoteType == Note.NoteType.PRESS_NOTE)
                {
                    this.ChangeDeactiveVirtual();
                }
                else
                {
                    this.ChangeDeactive();
                }

                //turn off renderer and change parent
                note.ChangeDeactive();
            }
        }

        else if(note == null)
        {
            note = GetComponent<Note>();
        }
    }

    private void NoteFalling(float scrollPower)
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - scrollPower * Time.deltaTime);

    }

    private void ChangeDeactive()
    {
        GameObject ActivedObject = NoteGenerate.Instance.activeNotes[note.Line].Dequeue();
        NoteGenerate.Instance.deactiveNotes.Enqueue(ActivedObject);
    }

    private void ChangeDeactiveVirtual()
    {
        GameObject ActivedObject = NoteGenerate.Instance.activeVirtualNotes.Dequeue();
        NoteGenerate.Instance.deactiveVirtualNotes.Enqueue(ActivedObject);
    }
}
