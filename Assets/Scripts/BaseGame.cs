using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseGame : MonoBehaviour
{
 
    private BasePlayer _chef;
    private List<BaseFood> _meats;

    public int _score { get; set; }

    private FLabel _bestScoreLabel;
    // Use this for initialization
    void Start()
    {
        FutileParams fparams = new FutileParams(true, false, false, false);

        fparams.AddResolutionLevel(480.0f, 1.0f, 1.0f, ""); 
        fparams.origin = new Vector2(0.5f, 0.5f);

        Futile.instance.Init(fparams);

        Futile.atlasManager.LoadAtlas("Atlases/Game");
        Futile.atlasManager.LoadFont("Arial", "Arial_0.png", "Atlases/Arial");
     
        FSprite map = new FSprite("map.png");
        Futile.stage.AddChild(map);
     
        _bestScoreLabel = new FLabel("Arial", "Best score: 0 Bananas");
        Futile.stage.AddChild(_bestScoreLabel);
        _bestScoreLabel.scale = 0.5f;
        _bestScoreLabel.anchorX = 0.0f;
        _bestScoreLabel.anchorY = 1.0f;
        _bestScoreLabel.color = new Color(1.0f, 0.9f, 0.2f);
        _bestScoreLabel.x = -Futile.screen.halfWidth;
        _bestScoreLabel.y = Futile.screen.halfHeight;

        _meats = new List<BaseFood>();
         
        _chef = new BasePlayer("chef.png");
        Futile.stage.AddChild(_chef);
        pickAllergy();
    }
 
    BaseFood randomFood()
    {
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
 
    void pickAllergy()
    {
        //foods[allergyInt].Value = -100;
        BaseFood f = randomFood();
        f.Value = -100;
        f =(BaseFood)f.Clone();
        f.scale = 2;
        Futile.stage.AddChild(f);
        Go.to(f, 1f, new TweenConfig().setDelay(3).floatProp("scale", 1f).floatProp("y", Futile.screen.halfHeight - 16).onComplete(HandleStartFood));
    }
 
    void addMeat()
    {
        BaseFood meat =(BaseFood)randomFood().Clone();
        Futile.stage.AddChild(meat);
        _meats.Add(meat);
        meat.x = Random.Range(-Futile.screen.halfWidth + 32, Futile.screen.halfWidth - 32);
        meat.y = Random.Range(-Futile.screen.halfHeight + 32, Futile.screen.halfHeight - 32);
        meat.x -= meat.x % 32;
        meat.y -= meat.y % 32;
        Go.to(meat, 0.3f, new TweenConfig().setEaseType(EaseType.BounceIn).floatProp("alpha", 1.0f));
    }
 
    void HandleStartFood(AbstractTween tween)
    {
        for(int i = 0; i<10; i++) {
            addMeat();
        }
    }
 
    // Update is called once per frame
    float frameCount = 0;

    void Update()
    {
        _bestScoreLabel.text = string.Format("Score: {0}", _score);
     
        for(int i = 0; i < _meats.Count; i++) {
            float x = Vector2.Distance(new Vector2(_chef.x, _chef.y), new Vector2(_meats [i].x, _meats [i].y));
            Go.killAllTweensWithTarget(_meats [i]);
            Go.to(_meats [i],((x + 1) / 100f) /(_meats [i].Speed), new TweenConfig().floatProp("x", _chef.x).floatProp("y", _chef.y));
            Vector2 touchPos = _chef.GlobalToLocal(new Vector2(_meats [i].x, _meats [i].y));
            if(_chef.textureRect.Contains(touchPos)) {
                Go.to(_meats [i], 0.3f, new TweenConfig().floatProp("scale", 1.3f).floatProp("alpha", 0.0f).onComplete(HandleMeatComplete));
                _score += _meats [i].Value;
                _meats.RemoveAt(i);
                i--;
                addMeat();
                frameCount = 0;
            }
        }
        frameCount += Time.deltaTime;
        if(frameCount > 2) {
            frameCount = 0;
            Go.to(_meats [0], 0.3f, new TweenConfig().floatProp("scale", 0f).floatProp("alpha", 0.0f).onComplete(HandleMeatComplete));
            _meats.RemoveAt(0);
            addMeat();
        }
    }

    private void HandleMeatComplete(AbstractTween tween)
    {
        BaseFood meatSprite =(tween as Tween).target as BaseFood;
        //Go.to(this,0.1f, new TweenConfig().intProp("_score",_score+meatSprite.Value));
        meatSprite.RemoveFromContainer();
    }
}
