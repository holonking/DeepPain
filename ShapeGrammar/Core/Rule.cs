using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SGCore
{
    public class Rule : Node
    {
        public Grammar grammar;
        

        public Rule()
        {

        }
        public virtual void Execute()
        {
            //operate on meshables and update existing shapeobjects
        }
        public virtual void ExecuteShape()
        {

        }
    }
}
