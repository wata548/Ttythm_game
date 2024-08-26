using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

class Note
{
    public static List<Queue<Note>> presentChart = new List<Queue<Note>>();

    public static float TOP_NOTE_COOR = 3f;
    public static float NOTE_INTERVAL = 1.01f;
    public static float NOTE_START_COOR = -2.02f;

    //when note type equal long note, save long note's length. else if note type equal single note, this value is 1
    private ChartReader.NoteType noteType;
    private int noteLength;

    private GameObject note;

    public static void SetNoteStartCoor(short sumLines = 4)
    {
        NOTE_START_COOR = -(sumLines / 2) * NOTE_INTERVAL;
        
        if(sumLines % 2 == 1)
        {
            NOTE_START_COOR -= NOTE_INTERVAL / 2;
        }


        if(presentChart != null)
        {
            presentChart.Clear();
        }

        for(int i = 0; i < sumLines; i++)
        {
            presentChart.Add(new Queue<Note>());
        }
    }

    public Vector2 GetCoor()
    {
        return note.transform.position;
    }

    public Note(GameObject prefab, int line, int length = 1, short sumLines = 4)
    {
        this.noteLength = length;
        this.note = prefab;
        presentChart[line].Enqueue(this);

        this.noteType = (length == 1 ? ChartReader.NoteType.SHORT_NOTE : ChartReader.NoteType.LONG_NOTE);

        //note place on line
        prefab.transform.position = new Vector2(NOTE_START_COOR + NOTE_INTERVAL * line, TOP_NOTE_COOR);

        //fill color 
        GameObject renderer = prefab.transform.Find("Renderer").gameObject;
        SpriteRenderer colorChanger = renderer.GetComponent<SpriteRenderer>();

        if (sumLines % 2 == 1 && line == sumLines / 2)
        {
            colorChanger.color = Color.yellow;
        }

        else
        {
            if (sumLines / 2 > line)
            {
                colorChanger.color = (line % 2 == 0 ? Color.white : Color.blue);
            }

            else
            {
                colorChanger.color = ((sumLines - (1 + line)) % 2 == 0 ? Color.white : Color.blue);
            }
        }

        //I will make long note
    }
}
