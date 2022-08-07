using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _audioSourceCrash;
    [SerializeField] private AudioClip _miniCollisonClip;
    [SerializeField] private float _maxPitch;
    [SerializeField] private float _minPitch;
    private float _speed = 2;
    public void PlayAccelerationSong()
    {
        _audioSource.pitch = Mathf.Clamp(_audioSource.pitch + Time.fixedDeltaTime, _minPitch, _maxPitch);
    }

    public void PlayDeAccelerationSong()
    {
        _audioSource.pitch = Mathf.Clamp(_audioSource.pitch - (Time.fixedDeltaTime * _speed), _minPitch, _maxPitch);
    }

    public void PlayCollisionClip()
    {
        _audioSourceCrash.pitch = Random.Range(1f, 2f);
        _audioSourceCrash.volume = Random.Range(0.1f, 0.5f);
        _audioSourceCrash.PlayOneShot(_miniCollisonClip);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GenerationMap>() || other.CompareTag("Finish")) PlayCollisionClip();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<GenerationMap>() || other.transform.CompareTag("Finish"))
        {
            if (transform.eulerAngles.x > 90 || transform.eulerAngles.x < -120 ||
            transform.eulerAngles.z > 90 || transform.eulerAngles.x < -120)
            {
                _audioSourceCrash.pitch = 1f;
                _audioSourceCrash.volume = 1f;
                _audioSourceCrash.Play();
            }
        }
    }

}
