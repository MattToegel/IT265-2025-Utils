using Demo;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /**
     * TODO 04/30
     * Game Event Log
     * Player UI
     * Cards and basic interactions
     * Indicators of action results
     */
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var keyValuePair in Tile.allTiles)
        {
            Debug.Log($"Post-processing tile {keyValuePair.Value.name}");
            keyValuePair.Value.onTileTriggered.AddListener((type) =>
            {
                Debug.Log("Checking tile trigger");
                if (type == Tile.TileType.POSITIVE)
                {
                    var currentPlayer = PlayerManager.Instance.CurrentPlayer();
                    int gems = Random.Range(0, 4);
                    if (gems > 0)
                    {
                        
                        currentPlayer.ChangeGems(gems);
                        Debug.Log($"{currentPlayer.name} gained {gems} gem(s)");
                        EventLog.Instance.Log($"{currentPlayer.name} gained {gems} gem(s)");
                        if (currentPlayer.gems >= 10)
                        {
                            Debug.Log($"{currentPlayer.name} wins!");
                            EventLog.Instance.Log($"{currentPlayer.name} wins!");
                            UIManager.Instance.ShowWinner($"{currentPlayer.name} wins with {currentPlayer.gems} gems!");
                        }
                    }
                    else {
                        Debug.Log($"{currentPlayer.name} didn't gain any gems");
                        EventLog.Instance.Log($"{currentPlayer.name} didn't gain any gems");
                    }
                }
                else if (type == Tile.TileType.NEGATIVE)
                {
                    var currentPlayer = PlayerManager.Instance.CurrentPlayer();
                    if (currentPlayer.gems > 0) {
                        currentPlayer.ChangeGems(-1);
                        Debug.Log($"{currentPlayer.name} lost a gem");
                        EventLog.Instance.Log($"{currentPlayer.name} lost a gem");
                    }

                }
            });
        }
    }
    public void PlayAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
