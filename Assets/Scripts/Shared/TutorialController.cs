using System;
using System.Collections;
using System.Collections.Generic;
using Layer;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameObject _tutorial;
    
    private void Start()
    {
        _tutorial.SetActive(!PlayerPrefs.HasKey("Tutorial"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    var colliderGameObject = hit.collider.gameObject;

                    if (colliderGameObject.layer == LayerHelper.GetLayer(Layers.Button))
                    {
                        _tutorial.SetActive(false);
                        PlayerPrefs.SetString("Tutorial", "Player");
                        PlayerPrefs.Save();
                    }
                }
            }
        }
    }
}
