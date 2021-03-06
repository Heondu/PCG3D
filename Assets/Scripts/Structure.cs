using UnityEngine;

public class Structure : MonoBehaviour
{
    public static Structure instance;

    public int[,,] tree = 
    { 
        { 
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 4, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
        }, 
        { 
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 4, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
        },
        {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 4, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
        },
        {
            { 5, 5, 5, 5, 5 },
            { 5, 5, 5, 5, 5 },
            { 5, 5, 4, 5, 5 },
            { 5, 5, 5, 5, 5 },
            { 5, 5, 5, 5, 5 },
        },
        {
            { 5, 5, 5, 5, 5 },
            { 5, 5, 5, 5, 5 },
            { 5, 5, 4, 5, 5 },
            { 5, 5, 5, 5, 5 },
            { 5, 5, 5, 5, 5 },
        },
        {
            { 0, 0, 0, 0, 0 },
            { 0, 5, 5, 5, 0 },
            { 0, 5, 4, 5, 0 },
            { 0, 5, 5, 5, 0 },
            { 0, 0, 0, 0, 0 },
        },
        {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 5, 0, 0 },
            { 0, 5, 5, 5, 0 },
            { 0, 0, 5, 0, 0 },
            { 0, 0, 0, 0, 0 },
        }
    };

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
}
