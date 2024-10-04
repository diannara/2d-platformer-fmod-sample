using UnityEngine;

using TIGD.Services;
using TIGD.UI.Services;

namespace TIGD.UI.Overlays
{
    public abstract class AbstractOverlay : MonoBehaviour, IOverlay
    {
        protected virtual void OnDestroy()
        {
            if(ServiceLocator.TryGet(out OverlayService overlayService))
            {
                overlayService.Unregister(this);
            }
        }

        public abstract void Hide();


        public abstract void Show();

        protected virtual void Start()
        {
            if(ServiceLocator.TryGet(out OverlayService overlayService))
            {
                overlayService.Register(this);
            }
        }
    }
}
