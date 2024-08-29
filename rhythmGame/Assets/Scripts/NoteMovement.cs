using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    public float deadLine = -6;
    public float scrollPower = 22;

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
            
            if(transform.position.y <= deadLine)
            {
                note.Active = false;

                //push deactiveObjects
                this.ChangeDeactive();
                
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
}
