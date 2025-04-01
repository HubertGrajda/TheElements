using UnityEngine;

namespace _Scripts
{
    public interface IAirInteractable
    {
        public void OnInteractionStart();
        public void OnInteractionStay(GameObject trigger);
        public void OnInteractionEnd();
    }
}