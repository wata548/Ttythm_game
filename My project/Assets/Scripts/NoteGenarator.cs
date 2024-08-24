using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NoteGenarator : MonoBehaviour
{
    public GameObject notePrefab;
    [SerializeField] List<(List<string>,float)> chart;
    [SerializeField] float noteGenDelay;
    [SerializeField] short noteBarlength;

    List<Note> notes = new List<Note>();

    Note CreateNote(int line, int beat = 1) {
        Note newNote = new Note(Instantiate(notePrefab), line, beat);
        return newNote;
    }

    IEnumerator GenarateRowChart(List<(List<string>, float)> chart, int columnIndex = 0, int rowIndex = 0)
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(chart[columnIndex].Item1[rowIndex][i]);
            if ((Note.NoteType)chart[columnIndex].Item1[rowIndex][i] == Note.NoteType.SHORT_NOTE)
            {
                CreateNote(i);
            }
        }

        yield return new WaitForSeconds(chart[columnIndex].Item2);


        if (rowIndex + 1 != chart[columnIndex].Item1.Count)
        {
            StartCoroutine(GenarateRowChart(chart, columnIndex, rowIndex + 1));
        }
        else
        {
            if(columnIndex + 1 != chart.Count)
            {
                StartCoroutine(GenarateRowChart(chart, columnIndex + 1));
            }

            else 
                StartCoroutine(GenarateRowChart(chart));
        }
    }
    void Start()
    {
        chart = ChartReader.Instance.chartss;
        StartCoroutine(GenarateRowChart(chart));
    }
}
