using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DisplayNeuralNetwork : MonoBehaviour
{
    public static NeuralNetwork displayedNN;
    public GameObject neuronDisplay;
    public GameObject line;
    Vector2 drawPosition = new Vector2(0,-10);
    public float distanceBetweenNeurons;
    private HashSet<GameObject> instantiated = new HashSet<GameObject>();
    Dictionary<Neuron, Vector2> displayPositions = new Dictionary<Neuron, Vector2>();

    public void Start()
    {
        InvokeRepeating("Routine", 1f, SimulationParameters.brainTick);
    }

    private void Routine()
    {
        if (displayedNN == null)
            return;

        Clear();

        DrawLayer(displayedNN.GetInputLayer(), drawPosition);

        DrawLayer(displayedNN.GetHiddenLayer(), new Vector2(drawPosition.x + distanceBetweenNeurons * 2, drawPosition.y));

        DrawLayer(displayedNN.GetOutputLayer(), new Vector2(drawPosition.x + distanceBetweenNeurons * 4, drawPosition.y));

        DrawLines(displayedNN.GetHiddenLayer());

        DrawLines(displayedNN.GetOutputLayer());

        DrawOutput(displayedNN.GetOutputLayer());

    }
    private void DrawOutput(Neuron[] outputNeurons)
    {
        foreach (Neuron neuron in outputNeurons)
        {
    
                Vector3[] positions = new Vector3[2];
                positions[0] = displayPositions[neuron];
                positions[1] = positions[0]+new Vector3(distanceBetweenNeurons,0,0);

                var go = Instantiate(line, new Vector3(), new Quaternion());
                var lr = go.GetComponent<LineRenderer>();
                lr.SetPositions(positions);
                lr.startWidth = 0.5f;
                lr.endWidth = lr.startWidth;
                lr.startColor = new Color(neuron.Value, neuron.Value, neuron.Value);
                lr.endColor = lr.startColor;
                go.GetComponent<LineRenderer>().SetPositions(positions);
                instantiated.Add(go);
        }

    }

    private void DrawLines(Neuron[] layer)
    {
        foreach (Neuron neuron in layer)
        {
            int i = 0;
            foreach (Neuron input in neuron.inputNeurons)
            {
                Vector3[] positions = new Vector3[2];
                positions[0] = displayPositions[input];
                positions[1] = displayPositions[neuron];

                var go = Instantiate(line, new Vector3(), new Quaternion());
                var lr = go.GetComponent<LineRenderer>();
                lr.SetPositions(positions);
                lr.startWidth=1-1/(1+Math.Abs(neuron.weights[i]));
                lr.endWidth = lr.startWidth;
                lr.startColor = new Color(input.Value, input.Value, input.Value);
                lr.endColor = lr.startColor;
                go.GetComponent<LineRenderer>().SetPositions(positions);
                instantiated.Add(go);
                i++;
            }
        }

    }

    private void Clear()
    {
        foreach(GameObject gameObject in instantiated)
        {
            Destroy(gameObject);
        }
        instantiated.Clear();
        displayPositions.Clear();
    }

    private void DrawLayer(Neuron[] layer, Vector2 startingPos)
    {
        foreach (Neuron neuron in layer)
        {
            
            instantiated.Add(Instantiate(neuronDisplay, startingPos, new Quaternion()));
            displayPositions[neuron] = startingPos;
            startingPos =  new Vector2(startingPos.x, startingPos.y - distanceBetweenNeurons);

        }
        
    }
}