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

        private void FixedUpdate()
        {
            _ecsController.FixedUpdate();
        }
    }
}