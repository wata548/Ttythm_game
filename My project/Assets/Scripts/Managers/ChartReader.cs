using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
        ROOF = '@',
        CHANGE_BPM = '~'
    }

    //I will move this other script
    public float power = 10f;

    public short keyLineCount = 10;

        
    //file access
    private string defaultAddress = "Assets\\Charts\\";
    [SerializeField] string fileName;

    //remember all charts
    public List<(List<string>, float)> charts = new List<(List<string>, float)>();
    StreamReader chartReader;

    //read single chart( = one bpm)
    /*each bpm chart, second per beat, repeat count*/
    (List<string>, float, short) ReadChart()
    {
        string bpm = chartReader.ReadLine();

        //end check 
        if (bpm == null || bpm == "\n") {
            return (null, -1 , -1);
        }


        short repeat = 1;
        //check repeat
        if ((ChartReader.RhythmType)bpm[0] == (ChartReader.RhythmType.ROOF))
        {
            //skip number of repetition
            if(bpm.Length == 1)
            {
                repeat++;
            }
            else
            {
                //extract repeat count
                string[] temp = bpm.Split((char)ChartReader.RhythmType.ROOF);
                repeat += short.Parse(temp[1]);
            }

            bpm = chartReader.ReadLine();
        }

        //split bpm and beat
        short bpmValue = short.Parse(bpm);

        //make temporary repository
        List<string> singleCharts = new List<string>();

        //read Chart
        string singleChart;
        while (true)
        {
            singleChart = chartReader.ReadLine();

            //change BPM
            if ((ChartReader.RhythmType)singleChart[0] == ChartReader.RhythmType.CHANGE_BPM)
            {
                break;
            }
            
            //add chart
            else
            {
                singleCharts.Add(singleChart);
            }
        }

        return (singleCharts, 60 / (float)bpmValue, repeat);
    }

    //read all chart ( = many bpms)
    void ReadCharts()
    {
        //check how many key
        keyLineCount = short.Parse(chartReader.ReadLine());

        //remember result of ReadChart funtion
        (List<string>, float, short) chart;

        while(true)
        {
            chart = ReadChart();

            //end check
            if (chart.Item1 == null)
            {
                break;
            }

            //add chart
            else
            {
                //reapeat is remembered by temp.Itemp3 
                for(short i = 0; i < chart.Item3; i++)
                {
                    charts.Add((chart.Item1, chart.Item2));
                }
            }
        }
    }

    //singletone
    private static ChartReader instance = null;
    public static ChartReader Instance 
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        //apply singletone pattern
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
