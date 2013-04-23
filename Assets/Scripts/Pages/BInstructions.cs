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
        _instructions = new FLabel("BitOut", "you have been afflicted with\na terrible random allergy,\n ...let's say by a wizard.\n\nCollect other foods\nto gain points.\nMovement: Mouse/Touch ");
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
        if(Input.GetMouseButtonUp(0)){
                BaseMain.Instance.GoToPage(BPageType.InGamePage);
        }
    }

}
