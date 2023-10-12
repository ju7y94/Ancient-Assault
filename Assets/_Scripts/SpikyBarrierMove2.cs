using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
    public class SpikyBarrierMove2 : MonoBehaviour
    {
        [SerializeField] float barrierSpeed;
        [SerializeField] float barrierRange;
        [SerializeField] float offset;
        [SerializeField] Vector3 initialPosition;
        // Start is called before the first frame update
        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {
            SineMove();
        }

        void SineMove()
        {
            transform.position = initialPosition + transform.forward * Mathf.Sin(Time.time * barrierSpeed + offset) * barrierRange;
        }
    }
 

