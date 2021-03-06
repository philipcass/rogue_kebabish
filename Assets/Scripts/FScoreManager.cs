using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using MiniJSON;

public class FScoreManager
{
    static string url = "http://weh.bz/cgi-bin/application.cgi/scores";
    static string seturl = "http://weh.bz/cgi-bin/application.cgi/submitscore";
    static int scoreLimit = 5;
    static WWW www;
    public static bool isDone{
        get{
            return www.isDone;
        }
    }
    
    static public OrderedDictionary Scores{get; protected set;}
 
    public static void Init(){
        Update();
    }
    
    public static void Update(){
        CoroutineRunner.StartFutileCoroutine(getScores());
    }
    
    public static void PostScore(string name, int score){
        CoroutineRunner.StartFutileCoroutine(setScore(name, score));
    }
    
    static IEnumerator getScores(){
        www = new WWW(url);
        yield return www;
        
        ParseScores(www.text);
    }
    
    static IEnumerator setScore(string name, int score){
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("score", score);
        www = new WWW(seturl, form.data, form.headers);
        yield return www;
        
        ParseScores(www.text);
    }
    
    static void ParseScores(string json){
        Debug.Log("==========PARSING SCOREZ");
        int i = 0;
        IDictionary scores = ((IDictionary)Json.Deserialize(json));
        Dictionary<string,long> sort = new Dictionary<string, long>();
        foreach(string key in scores.Keys){
            if(i++ == scoreLimit)
                break;
            sort.Add(key, (long)scores[key]);
        }
        OrderedDictionary sort2 = new OrderedDictionary();
        foreach (KeyValuePair<string, long> pair in sort.OrderByDescending(key => key.Value)){
            sort2.Add(pair.Key, (long)scores[pair.Key]);
        }
        Scores = sort2;
    }
}