using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderScript : MonoBehaviour
{
    public static ShaderScript instance;

    void Awake()
    {
        //Un riferimento al renderer del gameobject e' necessario per usare i metodi seguenti
        if(instance == null)
            instance = this;
        else
            Destroy(this);
    }


    //Quando si parla di ColorStringName si intendono i nomi degli slot di colore con sintassi "_PrimaryColor", "_SecondaryColor" e "_TertiaryColor"
    //es.
    //SetColorInShader("_PrimaryColor", rend, hexadecimal);
    //
    //I metodi seguenti mancano modi per catturare eccezioni quindi potrebbero risultare in errori


    //Questo metodo ritorna il colore presente nello shader in questo momento basato su ColorStringName
    public Color GetColorFromShader(string ColorStringName, Renderer RendererRef)
    {
        return RendererRef.sharedMaterial.GetColor(ColorStringName);
    }


    //Questo metodo setta il colore presente nello shader in questo momento basato su ColorStringName e un riferimento a un colore 
    public void SetColorInShader(string ColorStringName, ref Renderer RendererRef, Color color)
    {
        RendererRef.sharedMaterial.SetColor(ColorStringName, color);
    }
    

    //Questo metodo converte un valore esadecimale in colore
    public Color HexToColor(string HexString)
    {
        Color ColorTest;
        HexString = "#"+ HexString;
        ColorUtility.TryParseHtmlString(HexString, out ColorTest);
        return ColorTest;
    }


    //Questo metodo converte un colore in un valore esadecimale
    public string ColorToHex(Color color)
    {
        string test = ColorUtility.ToHtmlStringRGB(color);
        return test;
    }

    
}
