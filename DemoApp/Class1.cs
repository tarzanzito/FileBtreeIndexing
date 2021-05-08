using System;
using System.Collections.Generic;
using System.Text;

namespace DemoApp
{
    public abstract class MyBase
    {
        public MyBase()
        {

        }
    }


    public class MyXpto : MyBase
    {
        [AnimalType(10)]
        public string name;
    }



    // A custom attribute to allow a target to have a pet.
    [AttributeUsage(AttributeTargets.Field)]
    public class AnimalTypeAttribute : Attribute
    {
        // The constructor is called when the attribute is set.
        public AnimalTypeAttribute(int pet)
        {
            thePet = pet;
        }

        // Keep a variable internally ...
        protected int thePet;

        // .. and show a copy to the outside world.
        public int Pet
        {
            get { return thePet; }
            set { thePet = value; }
        }
    }
}
