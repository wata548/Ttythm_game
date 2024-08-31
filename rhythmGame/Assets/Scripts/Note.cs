using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] static float NOTE_START_COOR_Y = 3f;
    [SerializeField] static float NOTE_INTERVAL = 1.01f;
    [SerializeField] static float NOTE_START_COOR_X = -2.02f;
    
    //It will place other place
    [SerializeField] static int sumLines = 4;

    static GameObject activeNote;
    static GameObject deactiveNote;
    static bool creatFolder = false;

    public enum NoteType {
        VOID = '.',
        
        START_PRESS_NOTE = 's',
        PRESS_NOTE = 'p', 
        END_PRESS_NOTE = 'e',

        SINGLE_NOTE = 'n', 

        ALL_DIRECTION_FLICK_NOTE = 'a',
        FRONT_DIRECTION_FLICK_NOTE = 'f',
        LEFT_DIRECTION_FLICK_NOTE = 'l',
        RIGHT_DIRECTION_FLICK_NOTE = 'r'
    };

    [SerializeField] bool active;
    public bool Active { get { return active; } set { active = value; } }

    [SerializeField] int line;
    public int Line { get { return line; } }

    [SerializeField] NoteType noteType;
    public NoteType thisNoteType
    {
        get { return noteType; }
    }

    [SerializeField] GameObject noteObject;

    static public void MakeNoteFolder()
    {
        if (!creatFolder)
        {
            creatFolder = true;

            activeNote = new GameObject();
            activeNote.name = "ActiveNote";

            deactiveNote = new GameObject();
            deactiveNote.name = "DeactiveNote";
        }
    }

    public static void SetNoteStartCoor(int sumLines = 4)
    {
        Note.sumLines = sumLines;
        NOTE_START_COOR_X = -(sumLines / 2) * NOTE_INTERVAL;

        if (sumLines % 2 == 1)
        {
            NOTE_START_COOR_X -= NOTE_INTERVAL / 2;
        }
    }

    public GameObject Set(GameObject note, int line)
    {
        active = true;

        this.noteObject = note;
        this.line = line;
        
        this.ChangeActive();

        SetNoteCoordinate(this.noteObject, this.line);

        ChangeColor(this.noteObject, this.line);

        SetParent(this.noteObject, true);

        return noteObject;
    }

    public GameObject Set(int line)
    {
        active = true;

        this.ChangeActive();

        this.line = line;

        SetNoteCoordinate(this.noteObject, this.line);

        ChangeColor(this.noteObject, this.line);

        SetParent(this.noteObject, true);

        return noteObject;
    }

    private Color DecisionColor(int line)
    {
        if (line % 2 == 1 && line == sumLines / 2)
        {
            return Color.yellow;
        }

        else
        {
            if (sumLines / 2 > line)
            {
                return (line % 2 == 0 ? Color.white : Color.blue);
            }

            else
            {
                return ((sumLines - (1 + line)) % 2 == 0 ? Color.white : Color.blue);
            }
        }
    }

    private void ChangeColor(GameObject noteObject, int line)
    {
        SpriteRenderer colorChanger = noteObject.GetComponent<SpriteRenderer>();
        colorChanger.color = DecisionColor(line);
    }

    private void SetNoteCoordinate(GameObject noteObject, int line) {
        noteObject.transform.position = new Vector2(NOTE_START_COOR_X + NOTE_INTERVAL * line, NOTE_START_COOR_Y);
    }

    private void SetParent(GameObject noteObject, bool active)
    {
        noteObject.transform.parent = active ? activeNote.transform : deactiveNote.transform;
    }

    public void ChangeDeactive()
    {
        this.noteObject.GetComponent<SpriteRenderer>().enabled = false;
        this.noteObject.transform.parent = deactiveNote.transform;
    }

    public void ChangeActive()
    {
        this.noteObject.GetComponent<SpriteRenderer>().enabled = true;
        this.noteObject.transform.parent = activeNote.transform;
    }
}
