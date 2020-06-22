using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public int height;
    public int width;
    public GameObject barrierBlock;

    public float timescale;
    public static int minutes;
    public Text text;
    public static Vector3 Current = new Vector3();
    private System.Random r=new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        MakeBarriers();
        MakeCenterBarriers();
        minutes = 0;
       // SerializationManager.Load();
        InvokeRepeating("CountTime", 60f, 60f);
        InvokeRepeating("SpawnFromFile", 60f, 60f+r.Next(3)-1f);
        ChangeCurrent();
        InvokeRepeating("ChangeCurrent", 60f, 4f);
    }

    private void ChangeCurrent()
    {
        Current = (Current*0.2f+new Vector3((float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f, 0)).normalized;
    }

    private void CountTime()
    {
        minutes++;
        text.text = "Execution time(min): "+minutes.ToString();
        if (minutes % 10 == 0)
        {
            SerializationManager.Save();
        }
    }
    private void SpawnFromFile()
    {
        if (r.Next() % 30 == 0)//average 1 time every 30 minutes
        {
            var sv = SerializationManager.Load();
            SerializationManager.InstantiateRandomOrganism(sv);
        }
    }

    private void MakeBarriers()
    {
        //horizontal bottom
        GameObject b = Instantiate(barrierBlock, new Vector3(width / 2, -2), new Quaternion());
        b.transform.localScale = new Vector3(width / 1.85f, 1, 1);
        //horizontal top
        b = Instantiate(barrierBlock, new Vector3(width / 2, height + 2), new Quaternion());
        b.transform.localScale = new Vector3(width / 1.85f, 1, 1);

        //vertical left
        b = Instantiate(barrierBlock, new Vector3(-2, height / 2), new Quaternion());
        b.transform.localScale = new Vector3(width / 1.85f, 1, 1);
        b.transform.Rotate(new Vector3(0, 0, 90));

        //vertical right
        b = Instantiate(barrierBlock, new Vector3(width + 2, height / 2), new Quaternion());
        b.transform.localScale = new Vector3(width / 1.85f, 1, 1);
        b.transform.Rotate(new Vector3(0, 0, 90));
    }
    private void MakeCenterBarriers()
    {

        //DIAGONAL
        GameObject b = Instantiate(barrierBlock, new Vector3(width / 2, height / 2), new Quaternion());
        b.transform.localScale = new Vector3(height / 40, height / 18, 1);
        b.transform.Rotate(new Vector3(0, 0, 45));

        if (Hyperparameters.BARRIERS_NUMBER == 0)
            return;

        //vertical 
        b = Instantiate(barrierBlock, new Vector3(width / 2, height * 14 / 29), new Quaternion());
        b.transform.localScale = new Vector3(width * 14 / 29, 1, 1);
        b.transform.Rotate(new Vector3(0, 0, 90));


        if (Hyperparameters.BARRIERS_NUMBER == 1)
            return;


        //horizontal 
        b = Instantiate(barrierBlock, new Vector3(width / 2, height / 2), new Quaternion());
        b.transform.localScale = new Vector3(width*7 / 15, 1, 1);

        
    }
    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timescale;
    }
}
