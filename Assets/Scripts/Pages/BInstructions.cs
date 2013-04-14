using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using MiniJSON;

public class BInstructions : BPage{
    
    FContainer scoreArea;
    FLabel _instructions;
    override public void Start(){
        _instructions = new FLabel("BitOut", "how to play:\n-given an allergy\n-avoid allergy while \ncollecting other food\n-eat and gain points\n\n-wasd to move\n-spacebar to eat");
        _instructions.color = new Color(1.0f, 0.9f, 0.2f);
        this.AddChild(_instructions);
    }
 
    override public void HandleAddedToStage()
    {
        Futile.instance.SignalUpdate += HandleUpdate;
        base.HandleAddedToStage();  
    }

    override public void HandleRemovedFromStage()
    {
        Futile.instance.SignalUpdate -= HandleUpdate;
        base.HandleRemovedFromStage();  
    }
    
    void HandleUpdate(){
        if(Input.GetKeyUp(KeyCode.Space)){
                BaseMain.Instance.GoToPage(BPageType.MainMenu);
        }
    }

}
