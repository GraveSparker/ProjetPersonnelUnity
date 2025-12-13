using UnityEngine;

public class CollisionDataRetriever : MonoBehaviour
{
    public bool OnGround { get; private set; }
    public bool OnWall { get; private set; }
    public float Friction { get; private set; }
    public Vector2 ContactNormal { get; private set; }

    private PhysicsMaterial2D material;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnGround = false;
        Friction = 0;
        OnWall = false;
    }

    public void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactNormal = collision.GetContact(i).normal;
            OnGround |= ContactNormal.y >= 0.9f;
            OnWall = Mathf.Abs(ContactNormal.x) >= 0.9f;
        }
    }

    private void RetrieveFriction(Collision2D collision)
    {
        // Default value
        Friction = 0f;

        // Make sure collider exists
        if (collision == null || collision.collider == null)
            return;

        // Get Physics Material from the collider
        material = collision.collider.sharedMaterial;

        // If material exists, assign friction
        if (material != null)
        {
            Friction = material.friction;
        }
    }

    public bool GetOnGround()
    {
        return OnGround;
    }

    public float GetFriction()
    {
        return Friction;
    }
}
