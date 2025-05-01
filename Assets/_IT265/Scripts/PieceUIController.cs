using UnityEngine;
using UnityEngine.UI;

public class PieceUIController : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI stats;
    [SerializeField] private Image fill;
    
    
    public void SetStats(string str)
    {
        stats.text = str;
    }

    public void SetLife(float p)
    {
        fill.fillAmount = p;
    }
}
