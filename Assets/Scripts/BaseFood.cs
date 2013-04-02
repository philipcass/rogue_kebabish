using System;
using UnityEngine;

//type-safe-enum pattern
public class BaseFood:FSprite,ICloneable
{
    public int Value{ get; set; }

    public float Speed{ get; set; }

    public static readonly BaseFood HAM = new BaseFood("ham.png", 10, 1);
    public static readonly BaseFood STEAK = new BaseFood("steak.png", 10, 0.66f);
    public static readonly BaseFood CHICKEN = new BaseFood("chicken.png", 10, 0.33f);
 
    public BaseFood(string elementName, int value, float speed):base(elementName)
    {
        Value = value;
        Speed = speed;

    }
 
    public object Clone()
    {
        return this.MemberwiseClone();
    }

}