using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour {

    public Player player;
    private SpriteRenderer spriteRendererSky0;
    private SpriteRenderer spriteRendererSky1;
    private SpriteRenderer spriteRendererSky2;
    private const float yVal = 10.1f;

	void Start () {
        spriteRendererSky0 = transform.Find("/Environment/Sky/Sky0").gameObject.GetComponent<SpriteRenderer>();
        spriteRendererSky1 = transform.Find("/Environment/Sky/Sky1").gameObject.GetComponent<SpriteRenderer>();
        spriteRendererSky2 = transform.Find("/Environment/Sky/Sky2").gameObject.GetComponent<SpriteRenderer>();
        Bounds boundsSky0 = spriteRendererSky0.bounds;
        Bounds boundsSky1 = spriteRendererSky1.bounds;
        Bounds boundsSky2 = spriteRendererSky2.bounds;

        // ahead
        spriteRendererSky2.transform.position = new Vector3(boundsSky1.max.x + boundsSky2.extents.x, yVal, 0);

        // behind
        boundsSky0 = spriteRendererSky0.bounds;
        boundsSky1 = spriteRendererSky1.bounds;
        spriteRendererSky0.transform.position = new Vector3(boundsSky1.min.x - boundsSky0.extents.x, yVal, 0);
	}

	void Update () {
	    Bounds b0 = spriteRendererSky0.bounds;
	    Bounds b1 = spriteRendererSky1.bounds;
	    Bounds b2 = spriteRendererSky2.bounds;

        if (player.transform.position.x > b1.max.x) {
            // 0 gets moved to end of 2, then all renumbered
            spriteRendererSky0.transform.position = new Vector3(b2.max.x + b0.extents.x, yVal, 0);
            SpriteRenderer tempOld1 = spriteRendererSky1;
            SpriteRenderer tempOld2 = spriteRendererSky2;
            spriteRendererSky2 = spriteRendererSky0;
            spriteRendererSky1 = tempOld2;
            spriteRendererSky0 = tempOld1;
        }

        else if (player.transform.position.x < b1.min.x) {
            // 2 gets moved to front of 0, then all renumbered
            spriteRendererSky2.transform.position = new Vector3(b0.min.x - b2.extents.x, yVal, 0);
            SpriteRenderer tempOld0 = spriteRendererSky0;
            SpriteRenderer tempOld1 = spriteRendererSky1;
            spriteRendererSky0 = spriteRendererSky2;
            spriteRendererSky1 = tempOld0;
            spriteRendererSky2 = tempOld1;
        }

        spriteRendererSky0.sortingOrder = 0;
        spriteRendererSky1.sortingOrder = 1;
        spriteRendererSky2.sortingOrder = 2;
	}

}
