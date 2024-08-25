using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NoteGenarator : MonoBehaviour
{
    //set up
    public GameObject notePrefab;
    public Transform parent;
    [SerializeField] List<(List<string>,float)> chart;

    //notes repository
    List<Note> notes = new List<Note>();

    //select line ex) 4k, 5k, 6k, 8k....
    short keyLine;

    //check special rhythm index
    const short checkSpecialRhythmRange = 2;

    /*sum lines and beat(it remember note is how long, if note is single note beatequal 1)*/
    Note CreateNote(int line, int beat = 1) {
        Note newNote = new Note(Instantiate(notePrefab, parent), line, beat, keyLine);
        return newNote;
    }

    /*Tuple(charts(string type), seconnd per beat)*/
    IEnumerator GenarateRowChart(List<(List<string>, float)> chart, int columnIndex = 0, int rowIndex = 0)
    {
        string thisChart = chart[columnIndex].Item1[rowIndex];

        //read present line
        for (int i = 0; i < keyLine; i++)
        {
            if ((ChartReader.NoteType)thisChart[i] == ChartReader.NoteType.SHORT_NOTE)
            {
                CreateNote(i);
            }
        }

        //check using special rhythm
        if (thisChart.Length >= checkSpecialRhythmRange + keyLine)
        {
            char check = thisChart[keyLine - 1 + checkSpecialRhythmRange];

            //process mismatch
            if ((ChartReader.RhythmType)check == ChartReader.RhythmType.MISMATCHED)
            {
                //skip number
                if (thisChart.Length == checkSpecialRhythmRange + keyLine)
                {
                    yield return new WaitForSeconds(chart[columnIndex].Item2 / 2);
                }

                //use number
                else
                {
                    string[] temp = thisChart.Split((char)ChartReader.RhythmType.MISMATCHED);
                    yield return new WaitForSeconds(chart[columnIndex].Item2 / (1 << (int.Parse(temp[1]))));
                }
            }

            //process rest
            if ((ChartReader.RhythmType)check == ChartReader.RhythmType.REST)
            {
                //skip number
                if(thisChart.Length == checkSpecialRhythmRange + keyLine)
                {
                    yield return new WaitForSeconds(chart[columnIndex].Item2 * 2);
                }

                //use number
                else
                {
                    string[] temp = chart[columnIndex].Item1[rowIndex].Split((char)ChartReader.RhythmType.REST);
                    yield return new WaitForSeconds(chart[columnIndex].Item2 * (1 + int.Parse(temp[1])));
                }
            }
        }

        //not use special rhythm
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

            //I will delete this code. It cause infinty roof
            else 
                StartCoroutine(GenarateRowChart(chart));
        }
    }
    void Start()
    {
        chart = ChartReader.Instance.charts;
        keyLine = ChartReader.Instance.keyLineCount;
        Note.SetNoteStartCoor(keyLine);
        StartCoroutine(GenarateRowChart(chart));
    }
}
