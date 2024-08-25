using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NoteGenarator : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform parent;
    [SerializeField] List<(List<string>,float)> chart;

    List<Note> notes = new List<Note>();
    const int keyLine = 4;
    const int checkSpecialRhythm = keyLine + 2;

    Note CreateNote(int line, int beat = 1) {
        Note newNote = new Note(Instantiate(notePrefab, parent), line, beat);
        return newNote;
    }

    IEnumerator GenarateRowChart(List<(List<string>, float)> chart, int columnIndex = 0, int rowIndex = 0)
    {
        for (int i = 0; i < keyLine; i++)
        {
            if ((ChartReader.NoteType)chart[columnIndex].Item1[rowIndex][i] == ChartReader.NoteType.SHORT_NOTE)
            {
                CreateNote(i);
            }
        }

        if (chart[columnIndex].Item1[rowIndex].Length >= checkSpecialRhythm)
        {
            if ((ChartReader.RhythmType)chart[columnIndex].Item1[rowIndex][keyLine + 1] == ChartReader.RhythmType.MISMATCHED)
            {
                yield return new WaitForSeconds(chart[columnIndex].Item2 / 2);
            }

            if ((ChartReader.RhythmType)(ChartReader.RhythmType)chart[columnIndex].Item1[rowIndex][keyLine + 1] == ChartReader.RhythmType.REST)
            {
                if(chart[columnIndex].Item1[rowIndex].Length == checkSpecialRhythm)
                {
                    yield return new WaitForSeconds(chart[columnIndex].Item2 * 2);
                }

                else
                {
                    string[] temp = chart[columnIndex].Item1[rowIndex].Split((char)ChartReader.RhythmType.REST);
                    yield return new WaitForSeconds(chart[columnIndex].Item2 * (1 + int.Parse(temp[1])));
                }

            }
        }

        else
        {
            yield return new WaitForSeconds(chart[columnIndex].Item2);
        }


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
        chart = ChartReader.Instance.charts;
        StartCoroutine(GenarateRowChart(chart));
    }
}
