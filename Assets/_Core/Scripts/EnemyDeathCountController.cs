using UnityEngine;

public class EnemyDeathCountController : MonoBehaviour
{
    private void Start()
    {
        GetComponent<HealthBehaviour>().OnDeath.AddListener(FindFirstObjectByType<HUDController>().EnemyKilled);
    }
}
