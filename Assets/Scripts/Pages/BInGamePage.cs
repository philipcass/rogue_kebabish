using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BInGamePage : BPage{
    private BasePlayer _chef;
    private List<BaseFood> _meats;

    public int _multiplier { get; set; }

    private FContainer kebabSpit;
    private FContainer playArea;

    private FLabel _bestScoreLabel;
    private FLabel _timeLabel;
    public float _time { get; set; }
    public int _inprogScore { get; set; }
    Tween scoreTween;

    override public void Start(){
        FSprite map = new FSprite("map.png");
        this.AddChild(map);
        scoreTween = Go.to(this, 0.3f, new TweenConfig().intProp("_inprogScore", BaseMain.Instance._score));

        playArea = new FContainer();
        this.AddChild(playArea);
     
        _bestScoreLabel = new FLabel("BitOut", "");
        this.AddChild(_bestScoreLabel);
        _bestScoreLabel.scale = 0.5f;
        _bestScoreLabel.anchorX = 0.0f;
        _bestScoreLabel.anchorY = 1.0f;
        _bestScoreLabel.color = new Color(1.0f, 0.9f, 0.2f);
        _bestScoreLabel.x = -Futile.screen.halfWidth;
        _bestScoreLabel.y = Futile.screen.halfHeight;
        
        _timeLabel = new FLabel("BitOut", "clock: 30");
        _time = 30;
        this.AddChild(_timeLabel);
        _timeLabel.scale = 0.5f;
        _timeLabel.anchorX = 0.0f;
        _timeLabel.anchorY = -1.0f;
        _timeLabel.color = new Color(0.5f, 0.9f, 0.5f);
        _timeLabel.x = -Futile.screen.halfWidth;
        _timeLabel.y = -Futile.screen.halfHeight;
        
        kebabSpit = new FContainer();
        kebabSpit.y = Futile.screen.halfHeight - 16;
        kebabSpit.x += 64;
        playArea.AddChild(kebabSpit);

        _meats = new List<BaseFood>();
         
        _chef = new BasePlayer("chef.png");
        playArea.AddChild(_chef);
        pickAllergy();
        _multiplier = 1;
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
    
    void pickAllergy()
    {
        //foods[allergyInt].Value = -100;
        BaseFood f = BaseFood.randomFood();
        f.Value = -100;
        f = (BaseFood)f.Clone();
        f.scale = 2;
        playArea.AddChild(f);
        Go.to(f, 1f, new TweenConfig().setDelay(3).floatProp("scale", 1f).floatProp("y", Futile.screen.halfHeight - 16).onComplete(HandleGameStart));
    }
 
    void addMeat()
    {
        BaseFood meat =(BaseFood)BaseFood.randomFood().Clone();
        playArea.AddChild(meat);
        _meats.Add(meat);
        meat.x = Random.Range(-Futile.screen.halfWidth + 32, Futile.screen.halfWidth - 32);
        meat.y = Random.Range(-Futile.screen.halfHeight + 32, Futile.screen.halfHeight - 32);
        meat.x -= meat.x % 32;
        meat.y -= meat.y % 32;
        Go.to(meat, 0.3f, new TweenConfig().setEaseType(EaseType.BounceIn).floatProp("alpha", 1.0f));
    }
 
    void HandleGameStart(AbstractTween tween)
    {
        FSoundManager.PlayMusic("dino_ingame");
        for(int i = 0; i<10; i++) {
            addMeat();
        }
        //Counter for end of game
        Go.to(this, _time, new TweenConfig().floatProp("_time", 0).onComplete(HandleGameComplete));
    }
 
    // Update is called once per frame
    float frameCount = 0;
    public void HandleUpdate()
    {
        _timeLabel.text = string.Format("clock: {0}", _time);

        for(int i = 0; i < _meats.Count; i++) {
            float x = Vector2.Distance(new Vector2(_chef.x, _chef.y), new Vector2(_meats[i].x, _meats[i].y));
            Go.killAllTweensWithTarget(_meats[i]);
            Go.to(_meats[i],((x + 1) / 100f) /(_meats[i].Speed), new TweenConfig().floatProp("x", _chef.x).floatProp("y", _chef.y));
            Vector2 touchPos = _chef.GlobalToLocal(new Vector2(_meats[i].x, _meats[i].y));
            if(_chef.textureRect.Contains(touchPos)) {
                Go.to(_meats[i], 0.3f, new TweenConfig().floatProp("scale", 1.3f).floatProp("alpha", 0.0f).onComplete(HandleMeatCompleteScore));
                _meats.RemoveAt(i);
                i--;
                addMeat();
                frameCount = 0;
            }
        }
        frameCount += Time.deltaTime;
        if(frameCount > 2) {
            frameCount = 0;
            Go.to(_meats[0], 0.3f, new TweenConfig().floatProp("scale", 0f).floatProp("alpha", 0.0f).onComplete(HandleNodeComplete));
            _meats.RemoveAt(0);
            addMeat();
            _multiplier = 1;
        }

        if(Input.GetKeyDown(KeyCode.Space) && _time != 0){
            BaseFood top = (BaseFood)kebabSpit.GetChildAt(kebabSpit.GetChildCount()-1);
            if(top.Value>0){
                BaseMain.Instance._score += top.Value*_multiplier;
                FLabel mult = new FLabel("BitOut",string.Format("x{0}", _multiplier));
                mult.x = _chef.x;
                mult.y = _chef.y;
                mult.scale = 0.5f;
                playArea.AddChild(mult);
                Go.to(mult, 0.66f, new TweenConfig().floatProp("y", mult.y+10f).floatProp("alpha", 0.0f).onComplete(HandleNodeComplete));
                _multiplier++;
            }else{
                BaseMain.Instance._score += top.Value;
                _multiplier = 1;
            }
            top.RemoveFromContainer();
        }
        
        if(Input.GetKeyUp(KeyCode.Space) && _time == 0){
            Debug.Log("Loading Scores...");
            BaseMain.Instance.GoToPage(BPageType.ScorePage);
        }
        if ( scoreTween.state == TweenState.Destroyed)
            scoreTween = Go.to(this, 0.3f, new TweenConfig().intProp("_inprogScore", BaseMain.Instance._score));
        _bestScoreLabel.text = string.Format("score: {0}", this._inprogScore);
    }

    private void HandleGameComplete(AbstractTween tween){
        Go.to(playArea, 0.3f, new TweenConfig().floatProp("scale", 0f).floatProp("alpha", 0.0f));
        Go.to(_bestScoreLabel, 1f, new TweenConfig().setEaseType(EaseType.SineOut).floatProp("anchorX", 0.5f).floatProp("anchorY", 0.5f).floatProp("scale", 1f).floatProp("x", 0.0f).floatProp("y", 0.0f));
    }

    private void HandleNodeComplete(AbstractTween tween)
    {
        FNode sprite =(tween as Tween).target as FNode;
        sprite.RemoveFromContainer();
    }

    private void HandleMeatCompleteScore(AbstractTween tween)
    {
        BaseFood meatSprite =(tween as Tween).target as BaseFood;
        //Go.to(this,0.1f, new TweenConfig().intProp("_score",_score+meatSprite.Value));
        meatSprite.RemoveFromContainer();
        if(kebabSpit.GetChildCount() < 10){
            Go.killAllTweensWithTarget(meatSprite);
            meatSprite.x = meatSprite.y = 0;
            meatSprite.alpha = meatSprite.scale = 1;
            kebabSpit.AddChild(meatSprite);
            meatSprite.x = kebabSpit.GetChildCount()*16;
            _multiplier = 1;
        }
    }
}