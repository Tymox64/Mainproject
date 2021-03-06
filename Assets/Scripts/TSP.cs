﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
//using MathNet.Numerics;
namespace Assets.Scripts
{

    /// <summary>
    /// open traveling salesman problem
    /// </summary>
    class TSP
    {

        //TSP simulated annealing

        public GameObject worldObj;

        Block[] currentRoute;
        float recentCost;

        float sigma = 5000f;
        float sigmaReduction = 0.005f; //sigma = 5000 and reduction 0.005 results in 1700 while loop iterations

        public List<List<Block>> simulatedAnnealingIntermediateConfigs;

        public TSP(List<Block> blockList)
        {
            currentRoute = blockList.ToArray();
            simulatedAnnealingIntermediateConfigs = new List<List<Block>>();
        }

        int visualizingCounter = 0;
        int currentIteration = 0;

        public Block[] simulatedAnnealing()
        {
       
            while (sigma >= 1)
            {
                int nodeAIndex = (int)(UnityEngine.Random.RandomRange(1f, currentRoute.Length) - 0.000001f);
                int nodeBIndex = (int)(UnityEngine.Random.RandomRange(1f, currentRoute.Length) - 0.000001f);

                recentCost = CalcCurrentCost();

                SwapTwoNodes(nodeAIndex, nodeBIndex);                

                if (!AcceptSwap())
                {                    
                    SwapTwoNodes(nodeBIndex, nodeAIndex);
                }                
                sigma *= 1 - sigmaReduction;

                 if (currentIteration++ % 75 == 0)
                 {
                     SnapshotCurrentConfig();
                 }          
            }
          

            return currentRoute;
        }                




        public void SnapshotCurrentConfig()
        {
            List<Block> currentConfig = new List<Block>();
            foreach (Block block in currentRoute)
            {
                currentConfig.Add(block);
            }
            simulatedAnnealingIntermediateConfigs.Add(currentConfig);
        }

        public void VisualizeRoundtrip()
        {
            for(int i = 0; i < currentRoute.Length - 1; i++)
            {
                worldObj.GetComponent<World>().VisualizeBlockpath(GetBlockpathFromAToB(currentRoute[i], currentRoute[i + 1]));
            }
            worldObj.GetComponent<World>().VisualizeBlockpath(GetBlockpathFromAToB(currentRoute[currentRoute.Length-1], currentRoute[0]));
        }









        /// <summary>
        /// calculates the cost of the current path
        /// </summary>
        float CalcCurrentCost()
        {
            float cost = 0f;
            Blockpath bp = null;

            for (int i = 1; i < currentRoute.Length; i++)
            {
                bp = GetBlockpathFromAToB(currentRoute[i - 1], currentRoute[i]);
                if (bp == null)
                {
                    Debug.Log("ERROR: path between two blocks was not calculated before, or blockpath was not set accordingly");
                    return float.PositiveInfinity;
                }
                cost += bp.cost;
            }

            //back to start location
            bp = GetBlockpathFromAToB(currentRoute[currentRoute.Length - 1], currentRoute[0]);
            if (bp == null)
            {
                Debug.Log("ERROR: path between two blocks was not calculated before, or blockpath was not set accordingly");
                return float.PositiveInfinity;
            }
            cost += bp.cost;
            //Debug.Log("cost: " + cost);
            return cost;
        }

        /// <returns> The Blockpath between two given blocks, direction sensitive! </returns>
        public static Blockpath GetBlockpathFromAToB(Block blockA, Block blockB)
        {
            if (blockA.edges == null || blockB.edges == null)
                return null;
            foreach (Blockpath bp in blockA.edges)
            {
                //blockA and blockB to be found as start- and endBlock in desired blockpath bp
                if (bp.endBlock == blockB)
                {
                    return bp;
                }
            }
            return null;
        }






        /// <summary>
        /// Swap two nodes in the current path
        /// </summary>    
        void SwapTwoNodes(int i, int j)
        {
            Block firstBlock = currentRoute[i];
            currentRoute[i] = currentRoute[j];
            currentRoute[j] = firstBlock;
        }

        /// <summary>
        /// accepts a swap with a certain chance depending on sigma and the total amount our current path became worse
        /// </summary>
        bool AcceptSwap()
        {
            //if the path improved we accept right away
            float currentCost = CalcCurrentCost();
            if (recentCost >= currentCost)
                return true;

            float delta = recentCost - currentCost;
            float chance = (float)(Mathf.Exp((float)delta / (float)sigma));

            float randomVal = UnityEngine.Random.Range(0f, 1f);
            //Debug.Log("Chance: " + chance);
            return chance > randomVal;
        }

        public void SetWorldObj(GameObject _worldObj)
        {
            worldObj = _worldObj;
        }




























































        /* TSP brute force
        
        public List<Block> nodes;
        public float[,] distance;

        /// <summary>
        /// adds startblock and apples to nodes
        /// </summary>
        /// <param name="startBlock"></param>
        /// <param name="apples"></param>

        public TSP(Block startBlock, List<Block> apples)
        {
            int minCost;
            List<Block> cheapestPath;

            nodes.Add(startBlock);


            foreach (Block b in apples)
            {

                nodes.Add(b);

            }


        }


        /// <summary>
        /// classic TSP distance matrix
        /// </summary>
        /// <returns></returns>
        public float[,] distanceMatrix()
        {

            distance = new float[nodes.Count, nodes.Count];

            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes.Count; j++)
                {


                    distance[i, j] = findPath(nodes.ElementAt<Block>(i), nodes.ElementAt<Block>(j)).cost;
                    distance[j, i] = distance[i, j]; //the same price for the way there and back
                }



            }
            return distance;
        }


        /// <summary>
        /// returns the cheapest path 
        /// </summary>
        /// <returns></returns>
        public List<Block> BruteForce()
        {

            for (int i = 1; i <= faculty(nodes.Count - 1); i++)
            {


                List<Block> sublist = new List<Block>();
                sublist.Add(nodes.ElementAt<Block>(0));//all paths start at the player's starting position

            }

            List<List<Block>> paths = new List<List<Block>>(); //all possible paths

            //hier sollen später aller permutationen berechnet werden


            return null;
        }



        /// <summary>
        /// faculty
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public int faculty(int n)
        {
            int f = 1;
            for (int i = 1; i <= n; i++)
            {

                f *= i;

            }

            return f;

        }

    }*/
    }
}