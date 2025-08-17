using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    //private int bgIndex = 0;
    [SerializeField]
    private float scrollSpeed = 1f;
    private float resetDistance = -30f;
    private Vector2 scrollDirection = new Vector2(-1, -1f);
    private Vector2 startPoint = new Vector2(50f, 50f);

    [SerializeField]
    private Transform[] bgTransforms;

    private void Update()
    {
        for (int i = 0; i < bgTransforms.Length; i++)
        {
            bgTransforms[i].position += (Vector3)scrollDirection * scrollSpeed * Time.deltaTime;

            if (bgTransforms[i].position.x <= resetDistance)
            {
                bgTransforms[i].position = startPoint;
            }
        }
    }
}
