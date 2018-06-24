using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollectableItem : MonoBehaviour {

    Collider _collider;
    Renderer[] renderers;
    AudioSource _audioSource;
    public AudioClip collectedAudioClip;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        _audioSource = GetComponent<AudioSource>();
        _collider = GetComponent<Collider>();
        _audioSource.clip = collectedAudioClip;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectItem(other);
        }
    }

    public virtual void CollectItem(Collider other)
    {
        _audioSource.Play();
        _collider.enabled = false;

        foreach (var render in renderers)     
            render.enabled = false;

        Destroy(gameObject, 3f);
    }
}
