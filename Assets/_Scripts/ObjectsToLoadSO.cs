using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "ManagersToLoad", fileName ="ManagersToLoad")]
public class ObjectsToLoadSO : ScriptableObject
{
    [SerializeField] private List<GameObject> managersList;
    public List<GameObject> ManagersList => managersList;
}
