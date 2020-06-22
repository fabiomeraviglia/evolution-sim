using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FatCell : Cell
{
    
    void Start()
    {
        InvokeCellStuff();
    }


    public override void SetInputNeurons(List<Neuron> inputNeurons)
    {
  
    }

    public override void SetOutputNeurons(List<Neuron> outputNeurons)
    {
    }

    public override List<int> GetInputNeuronsNumbers(int startingNumber)
    {
        return new List<int>();
    }

    public override List<int> GetOutputNeuronsNumbers(int startingNumber)
    {
        return new List<int>();
    }

    public override bool CanLiveAlone()
    {
        return false;
    }
}
