using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public StateManager stateManager;
    public GameObject objectPrefab1;
    public GameObject objectPrefab2;
    public GameObject objectPrefab3;
    public int selectedInteger = 0;
    private float timeSinceLastGenerate = 0f;
    private float generateInterval = 3f;
    private bool logicActive = false;

    void Update()
    {
        if (logicActive);

            timeSinceLastGenerate += Time.deltaTime;

            if (timeSinceLastGenerate >= generateInterval)
            {
                GenerateObject();
                timeSinceLastGenerate = 0f;
                selectedInteger -= 1;

                if (selectedInteger <= 0)
                {
                    stateManager.GameOver();
                }
            }
    }

    private void Start()
    {
        if (PlayerManager.Score > 0)
            selectedInteger = PlayerManager.Score;
        else
            selectedInteger = 50;
    }
    void GenerateObject()
    {
        Vector3 playerPosition = transform.position;
        Vector3 spawnPosition = playerPosition - transform.forward * 2f + transform.up * 1.5f;

        GameObject selectedObject = GetRandomObject();
        GameObject instantiatedObject = Instantiate(selectedObject, spawnPosition, Quaternion.identity);

        // Ignore collision between the player and generated objects
        Physics.IgnoreCollision(GetComponent<Collider>(), instantiatedObject.GetComponent<Collider>());
    }

    GameObject GetRandomObject()
    {
        int randomIndex = Random.Range(1, 4);

        switch (randomIndex)
        {
            case 1:
                return objectPrefab1;
            case 2:
                return objectPrefab2;
            case 3:
                return objectPrefab3;
            default:
                Debug.LogError("Invalid random index");
                return null;
        }
    }
}
