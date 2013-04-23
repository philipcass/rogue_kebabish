using System;
using UnityEngine;

//type-safe-enum pattern
public class BaseFood:FSprite,ICloneable
{
    public int Value{ get; set; }

    public float Speed{ get; set; }

    public static readonly BaseFood HAM = new BaseFood("ham.png", 10, 0.80f);
    public static readonly BaseFood STEAK = new BaseFood("steak.png", 10, 0.50f);
    public static readonly BaseFood CHICKEN = new BaseFood("chicken.png", 10, 0.20f);
 
    public BaseFood(string elementName, int value, float speed):base(elementName)
    {
        Value = value;
        Speed = speed;

    }
 
    public static BaseFood randomFood(){
        int allergyInt = RXRandom.Int(3);
        switch(allergyInt) {
        case(0):
            return BaseFood.HAM;
        case(1):
            return BaseFood.STEAK;
        case(2):
            return BaseFood.CHICKEN;
        default:
            return null;
        }
    }
 

    
    public object Clone()
    {
        return this.MemberwiseClone();
    }

}