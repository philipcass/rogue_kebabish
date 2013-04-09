using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BPageType
{
    None,
    InGamePage,
    ScorePage,
}

public class BaseMain : MonoBehaviour
{
    public static BaseMain Instance;
    private BPageType _currentPageType = BPageType.None;
    private BPage _currentPage = null;
    public int _score { get; set; }

    private FStage _stage;
    // Use this for initialization
    void Start ()
    {
        Instance = this;
        FutileParams fparams = new FutileParams (true, false, false, false);

        fparams.AddResolutionLevel (480.0f, 1.0f, 1.0f, ""); 
        fparams.origin = new Vector2 (0.5f, 0.5f);

        Futile.instance.Init (fparams);

        Futile.atlasManager.LoadAtlas ("Atlases/Game");
        Futile.atlasManager.LoadFont ("BitOut", "BitOut_0.png", "Atlases/BitOut");
        
        _stage = Futile.stage;

        GoToPage(BPageType.InGamePage);

    }

    public void GoToPage (BPageType pageType)
    {
        if (_currentPageType == pageType)
            return; //we're already on the same page, so don't bother doing anything

        BPage pageToCreate = null;

        if (pageType == BPageType.InGamePage) {
            pageToCreate = new BInGamePage ();
        } else if (pageType == BPageType.ScorePage){
            pageToCreate = new BScorePage ();
        }

        if (pageToCreate != null) { //destroy the old page and create a new one
            _currentPageType = pageType;    

            if (_currentPage != null) {
                _stage.RemoveChild (_currentPage);
            }

            _currentPage = pageToCreate;
            _stage.AddChild (_currentPage);
            _currentPage.Start ();
        }

    }
}
