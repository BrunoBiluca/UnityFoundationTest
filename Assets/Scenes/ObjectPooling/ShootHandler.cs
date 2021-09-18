using Assets.UnityFoundation.Code.ObjectPooling;
using System.Collections;
using UnityEngine;

public class ShootHandler : PooledObject {

    private float speed = 3f;
    
    private float maxVerticalPosition;
    private Vector2 endPosition;

    void OnEnable() {
        maxVerticalPosition = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, 1)).y;
        endPosition = new Vector2(transform.position.x, maxVerticalPosition + 1);

        StartCoroutine(Move(speed));
    }

    void Update() {
        if(transform.position.y > maxVerticalPosition) {
            Deactivate();
        }
    }

    private IEnumerator Move(float duration) {
        float elapsedTime = 0;
        Vector2 startingPos = transform.position;

        while(elapsedTime < duration) {
            transform.position = Vector3.Lerp(startingPos, endPosition, (elapsedTime/duration));
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

}
