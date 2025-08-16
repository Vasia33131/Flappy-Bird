using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ScoreTrigger : MonoBehaviour
{
    private bool isTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            isTriggered = true;
            GetComponent<BoxCollider2D>().enabled = false;

            GameManager.Instance?.AddScore();
            PlayScoreSound();
            Destroy(gameObject, 0.1f);
        }
    }

    private void PlayScoreSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayScoreSound();
        }
        else
        {
            AudioClip clip = Resources.Load<AudioClip>("ScoreSound");
            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, transform.position);
            }
        }
    }
}