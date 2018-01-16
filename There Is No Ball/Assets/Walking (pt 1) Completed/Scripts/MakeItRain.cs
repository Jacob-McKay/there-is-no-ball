using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeItRain : MonoBehaviour {

    public GameObject ragdollPrefab;
    public AudioClip audioClip;
    private AudioSource _audioSource;

    private bool _alreadyKickedIt = false;

    // Use this for initialization
    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform == transform)
                {
                    GameObject.Instantiate(ragdollPrefab, hit.point, Random.rotation);
                    KickIt();
                }
            }
        }
    }

    private void KickIt()
    {
        if (_alreadyKickedIt == false)
        {
            _audioSource.clip = audioClip;
            _audioSource.loop = true;
            _audioSource.Play();
            _alreadyKickedIt = true;
        }
    }
}


