using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private SpriteRenderer  spriteRenderer;
    [SerializeField] private SpriteRenderer  playerSpriteRenderer;
    [SerializeField] private Color color;

    private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;
    private float alphaSet = 0.8f;
    private float alphaMultiplier = 0.85f;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindObjectOfType<Player>().transform;
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        spriteRenderer.sprite = playerSpriteRenderer.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;
    }

    private void Update() 
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, alpha);
        spriteRenderer.color = color;

        if(Time.time >= (timeActivated + activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
