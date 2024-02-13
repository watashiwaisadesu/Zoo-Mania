using System.Collections.Generic;
using UnityEngine;

public class ColorPiece : MonoBehaviour
{
    [System.Serializable]
    public struct ColorSprite
    {
        public ColorType color; 
        public AnimationClip animationName; // Change to string
    }

    public ColorSprite[] colorSprites;
    [SerializeField] private ColorType _color;
    
    public ColorType Color
    {
        get => _color;
        set => SetAnimal(value);
    }
    
    public int NumColors => colorSprites.Length;
   
    [SerializeField] private Animator _animatorGFX;
    private Dictionary<ColorType, string> _colorSpriteDict; // Change the value type to string
    
    private void Awake ()
    {
        // instantiating and populating a Dictionary of all Color Types / Animation names (for fast lookup)
        _colorSpriteDict = new Dictionary<ColorType, string>();
        
        for (int i = 0; i < colorSprites.Length; i++)
        {
            _colorSpriteDict.TryAdd(colorSprites[i].color, colorSprites[i].animationName.name);
        }
    }
    
    private void SetAnimal(ColorType newColor)
    {
        _color = newColor;
        if (_animatorGFX)
        {
            if (_colorSpriteDict.TryGetValue(newColor, out var value))
            {
                _animatorGFX.Play(value);
            }
        }
    }
}