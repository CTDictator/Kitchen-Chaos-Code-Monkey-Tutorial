using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Kitchen Object")]
public class KitchenObjectSO : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public string objectName;
}
