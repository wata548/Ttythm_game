using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class ChartReader : MonoBehaviour
{
    public enum NoteType
    {
        NOT_NOTE = '.',
        LONG_NOTE = 'l',
        SHORT_NOTE = 's'
    };

    public enum RhythmType
    {
        MISMATCHED = '*',
        REST = '#',
        ROOF = '@'
    }

    public float power = 10f;

    private static ChartReader instance = null;
    public static ChartReader Instance {
        get
        {
            return instance;
        }
    }

    private string defaultAddress = "Assets\\Charts\\";
    [SerializeField] string fileName;


    public List<(List<string>, float)> chartss = new List<(List<string>, float)>();
    StreamReader chartReader;

    (List<string>, float) ReadChart()
    {
        string temp = chartReader.ReadLine();

        if (temp == null) {
            return (null, -1);
        }

        string[] tempList = temp.Split(' ');

        short bpm = short.Parse(tempList[0]);
        short beat = short.Parse(tempList[1]);

        List<string> charts = new List<string>();
        for (int i = 0; i < beat; i++)
        {
            temp = chartReader.ReadLine();
            charts.Add(temp);
        }

        return (charts, 60 / (float)bpm);
    }

    void ReadCharts()
    {
        while(true)
        {
            (List<string>, float) temp = ReadChart();
            if (temp == (null, -1))
            {
                break;
            }

            else
            {
                chartss.Add(temp);
            }
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        chartReader = new StreamReader(defaultAddress + fileName);
        ReadCharts();
    }
}
