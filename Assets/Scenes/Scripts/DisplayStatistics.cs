using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStatistics : MonoBehaviour
{
    public Text displayBestOrganism;

    System.Random r = new System.Random();

    void Start()
    {
        InvokeRepeating("KeepTrackOfOrganisms", 0f, 60f);
    }

    public Dictionary<string, int> speciesAge = new Dictionary<string, int>();
    public Dictionary<string, int[]> speciesLifespan = new Dictionary<string, int[]>();//the vector means [0] = sum of all ages [1] numer of organisms so average lifespan = [0]/[1]
    public Dictionary<string, float[]> speciesSize = new Dictionary<string, float[]>();//the vector means [0] = sum of all ages [1] numer of organisms so average lifespan = [0]/[1]

    public void reset()
    {
        speciesAge = new Dictionary<string, int>();
        speciesLifespan = new Dictionary<string, int[]>();//the vector means [0] = sum of all ages [1] numer of organisms so average lifespan = [0]/[1]
        speciesSize = new Dictionary<string, float[]>();//the vector means [0] = sum of all ages [1] numer of organisms so average lifespan = [0]/[1]

    }

    void KeepTrackOfOrganisms()
    {

        GameObject[] organisms = GameObject.FindGameObjectsWithTag("organism");
        HashSet<string> counted = new HashSet<string>();

        speciesLifespan.Clear();
        speciesSize.Clear();
        if (speciesAge.Count > 100000) speciesAge.Clear();
        foreach (GameObject organism in organisms)
        {
            if (speciesLifespan.ContainsKey(organism.name))
            {
                speciesLifespan[organism.name] = new int[] { speciesLifespan[organism.name][0] + organism.GetComponent<Organism>().Age, speciesLifespan[organism.name][1] + 1 };
            }
            else
                speciesLifespan[organism.name] = new int[] { organism.GetComponent<Organism>().Age, 1 };

            if (speciesSize.ContainsKey(organism.name))
            {
                speciesSize[organism.name] = new float[] { speciesSize[organism.name][0] + organism.GetComponent<Rigidbody2D>().mass, speciesSize[organism.name][1] + 1 };
            }
            else
                speciesSize[organism.name] = new float[] { organism.GetComponent<Rigidbody2D>().mass, 1 };


            if (counted.Contains(organism.name)) continue;

            counted.Add(organism.name);

            if (speciesAge.ContainsKey(organism.name))
            {
                speciesAge[organism.name] += 1;

            }
            else speciesAge[organism.name] = 0;
        }


        Display(organisms);


    }

    string fileName = "";

    private void Display(GameObject[] organisms)
    {
        if (organisms.Length == 0) return;
        Dictionary<String, int> organismCount = new Dictionary<string, int>();


        foreach (GameObject organism in organisms)
        {
            string key = organism.name + "-" + organism.GetComponent<Organism>().chromosome.MutationTracker;
            if (!organismCount.ContainsKey(key))
            {
                organismCount[key] = 1;

            }
            else
            {
                organismCount[key] = organismCount[key] + 1;
            }
        }

        var myList = organismCount.ToList();

        myList.Sort((pair1, pair2) => -pair1.Value.CompareTo(pair2.Value));
        displayBestOrganism.text = "";
        
        int i;
        float totalLifespan=0, totalMass=0, totalAge=0;
        for (i = 0; i < 20 && i < myList.Count; i++)
        {
            int age = speciesAge.ContainsKey(myList[i].Key.Split('-')[0]) ? speciesAge[myList[i].Key.Split('-')[0]] : 0;

            string name = myList[i].Key.Split('-')[0];

            float lifespan = (float)speciesLifespan[name][0] / (float)speciesLifespan[name][1];
            float mass = speciesSize[name][0] / speciesSize[name][1];

            totalLifespan += lifespan;
            totalMass += mass;
            totalAge += age;

            displayBestOrganism.text += myList[i].Key +
                "   age:" + age +
                "  lifespan:" + Math.Round(lifespan, 1) +
                "  mass:" + Math.Round(mass, 1) +
                "  total: " + myList[i].Value + "\n";
        }

        if(fileName =="")
        {
            fileName = Application.persistentDataPath + "/Statistics" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '.') + ".txt"; 
            
        }
        string content =   organisms.Length +" "+ speciesLifespan.Count + " " + myList[0].Value + " "+ speciesAge[myList[0].Key.Split('-')[0]] + " " + (totalAge / (float)i) + " " + (totalLifespan / (float)i) + " " + (totalMass / (float)i) + " " + GameObject.FindGameObjectsWithTag("food").Length+"\r\n";
        File.AppendAllText(fileName, content);

    }


}
