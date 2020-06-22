using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


static class SerializationManager
{
    public static string previous = null;
    private static System.Random r = new System.Random();

    public static void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        if (previous != null)
            File.Delete(previous);

        string path = Application.persistentDataPath + "/organisms_" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '.') + ".sav";
        FileStream stream = new FileStream(path, FileMode.Create);
        previous = path;

        List<SaveOrganism> organisms = new List<SaveOrganism>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("organism"))
        {
            organisms.Add(new SaveOrganism(obj.GetComponent<Organism>()));
        }

        SaveObject saveObject = new SaveObject();
        saveObject.organisms = organisms.ToArray();
        saveObject.foodSpawned = GameObject.FindGameObjectsWithTag("food").Length;

        formatter.Serialize(stream, saveObject);

        stream.Close();

    }

    public static void InstantiateRandomOrganism(SaveObject s)
    {
        SaveOrganism so = s.organisms[r.Next(s.organisms.Length)];

        Organism org = OrganismSpawn.SpawnOrganism(so.chromosome, new Vector3(so.x, so.y), so.initialEnergy);
        if (org != null)
        {
            org.fullyGrow = true;
        }
    }

    public static SaveObject Load()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath);

        if (files.Length == 0)
            return null;

        string mostRecent = "";
        DateTime mostRecentDate = new DateTime();
        foreach (string file in files)
        {
            try
            {
                string tmp = file.Split('_')[1];
                tmp = tmp.Remove(tmp.Length - 4).Replace('-', '/').Replace('.', ':');

                DateTime d = Convert.ToDateTime(tmp);
                if (DateTime.Compare(d, mostRecentDate) > 0)
                {
                    mostRecent = file;
                    mostRecentDate = d;
                }
            }
            catch (Exception) { }
        }

        if (File.Exists(mostRecent))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(mostRecent, FileMode.Open);

            SaveObject deserialized = (SaveObject)formatter.Deserialize(stream);

            stream.Close();

            return deserialized;
        }
        return null;
    }

    public static void InstantiateSaveObject(SaveObject s)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("food"))
        {
            GameObject.Destroy(obj);
        }
        var fs = GameObject.FindGameObjectsWithTag("foodspawner");
        double totalfoodRate = 0;
        for (int i = 0; i < fs.Length; i++)
        {
            totalfoodRate += fs[i].GetComponent<FoodSpawn>().foodRate;
        }

        for (int i = 0; i < fs.Length; i++)
        {
            fs[i].GetComponent<FoodSpawn>().foodCount = 0;

            while (fs[i].GetComponent<FoodSpawn>().foodCount < s.foodSpawned * (fs[i].GetComponent<FoodSpawn>().foodRate / totalfoodRate) && fs[i].GetComponent<FoodSpawn>().foodCount < fs[i].GetComponent<FoodSpawn>().MAX_FOOD)
            {
                fs[i].GetComponent<FoodSpawn>().SpawnFood();
            }
        }
        foreach (SaveOrganism data in s.organisms)
        {
            Organism org = OrganismSpawn.SpawnOrganism(data.chromosome, new Vector3(data.x, data.y), data.initialEnergy);
            if (org != null)
            {
                //    org.fullyGrow = true;
            }
        }
    }

}
[Serializable]
public class SaveObject
{
    public SaveOrganism[] organisms;
    public int foodSpawned;

}