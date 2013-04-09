using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using MiniJSON;

public class BScorePage : BPage{
    
    string url = "http://weh.bz/cgi-bin/application.cgi/scores";
    string seturl = "http://weh.bz/cgi-bin/application.cgi/submitscore";
    FContainer scoreArea;
    FLabel _bestScoreLabel;
    override public void Start(){
        //CoroutineRunner.StartFutileCoroutine(setScore("philz", BaseMain.Instance._score));
        SetupScroller();
        FSoundManager.PlayMusic("dino_menu");
        CoroutineRunner.StartFutileCoroutine(getScores());
        scoreArea = new FContainer();
        Futile.stage.AddChild(scoreArea);
        _bestScoreLabel = new FLabel("BitOut", "best scores!\n\n");
        scoreArea.AddChild(_bestScoreLabel);
        _bestScoreLabel.color = new Color(1.0f, 0.9f, 0.2f);
        _bestScoreLabel.anchorY = 1;
        _bestScoreLabel.y = Futile.screen.halfHeight/1.25f;
        _bestScoreLabel.shader = new FShader("Wavey", Shader.Find("Wavey"), 10);
    }
 
    IEnumerator getScores(){
        WWW www = new WWW(url);
        yield return www;
        IDictionary scores= (IDictionary)Json.Deserialize(www.text);
        
        SortedDictionary<string,long> sort = new SortedDictionary<string, long>();
        
        foreach(string key in scores.Keys){
            sort.Add(key, (long)scores[key]);
        }
        var x = sort.ToList();
        x.Sort(
            delegate(KeyValuePair<string,long> val1,
            KeyValuePair<string,long> val2)
            {
                return val2.Value.CompareTo(val1.Value);
            }
        );
        foreach(KeyValuePair<string,long> pair in x){
            _bestScoreLabel.text += (string.Format("{0}: {1}\n", pair.Key, pair.Value));
            yield return new WaitForSeconds(0.3f);
        }
        
    }
    
    IEnumerator setScore(string name, int score){
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("score", score);
        WWW www = new WWW(seturl, form.data, form.headers);
        yield return www;
        IDictionary scores = (IDictionary)Json.Deserialize(www.text);
        
        
        foreach(string key in scores.Keys){
            _bestScoreLabel.text += (string.Format("{0}: {1}\n",key, (long)scores[key]));
            yield return new WaitForSeconds(0.3f);
        }
        
        
    }
    
    void SetupScroller(){
        FContainer scrollContainer = new FContainer();
        for(float x = -(Futile.screen.halfWidth+16)*2; x<(Futile.screen.halfWidth+16)*2;x+=32){
            for(float y = -(Futile.screen.halfHeight+16)*2; y<(Futile.screen.halfHeight+16)*2;y+=32){
                FSprite s = new FSprite("drumstick.png");
                s.x = x;
                s.y = y;
                scrollContainer.AddChild(s);
            }
        }
        this.AddChild(scrollContainer);
        Go.to( scrollContainer, 10f, new TweenConfig()
                .floatProp("rotation", 360)
                .setIterations( -1, LoopType.RestartFromBeginning ));    
    }
    
    override public void HandleAddedToStage()
    {
        //Futile.instance.SignalUpdate += HandleUpdate;
        base.HandleAddedToStage();  
    }

    override public void HandleRemovedFromStage()
    {
        //Futile.instance.SignalUpdate -= HandleUpdate;
        base.HandleRemovedFromStage();  
    }
    
}
