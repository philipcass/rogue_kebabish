using System;
using UnityEngine;

public class BasePlayer : FSprite
{
    public static BasePlayer Instance;
    
    public BasePlayer(string elementName) : base(elementName)
    {
        Instance = this;
        Futile.instance.SignalUpdate += HandleUpdate;
    }
 
    public void HandleUpdate()
    {
        UpdateMovement();
    }
 
    Vector3 p = new Vector3();
    private void UpdateMovement()
    {
#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
        if(Input.touchCount == 1)
             p = Futile.instance.camera.ScreenToWorldPoint(Input.GetTouch(0).position);
#else
        p = Futile.instance.camera.ScreenToWorldPoint(Input.mousePosition);
#endif
        p = Vector3.Max(p, new Vector3(-Futile.screen.halfWidth+32, -Futile.screen.halfHeight+32, 0));
        p = Vector3.Min(p, new Vector3(Futile.screen.halfWidth-32, Futile.screen.halfHeight-32, 0));
        float dist = Vector2.Distance(new Vector2(this.x, this.y), new Vector2(p.x, p.y));

        Go.killAllTweensWithTarget(this);
        Go.to(this, 1.0f/dist, new TweenConfig().floatProp("x", p.x).floatProp("y", p.y));

    }
}

