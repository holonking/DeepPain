using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGCore
{
    public class Grammar:Node
    {
        public int currentStep=-1;
        public List<SGIO> stagedOutputs;
        public List<Rule> rules;
        public List<ShapeObject> assignedObjects;

        public Grammar():base()
        {
            stagedOutputs = new List<SGIO>();
            rules = new List<Rule>();
        }
        public void AddRule(Rule r, bool execute = true)
        {
            rules.Add(r);
        }
        public void SubRule(Rule r, bool execute = true)
        {
            int index = rules.IndexOf(r);
            rules.Remove(r);
            if (execute && index > 0 && index < rules.Count)
            {
                Execute(index);
            }
        }
        
        public void Execute()
        {
            for (int i = 0; i < rules.Count; i++)
            {
                Execute(i);
            }
        }
        public void Execute(int i)
        {
            currentStep = i;
            SGIO tobeMerged = PreExecution(i);
            rules[i].Execute();
            PostExecution(i,tobeMerged);
        }
        public SGIO PreExecution(int step)
        {
            List<ShapeObject> availableShapes;
            List<ShapeObject> outShapes=new List<ShapeObject>();
            SGIO tobeMerged=new SGIO();

            //get available shapes
            if (step == 0)
            {
                if (assignedObjects.Count > 0)
                {
                    rules[0].inputs.shapes = assignedObjects;
                    return tobeMerged;
                }
                availableShapes = inputs.shapes;
            }
            else
            {
                availableShapes = stagedOutputs[step-1].shapes;
            }

            //select shapes from available shapes base on required names 
            foreach(ShapeObject o in availableShapes)
            {
                if (rules[step].inputs.names.Contains(o.name))
                    outShapes.Add(o);
                else
                    if (!tobeMerged.names.Contains(o.name))
                        tobeMerged.names.Add(o.name);
                    tobeMerged.shapes.Add(o);
            }
            rules[step].inputs.shapes = availableShapes;
            return tobeMerged;

        }
        public void PostExecution(int step, SGIO tobeMerged)
        {
            SGIO tempOut=new SGIO();
            tempOut = SGIO.Merge(rules[step].outputs, tobeMerged);

            if (step >= stagedOutputs.Count)
            {
                stagedOutputs.Add(tempOut);
            }
            else
            {
                //remove outdated objects
                foreach (ShapeObject o in stagedOutputs[step].shapes)
                {
                    if (!tempOut.shapes.Contains(o))
                        GameObject.Destroy(o);
                }
                //add additional shape objects
                foreach (ShapeObject o in tempOut.shapes)
                {
                    if (!stagedOutputs[step].shapes.Contains(o))
                        stagedOutputs[step].shapes.Add(o);
                }
                //add additional names
                foreach (string n in tempOut.names)
                {
                    if (!stagedOutputs[step].names.Contains(n))
                        stagedOutputs[step].names.Add(n);
                }
            }
        }
        public void SelectStep(int i) { }

    }
}

