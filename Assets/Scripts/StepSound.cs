using UnityEngine;

public class StepSound : MonoBehaviour
{
    public AudioSource audioSource; // Assign an AudioSource with a footstep clip
    public AudioClip[] footstepSounds; // Array of different footstep sounds
    public float stepDelay = 0.3f; // Delay between steps

    private float stepCooldown;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            stepDelay = 0.3f;
        }
        else
        {
            stepDelay = 0.6f;
        }
        // Check if the player is pressing movement keys
        bool isMoving = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f;

        if (isMoving && Time.time > stepCooldown)
        {
            PlayFootstep();
            stepCooldown = Time.time + stepDelay;
        }
    }

    void PlayFootstep()
    {
        if (footstepSounds.Length > 0)
        {
            audioSource.clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
            audioSource.pitch = Random.Range(0.98f, 1.02f); // Randomize pitch for variation
            audioSource.Play();
        }
    }
}