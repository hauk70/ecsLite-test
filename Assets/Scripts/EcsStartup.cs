using UnityEngine;

namespace EcsTest
{
    public class EcsStartup : MonoBehaviour
    {
        private EcsController _ecsController;

        void Start()
        {
            _ecsController = new EcsController();
        }

        void Update()
        {
            _ecsController.Update();
        }

        private void FixedUpdate()
        {
            _ecsController.FixedUpdate();
        }
    }
}