using UnityEngine;
using System;
using System.Collections;

public class FKeyBoard : FContainer
{
    private System.Action _update;
    System.Action<string> _getText;
    public String Text {get; set;}
    
    public FKeyBoard (string fontName, System.Action parentUpdate, System.Action<string> getText)
    {
        _getText = getText;
        
        int asciiStarter = (int)'a';
        for(float y = 0; y > -Futile.screen.halfWidth; y -= 16*3){
            for(float x = -Futile.screen.halfWidth+16*3; x < Futile.screen.halfWidth-16*3; x += 16*3){
                if (asciiStarter > (int)'z')
                    break;
                FLabel c = new FLabel(fontName, ((char)asciiStarter).ToString());
                c.x = x;
                c.y = y;
                c.scale = 3;
                this.AddChild(c);
                asciiStarter++;
            }
        }
        _update = parentUpdate;
        this.Text = "";
    }

    override public void HandleAddedToStage()
    {
        Futile.instance.SignalUpdate += HandleUpdate;
        Futile.instance.SignalUpdate -= _update;
        base.HandleAddedToStage();  
    }

    override public void HandleRemovedFromStage()
    {
        Futile.instance.SignalUpdate -= HandleUpdate;
        Futile.instance.SignalUpdate += _update;
        base.HandleRemovedFromStage();  
    }
    
    void HandleUpdate(){
        if(Input.GetMouseButtonUp(0)){
            int y = (int)Mathf.Abs(Futile.instance.camera.ScreenToWorldPoint(Input.mousePosition).y / (16*3));
            int iconsPerWidth = (int)Mathf.Abs(Futile.screen.width/(16*3))-1;
            int x = (int)(Futile.instance.camera.ScreenToWorldPoint(Input.mousePosition).x + Futile.screen.halfWidth - (16*1.5)) / (16*3);
            this.Text +=(char)(((int)'a')+(y*iconsPerWidth+x));
        }
        Debug.Log(this.Text);
        _getText(this.Text);
        if(this.Text.Length == 3)
            this.RemoveFromContainer();
    }
}
