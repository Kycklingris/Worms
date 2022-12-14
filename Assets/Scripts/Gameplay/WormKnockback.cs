 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormKnockback : MonoBehaviour
{
    private CharacterController controller;
    private bool groundedPlayer;
    private bool knockback;
    private Vector3 velocity;
    private float gravityValue = -9.81f;
    private float knockbackTime = 0.5f;
    private float endTime;
    private GameObject origin;

    void Awake()
    {
        this.controller = gameObject.GetComponent(typeof(CharacterController)) as CharacterController;
        this.endTime = Time.time + this.knockbackTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!this.knockback)
        {
            return;
        }

        this.velocity.y += this.gravityValue * Time.deltaTime;
        controller.Move(this.velocity * Time.deltaTime);

        if (this.endTime > Time.time)
        {
            return;
        }

        this.groundedPlayer = controller.isGrounded;
        if (this.groundedPlayer)
        {
            this.velocity = Vector3.zero;
            this.knockback = false;

            if (this.origin != null)
            {
                this.origin.SendMessage("OnLanded", this.gameObject);
                this.origin = null;
            }

            return;
        }
    }

    public void Knockback(DamageClass damage)
    {
        this.velocity = damage.direction * damage.knockback;
        this.knockback = true;
        this.origin = damage.origin;
    }

    public void Death()
    {
        if (this.origin != null)
        {
            this.origin.SendMessage("OnLanded", this.gameObject);
            this.origin = null;
        }
    }
}
