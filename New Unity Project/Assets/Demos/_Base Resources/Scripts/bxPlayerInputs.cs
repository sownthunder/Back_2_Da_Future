using UnityEngine;

namespace Blartenix.Demos
{
    [RequireComponent(typeof(bxEllenController))]
    public class bxPlayerInputs : MonoBehaviour
    {
        private bxEllenController ellenController = null;

        private void Awake()
        {
            ellenController = GetComponent<bxEllenController>();
        }


        private void Update()
        {
            ellenController.Move(Input.GetAxisRaw("Horizontal"));

            if (Input.GetKeyDown(KeyCode.Space))
                ellenController.Jump();
        }
    }
}