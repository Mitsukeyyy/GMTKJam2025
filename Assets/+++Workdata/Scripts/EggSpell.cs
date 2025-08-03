using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class EggSpell : MonoBehaviour
{
    private SpriteRenderer sr;

    //public Color flashColor = Color.black;
    public float flashDuration = 0.4f;
    public Color flashColor = Color.blue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        Color c = sr.color;
        Debug.Log($"Color r:{c.r:F2} g:{c.g:F2} b:{c.b:F2} a:{c.a:F2}");
    }
    
    public void FlashSprite()
    {
        sr.DOColor(flashColor, flashDuration).SetLoops(4, LoopType.Yoyo).SetEase(Ease.InOutSine);
      
    }
}
