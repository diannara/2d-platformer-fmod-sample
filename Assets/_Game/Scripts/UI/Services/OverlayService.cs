using System;
using System.Collections.Generic;

using UnityEngine;

using TIGD.Services;
using TIGD.UI.Overlays;

namespace TIGD.UI.Services
{
    public class OverlayService : AbstractService
    {
        // This dictionary is used to store the overlays registered with the overlay service
        private Dictionary<Type, IOverlay> _overlays = new Dictionary<Type, IOverlay>();

        // This method is used to get an overlay of a specific type
        public T Get<T>() where T : IOverlay
        {
            Type type = typeof(T);
            if(!_overlays.ContainsKey(type))
            {
                Debug.LogWarning($"OverlayService :: Get() :: An overlay of type {type} was not previously registered!");
                return default;
            }

            return (T)_overlays[type];
        }

        // This method is used to register an overlay with the overlay service
        public void Register(IOverlay overlay)
        {
            Type type = overlay.GetType();
            if (_overlays.ContainsKey(type))
            {
                Debug.LogWarning($"OverlayService :: Register() :: Overlay of type {type} was already registered!");
                return;
            }

            _overlays.Add(type, overlay);
            overlay.Hide();
        }

        // This method returns a boolean value indicating whether the overlay was found and returned
        public bool TryGet<T>(out T overlay) where T : IOverlay
        {
            Type type = typeof(T);
            if(!_overlays.ContainsKey(type))
            {
                Debug.LogWarning($"OverlayService :: TryGet() :: An overlay of type {type} was not previously registered!");
                overlay = default;
                return false;
            }

            overlay = (T)_overlays[type];
            return true;
        }
        
        // This method returns a boolean value indicating whether the overlay was successfully hidden
        public bool TryHide<T>() where T : IOverlay
        {
            if(TryGet(out T overlay))
            {
                overlay.Hide();
                return true;
            }
            return false;
        }
        
        // This method returns a boolean value indicating whether the overlay was successfully shown
        public bool TryShow<T>() where T : IOverlay
        {
            if(TryGet(out T overlay))
            {
                overlay.Show();
                return true;
            }
            return false;
        }

        // This method is used to unregister an overlay from the overlay service
        public void Unregister(IOverlay overlay)
        {
            Type type = overlay.GetType();
            if (!_overlays.ContainsKey(type))
            {
                Debug.LogWarning($"OverlayService :: Unregister() :: Overlay of type {type} was not previously registered!");
                return;
            }
            _overlays.Remove(type);
        }
    }
}
