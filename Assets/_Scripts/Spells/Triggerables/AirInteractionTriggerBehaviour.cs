using UnityEngine;

namespace _Scripts.Spells
{
    [CreateAssetMenu(menuName = "Spells/TriggerBehaviour/AirInteraction", fileName = "New Air Interaction Trigger")]
    public class AirInteractionTriggerBehaviour : TriggerBehaviour<IAirInteractable>
    {
        protected override void OnTriggerableEnter(IAirInteractable airInteractable, BehaviourTrigger _)
        {
            airInteractable.OnInteractionStart();
        }

        protected override void OnTriggerableStay(IAirInteractable airInteractable, BehaviourTrigger _)
        {
            airInteractable.OnInteractionStart();
        }
    }
}