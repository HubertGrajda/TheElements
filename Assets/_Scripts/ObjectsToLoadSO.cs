using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//[CreateAssetMenu(menuName = "ManagersToLoad", fileName ="ManagersToLoad")]
public class ObjectsToLoadSO : ScriptableObject
{
    [SerializeField] private List<GameObject> managersList;
    [SerializeField] private List<View> viewsList;
    public List<GameObject> ManagersList => managersList;
    public List<View> ViewsList => viewsList;
}
