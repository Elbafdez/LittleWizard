using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbyPoint : MonoBehaviour
{
    public Enemy occupiedBy = null; // El enemigo que ocupa este punto

    public bool IsOccupied()
    {
        return occupiedBy != null;
    }
}
