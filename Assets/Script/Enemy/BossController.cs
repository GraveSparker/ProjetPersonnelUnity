using UnityEngine;

public class BossController : MonoBehaviour
{
    private bool hasWon = false;

    public void OnBossDefeated()
    {
        if (hasWon)
            return;

        hasWon = true;
        GameManager.Instance.WinGame();
    }
}
