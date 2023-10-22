using UnityEngine;

[CreateAssetMenu(fileName = "Data",
    menuName = "MyData/RoleData", order = 1)]
public class RoleData : ScriptableObject
{
    public string Name;

    public Color color;

    public KeyCode left;
    public KeyCode right;
    public KeyCode up;
    public KeyCode down;
 
}
