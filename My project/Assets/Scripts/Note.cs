using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Note
{
    public static float TOP_NOTE_COOR = 3f;
    public static float NOTE_INTERVAL = 1.01f;
    public static float FOUR_KEY_NOTE_START_COOR = -2.02f;

    //when note type equal long note, save long note's length. else if note type equal single note, this value is 1
    private ChartReader.NoteType noteType;
    private int noteLength;

    private GameObject note;
    private int noteLine;

    public Note(GameObject prefab, int line, int length = 1)
    {
        this.noteLength = length;
        this.note = prefab;
        this.noteLine = line;

        this.noteType = (length == 1 ? ChartReader.NoteType.SHORT_NOTE : ChartReader.NoteType.LONG_NOTE);

        //note place on line
        prefab.transform.position = new Vector2(FOUR_KEY_NOTE_START_COOR + NOTE_INTERVAL * noteLine, TOP_NOTE_COOR);

        //fill color 
        GameObject renderer = prefab.transform.Find("Renderer").gameObject;
        SpriteRenderer colorChanger = renderer.GetComponent<SpriteRenderer>();

        colorChanger.color = (noteLine == 1 || noteLine == 2 ? Color.blue : Color.white);

        //I will add long note
    }
}
