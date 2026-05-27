using UnityEngine;
using VDFramework;

namespace VDPackages.UtilityPackage.Utility.ApplicationUtil
{
    public class FPSLimiter : BetterMonoBehaviour
    {
        [SerializeField]
        private int fpsLimit = -1;
        
        private void Awake()
        {
            Application.targetFrameRate = fpsLimit;
        }
    }
}
