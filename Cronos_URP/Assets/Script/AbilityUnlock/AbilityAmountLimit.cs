using System.Drawing;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AbilityAmountLimit : MonoBehaviour
{
    [SerializeField] public TMP_Text availableText;

    public int available;
    public int totalSpent;

    private Player _player;

    public void Awake()
    {
        _player = Player.Instance;


        UpdatePlayerTimePoint();

        Render();
    }

    public int CanSpend(int point)
    {
        if (available <= 0)
            return -1;

        int result = available - point;

        if (result < 0)
        {
            return -1;
        }

        return result;
    }

    public int GetAvailable() => available;

    private void Render()
    {
        availableText.text = available.ToString();
    }

    public void UpdatePlayerTimePoint()
    {
        if (_player != null)
        {
            available = (int)_player.TP;
        }

        Render();
    }

    public void UpdateSpent(int point)
    {
        int canSpend = CanSpend(point);

        if (canSpend != -1)
        {
            available = canSpend;
            totalSpent += point;
        }

        if (_player != null)
        {
            _player.TP = (float)available;
        }

        Render();
    }
}
