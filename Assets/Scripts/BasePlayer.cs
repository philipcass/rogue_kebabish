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
 
    private void UpdateMovement()
    {
        if(this.y < Futile.screen.halfHeight - 48)
            if(Input.GetKey(KeyCode.W))
                this.y += 4;
        if(this.y > -Futile.screen.halfHeight + 48)
            if(Input.GetKey(KeyCode.S))
                this.y -= 4;
        if(this.x < Futile.screen.halfWidth - 48)
            if(Input.GetKey(KeyCode.D))
                this.x += 4;
        if(this.x > -Futile.screen.halfWidth + 48)
            if(Input.GetKey(KeyCode.A))
                this.x -= 4;

    }
}

