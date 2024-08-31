using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.U2D;
using Unity.VisualScripting;

public class ChartReader : MonoBehaviour
{
    //file access
    private string defaultAddress = "Assets\\Charts\\";
    [SerializeField] string fileName;

    StreamReader chartReader = null;

    public int sumLine;

    enum Gimmick
    {
        MATCHED = ' ',
        MIS_MATCHED = '*',
        REST = '#',
        ROOF = '@',
        SET_KEY = '$',
        END_PART = '~'
    };
    
    private List<(List<string> partChart, float secondPerBeat, int key)> ReadChart()
    {
        List<(List<string>, float, int)> chart = new();

        while (true)
        {
            (List<string> partChart, float secondPerBeat, int repeatCount, int key) partChartInformation = ReadPartChart();
            
            if(partChartInformation.partChart == null)
            {
                break;
            }

            for (int i = 0, size = partChartInformation.repeatCount; i < size; i++)
            {
                chart.Add((partChartInformation.partChart, partChartInformation.secondPerBeat, partChartInformation.key));
            }
        }

        return chart;

        (List<string> partChart, float secondPerBeat, int repeatCount, int key) ReadPartChart()
        {
            string bpmString = chartReader.ReadLine();

            if (bpmString == null)
            {
                return (null, 0, 0, 0);
            }

            int repeatCount = 1;
            int sumKey = sumLine;

            //if Chart want changing key, change key and 'bpmString' to remember bpm(string type)
            bpmString = CheckAndSetSumLine(bpmString, ref sumKey);
            bpmString = CheckAndSetRepeatCount(bpmString, ref repeatCount);

            sumLine = sumKey;

            //Set bpm
            float bpm = float.Parse(bpmString);
            float secondPerBeat = 60 / bpm;

            //repositories
            string line;
            List<string> lines = new List<string>();

            while (true)
            {
                line = chartReader.ReadLine();

                //part end check
                if (line[0] == (char)Gimmick.END_PART)
                {
                    break;
                }


                for (int i = 0, size = CheckRoof(ref line); i < size; i++)
                {
                    lines.Add(line);
                }

            }
            return (lines, secondPerBeat, repeatCount, sumKey);

            string CheckAndSetSumLine(string key, ref int sumKey)
            {
                if (key[0] == (char)Gimmick.SET_KEY)
                {
                    sumKey = int.Parse(key.Split((char)Gimmick.SET_KEY)[1]);
                    return chartReader.ReadLine();
                }

                return key;
            }
            string CheckAndSetRepeatCount(string line, ref int repeat)
            {
                if (line[0] == (char)Gimmick.ROOF)
                {
                    if (line.Length == 1)
                        repeat = 2;

                    else
                        repeat = int.Parse(line.Split((char)Gimmick.ROOF)[1]);

                    return chartReader.ReadLine();
                }

                return line;
            }

            int CheckRoof(ref string line)
            {
                (Gimmick, float) checkRoof = SpeacialGimmick(line);

                if (checkRoof.Item1 == Gimmick.ROOF)
                {
                    int size = (int)checkRoof.Item2;
                    line = line.Substring(0, sumLine);

                    return size;
                }

                else
                {
                    return 1;
                }
            }
        }
    }

    private (Gimmick, float) SpeacialGimmick(string line, float secondPerBeat = 1)
    {
        (Gimmick rhythmType, int power) rhythm =  CheckSpeacialRhythm(line);

        switch (rhythm.rhythmType) 
        {
            case Gimmick.MATCHED:
                return (rhythm.rhythmType, secondPerBeat);

            case Gimmick.MIS_MATCHED:
                return (rhythm.rhythmType, MismatchedRhythm(secondPerBeat, rhythm.power));

            case Gimmick.REST:
                return (rhythm.rhythmType, RestRhythm(secondPerBeat, rhythm.power));

            case Gimmick.ROOF:
                return (rhythm.rhythmType, RoofGimmick(rhythm.power));

            default:
                return (Gimmick.MATCHED, secondPerBeat);
        }
        float RoofGimmick(float repeat)
        {
            if (repeat == 1)
                return repeat + 1;
            
            else 
                return repeat;
        }
        float MismatchedRhythm(float secondPerBeat, int power = 1)
        {
            return secondPerBeat / (1 << power);
        }
        float RestRhythm(float secondPerBeat, int power = 1)
        {
            return secondPerBeat * (1 + power);
        }

        Gimmick CheckGimmick(string line)
        {
            int checkSpeacialRhythm = sumLine + 2;
            if (line.Length >= checkSpeacialRhythm)
            {
                return (Gimmick)line[checkSpeacialRhythm - 1];
            }

            else
            {
                return Gimmick.MATCHED;
            }
        }
        (Gimmick, int) CheckSpeacialRhythm(string line)
        {
            int checkSpeacialRhythm = sumLine + 2;
            Gimmick rhythmType = CheckGimmick(line);


            if (rhythmType != Gimmick.MATCHED)
            {
                //number skip
                if (line.Length == checkSpeacialRhythm)
                {
                    return (rhythmType, 1);
                }

                //not skip
                else
                {
                    return (rhythmType, int.Parse(line.Split((char)rhythmType)[1]));
                }
            }

            else
            {
                return (Gimmick.MATCHED, 0);
            }
        }
    }
   
    private void TestCheck(List<(List<string>, float, int)> chart)
    {
        for (int i = 0; i < chart.Count; i++)
        {
            Debug.Log(chart[i].Item2);
            Debug.Log(chart[i].Item3);
            for (int j = 0; j < chart[i].Item1.Count; j++)
            {
                Debug.Log(chart[i].Item1[j]);
            }
        }
    } 


    void GenerateNote(List<(List<string> partChart, float secondPerBeat, int key)> chart)
    {
        Note.MakeNoteFolder();

        StartCoroutine(CoroutineGenerateNotes(chart));
    }

    IEnumerator CoroutineGenerateNotes(List<(List<string> partChart, float secondPerBeat, int key)> chart, int partIndex= 0, int partLineIndex = 0)
    {
        string thisPartLine = chart[partIndex].partChart[partLineIndex];
        int thisKeys = chart[partIndex].key;
        float thisSecondPerBeat = chart[partIndex].secondPerBeat;

        NoteGenerate.Instance.SetKey();

        for (int i = 0; i < thisKeys; i++)
        {
            Note.NoteType noteType = (Note.NoteType)thisPartLine[i];

            if (noteType != Note.NoteType.VOID)
            {
                NoteGenerate.Instance.GenerateNote(i, noteType);

            }
        }

        (Gimmick gimmick, float second) checkGimick = SpeacialGimmick(thisPartLine, thisSecondPerBeat);

        if(checkGimick.gimmick != Gimmick.ROOF)
        {
            yield return new WaitForSeconds(checkGimick.second);
        }

        else {
            yield return new WaitForSeconds(thisSecondPerBeat);

        }

        partLineIndex++;
        if (partLineIndex >= chart[partIndex].partChart.Count)
        {
            partLineIndex = 0; 
            partIndex++;

            if(partIndex != chart.Count) 
            {
                StartCoroutine(CoroutineGenerateNotes(chart, partIndex, partLineIndex));
                 
            }
        }
        else StartCoroutine(CoroutineGenerateNotes(chart, partIndex, partLineIndex));



    }

    private List<(List<string>, float, int)> Chart;    
    private void Start()
    {
        chartReader = new StreamReader(defaultAddress + fileName + ".txt");
        Chart = ReadChart();

        TestCheck(Chart);

        GenerateNote(Chart);
    }
}
