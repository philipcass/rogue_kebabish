using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using MiniJSON;

public class BScorePage : BPage{
    
    FContainer scoreArea;
    FLabel _bestScoreLabel;
    override public void Start(){
        //CoroutineRunner.StartFutileCoroutine(setScore("philz", BaseMain.Instance._score));
        SetupScroller();

        FSoundManager.PlayMusic("dino_menu");

        scoreArea = new FContainer();
        this.AddChild(scoreArea);
        _bestScoreLabel = new FLabel("BitOut", "best scores!\n\n");
        scoreArea.AddChild(_bestScoreLabel);
        _bestScoreLabel.color = new Color(1.0f, 0.9f, 0.2f);
        _bestScoreLabel.anchorY = 1;
        _bestScoreLabel.y = Futile.screen.halfHeight/1.25f;
        _bestScoreLabel.shader = new FShader("Wavey", Shader.Find("Wavey"), 10);
        
        CoroutineRunner.StartFutileCoroutine(PausedText());
    }
    
    IEnumerator PausedText(){
        while(FScoreManager.Scores == null){
            yield return new WaitForSeconds(0.3f);
        }

        foreach (string key in FScoreManager.Scores.Keys){
            _bestScoreLabel.text += (string.Format("{0}: {1}\n", key, FScoreManager.Scores[key]));
            yield return new WaitForSeconds(0.3f);
        }
    }
 
    void SetupScroller(){
        FRepeatSprite rs = new FRepeatSprite("drumstick2", Futile.screen.width*2, Futile.screen.height*2);
        FContainer scrollContainer = new FContainer();
        scrollContainer.AddChild(rs);
        this.AddChild(scrollContainer);
        
        Go.to( scrollContainer, 10f, new TweenConfig()
                .floatProp("rotation", 360)
                .setIterations( -1, LoopType.RestartFromBeginning ));    
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
            BaseMain.Instance.GoToPage(BPageType.MainMenu);
        }
    }

}
