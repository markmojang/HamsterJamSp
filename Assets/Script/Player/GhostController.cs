using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float delay = 2.0f;
    float delta = 0;

    PlayerController player;
    SpriteRenderer spriteRenderer;
    public float destroyTime = 0.1f;
    public Color color;
    public Material material = null;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (delta > 0) { delta -= Time.deltaTime; }
        else { delta = delay; createGhost(); }
    }

    void createGhost()
    {
        GameObject ghostObj = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghostObj.transform.localScale = player.transform.localScale;

        spriteRenderer = ghostObj.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = player.spriteRenderer.sprite;
        spriteRenderer.color = color;
        if (material != null) { spriteRenderer.material = material; }

        StartCoroutine(FadeOutAndDestroy(ghostObj, destroyTime));
    }

    IEnumerator FadeOutAndDestroy(GameObject ghostObj, float destroyTime)
    {
        SpriteRenderer ghostSpriteRenderer = ghostObj.GetComponent<SpriteRenderer>();
        Color originalColor = ghostSpriteRenderer.color;

        for (float t = 0; t < destroyTime; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, t / destroyTime);
            ghostSpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(ghostObj);
    }

}
