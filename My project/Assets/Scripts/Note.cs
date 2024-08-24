using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Note
{
    public enum NoteType
    {
        NOT_NOTE = '.', LONG_NOTE = 'L', SHORT_NOTE = 'N'
    };

    public static float TOP_NOTE_COOR = 3f;
    public static float NOTE_SIZE = 1.01f;
    public static float FOUR_KEY_NOTE_START_COOR = -2.2f;

    private int noteLength;
    private GameObject note;
    private int noteLine;
    private NoteType noteType;

    public Note(GameObject prefab, int line, int length = 1)
    {
        this.noteLength = length;
        this.note = prefab;
        this.noteLine = line;

        this.noteType = (length == 1 ? NoteType.SHORT_NOTE : NoteType.LONG_NOTE);

        prefab.transform.position = new Vector2(FOUR_KEY_NOTE_START_COOR + NOTE_SIZE * noteLine, TOP_NOTE_COOR);

        GameObject renderer = prefab.transform.Find("Renderer").gameObject;
        SpriteRenderer colorChanger = renderer.GetComponent<SpriteRenderer>();

        colorChanger.color = (noteLine == 1 || noteLine == 2 ? Color.blue : Color.white);

        /*if(this.noteType == NoteType.LONG_NOTE)
        {

        }*/
    }
}
