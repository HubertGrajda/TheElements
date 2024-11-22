using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(menuName = "Spells/Air Interaction Trigger", fileName = "New Air Interaction Trigger")]
    public class AirInteractionTriggerBehaviour : OnColliderTriggerBehaviour<IAirInteractable>
    {
        protected override void OnTriggerableEnter(IAirInteractable airInteractable)
        {
            airInteractable.OnInteractionStart();
        }

        protected override void OnTriggerableExit(IAirInteractable airInteractable)
        {
        }

        protected override void OnTriggerableStay(IAirInteractable airInteractable)
        {
            airInteractable.OnInteractionStart();
        }
    }
}