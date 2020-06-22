using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCell : Cell
{
    Neuron[] neurons = new Neuron[3];
    void Start()
    {
        InvokeCellStuff();
        InvokeRepeating("Routine", 0.5f, SimulationParameters.brainTick);
    }

    private void Routine()
    {
        int numberOfRays = 3;
        float angle = -5f;
        float totalR=0, totalG=0, totalB=0;

        for (int i = 0; i < numberOfRays; i++)
        {
            RaycastHit2D result = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, angle) * -transform.up);

            if (result.collider != null)
            {
                Sprite mySprite;
                if (result.transform.tag == "organism")
                {
                    mySprite = result.collider.GetComponentInParent<SpriteRenderer>().sprite;
                }
                else
                {
                    mySprite = result.transform.GetComponent<SpriteRenderer>().sprite;
                }
                Texture2D myTexture = mySprite.texture;

                Color MyPixel = myTexture.GetPixel((int)mySprite.pivot.x, (int)mySprite.pivot.y);
                totalR += MyPixel.r;
                totalG += MyPixel.g;
                totalB += MyPixel.b;

                Debug.DrawLine(transform.position, result.point, MyPixel, 0.5f);

            }

            angle = angle + 5f;
        }
        
            neurons[0].Value = totalR/ numberOfRays;
            neurons[1].Value = totalG/ numberOfRays;
            neurons[2].Value = totalB / numberOfRays;

    }
    


   
    public override void SetInputNeurons(List<Neuron> inputNeurons)
    {
        for (int i = 0; i < inputNeurons.Count; i++)
        {
            neurons[i] = inputNeurons[i];
        }
    }

    public override void SetOutputNeurons(List<Neuron> outputNeurons)
    {
    }

    public override List<int> GetInputNeuronsNumbers(int startingNumber)
    {
        List<int> n = new List<int>();
        n.Add(startingNumber * 1000 + 20);
        n.Add(startingNumber * 1000 + 21);
        n.Add(startingNumber * 1000 + 22);
        return n;
    }

    public override List<int> GetOutputNeuronsNumbers(int startingNumber)
    {
        return new List<int>();
    }

    public override bool CanLiveAlone()
    {
        return true;
    }
}
