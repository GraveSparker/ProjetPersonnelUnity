using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private new Collider2D collider;
    [SerializeField] private PhysicsMaterial2D frictionlessMaterial;
    [SerializeField] private PhysicsMaterial2D highFrictionMaterial;
    [SerializeField] private int damage = 2;

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            StartCoroutine(RespawnPoint());
        }
    }

    IEnumerator RespawnPoint()
    {
        PlayerMovement.Instance.pState.cutscene = true;
        Physics2D.IgnoreLayerCollision(6, 9, true);
        rigidBody.linearVelocity = Vector2.zero;
        yield return UIManager.Instance.sceneFader.StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.In));
        FindAnyObjectByType<PlayerHealth>().DamagePlayer(damage);
        PlayerMovement.Instance.transform.position = GameManager.Instance.platformingRespawnPoint;
        StartCoroutine(HoldPlayerStill(rigidBody));
        collider.sharedMaterial = highFrictionMaterial;
        yield return UIManager.Instance.sceneFader.StartCoroutine(UIManager.Instance.sceneFader.Fade(SceneFader.FadeDirection.Out));
        yield return new WaitForSecondsRealtime(0.1f);
        collider.sharedMaterial = frictionlessMaterial;
        PlayerMovement.Instance.pState.cutscene = false;
        Physics2D.IgnoreLayerCollision(6, 9, false);
    }

    IEnumerator HoldPlayerStill(Rigidbody2D rb)
    {
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
        }

        yield return new WaitForSecondsRealtime(0.2f);

        if (rb != null)
            rb.gravityScale = 1;
    }

}
