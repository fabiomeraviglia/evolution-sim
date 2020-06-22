using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Chromosome chromosome = new Chromosome(MakeGenes(), MakeNeuralChromosome(), new ChromosomeParameters.ChromosomeParametersBuilder().SetAltruismEnergy(0.5).Build());
            OrganismSpawn.SpawnOrganism(chromosome, Camera.main.ScreenToWorldPoint(Input.mousePosition), OrganismSetter.BodyEnergy(chromosome) * 3 / 2);
        }
    }

    private NeuralChromosome MakeNeuralChromosome()
    {
        DendriteGene[] dg = {
       /*     new DendriteGene(-500,10,-3.5f),

            new DendriteGene(12022,10,4f),

            new DendriteGene(-500,11,-3f),
            new DendriteGene(4022,11,9f),
            new DendriteGene(3022,11,-9f),

            new DendriteGene(-500,12,-3f),
            new DendriteGene(4022,12,-9f),
            new DendriteGene(3022,12,9f),

            new DendriteGene(-500,13,-9f),
            new DendriteGene(10,13,6f),
            new DendriteGene(11,13,6f),

            new DendriteGene(-500,14,-9f),
            new DendriteGene(10,14,6f),
            new DendriteGene(12,14,6f),

            new DendriteGene(-500,6000,3f),
            new DendriteGene(10,6000,-3.5f),


            new DendriteGene(13,6001,3f),
            new DendriteGene(14,6001,-3f)*/
        };


        return new NeuralChromosome(dg);

    }

    Gene[] MakeGenes()
    {

        GameObject[] cellTypes = GameObject.FindGameObjectWithTag("organismspawner").GetComponent<OrganismSpawn>().cellTypes;
        Gene[] customOrganism = {new Gene(1,new Vector3(-1,0),cellTypes[5],1),
new Gene(1,new Vector3(0,1),cellTypes[2],2),
new Gene(1,new Vector3(-1,0),cellTypes[5],3),
new Gene(3,new Vector3(-1,0),cellTypes[5],1),
new Gene(3,new Vector3(-1,0),cellTypes[4],4),
new Gene(4,new Vector3(-1,0),cellTypes[5],3),
new Gene(1,new Vector3(0,1),cellTypes[5],1),
new Gene(1,new Vector3(0,1),cellTypes[5],1),
new Gene(2,new Vector3(0,1),cellTypes[5],5),
new Gene(4,new Vector3(0,1),cellTypes[5],8),
new Gene(8,new Vector3(-1,0),cellTypes[5],9),
new Gene(8,new Vector3(1,0),cellTypes[5],8),
new Gene(8,new Vector3(0,1),cellTypes[0],11),
new Gene(3,new Vector3(0,1),cellTypes[4],12),
new Gene(9,new Vector3(0,1),cellTypes[2],13),
new Gene(3,new Vector3(0,-1),cellTypes[1],14),
new Gene(3,new Vector3(0,-1),cellTypes[1],14),
new Gene(13,new Vector3(0,1),cellTypes[6],15),
new Gene(13,new Vector3(0,1),cellTypes[5],16),
new Gene(13,new Vector3(0,1),cellTypes[5],17),

                                 };
        /*   Gene[] customOrganism = {new Gene(0,new Vector3(0,0), cellTypes[0],1),
                                  new Gene(1,new Vector3(-1,0), cellTypes[4],2),
                                  new Gene(1,new Vector3(1,0), cellTypes[7],3),
                                  new Gene(2,new Vector3(-1,0), cellTypes[7],4),
                                  new Gene(1,new Vector3(0,-1), cellTypes[4],5),
                                  new Gene(2,new Vector3(0,-1), cellTypes[0],5),
                                  new Gene(5,new Vector3(0,-1), cellTypes[1],6),
                                  new Gene(6,new Vector3(0,-1), cellTypes[1],6),
                                  new Gene(4,new Vector3(0,-1), cellTypes[3],7),
                                  new Gene(4,new Vector3(0,1), cellTypes[3],9),
                                  new Gene(3,new Vector3(0,1), cellTypes[3],10),
                                  new Gene(3,new Vector3(0,-1), cellTypes[3],8),
                                  new Gene(1,new Vector3(0,1), cellTypes[4],11),

                                  new Gene(2,new Vector3(0,1), cellTypes[4],11),

                                  new Gene(11,new Vector3(0,1), cellTypes[7],12),

                                  new Gene(6,new Vector3(0,-1), cellTypes[1],6)
                                    };*/ //intelligen one

        /*    first version: slow and armored
         *    Gene[] customOrganism = {new Gene(1,new Vector3(0,1), cellTypes[0],1),
                                      new Gene(1,new Vector3(0,1), cellTypes[2],2),
                                      new Gene(1,new Vector3(1,0), cellTypes[2],3),
                                      new Gene(1,new Vector3(-1,0), cellTypes[2],3),
                                      new Gene(3,new Vector3(0,-1), cellTypes[3],4),
                                      new Gene(4,new Vector3(0,-1), cellTypes[3],5),
                                      new Gene(5,new Vector3(0,-1), cellTypes[3],6),
                                      new Gene(6,new Vector3(0,-1), cellTypes[3],7),
                                      new Gene(1,new Vector3(0,-1), cellTypes[1],10),
                                      new Gene(10,new Vector3(0,-1), cellTypes[0],11),
                                      new Gene(10,new Vector3(0,-1), cellTypes[0],11),
                                      new Gene(11,new Vector3(0,-1), cellTypes[1],12),
                                        };*/

        return customOrganism;
    }
}
