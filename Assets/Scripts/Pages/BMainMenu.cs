using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using MiniJSON;

public class BMainMenu : BPage{
    
    FLabel _title;
    string[] selection = {"*"," "," "};
    string menuString = "rOGUE\nkEBABISH!\n\n{0}play{0}\n{1}instructions{1}\n{2}scores{2}";
    override public void Start(){
        //CoroutineRunner.StartFutileCoroutine(setScore("philz", BaseMain.Instance._score));
        FSoundManager.PlayMusic("dino_menu");
        _title = new FLabel("BitOut", string.Format(menuString, selection));
        this.AddChild(_title);
        _title.color = new Color(1.0f, 0.9f, 0.2f);
        //_bestScoreLabel.y = Futile.screen.halfHeight/1.25f;
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
    int menuIndex = 0;
    void HandleUpdate(){
        if(Input.GetKeyUp(KeyCode.S) && menuIndex < selection.Length-1){
            selection[menuIndex] = " ";
            menuIndex++;
            selection[menuIndex] = "*";        
        } else if(Input.GetKeyUp(KeyCode.W) && menuIndex >0){
            selection[menuIndex] = " ";
            menuIndex--;
            selection[menuIndex] = "*";        
        }
        _title.text = string.Format(menuString, selection);
        
        if(Input.GetKeyUp(KeyCode.Space)){
            switch(menuIndex){
            case 0:
                BaseMain.Instance.GoToPage(BPageType.InGamePage);
                break;
            case 1:
                BaseMain.Instance.GoToPage(BPageType.Instructions);
                break;
            case 2:
                BaseMain.Instance.GoToPage(BPageType.ScorePage);
                break;
            }
        }
    }
}
