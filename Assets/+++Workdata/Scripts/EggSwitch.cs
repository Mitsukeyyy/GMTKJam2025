using UnityEngine;

public class EggSwitch : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Sprite[] eggFrames;

    private int currentFrame = 0;
    private bool isEgg = false;

    public void TriggerEggSpell()
    {
        isEgg = true;
        currentFrame = 0;

        animator.enabled = false;
        spriteRenderer.sprite = eggFrames[currentFrame];
    }

    public void AdvanceEggFrame()
    {
        if (!isEgg || eggFrames.Length < 2) return;

        currentFrame = (currentFrame + 1) % 2; 
        spriteRenderer.sprite = eggFrames[currentFrame];
    }

    public void RevertToSalamander()
    {
        isEgg = false;
        animator.enabled = true;
        
    }
    public void SetEggFrame(int frameIndex)
    {
        if (eggFrames.Length == 0 || frameIndex >= eggFrames.Length) return;

        currentFrame = frameIndex;
        spriteRenderer.sprite = eggFrames[currentFrame];
    }
}