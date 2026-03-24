using UnityEngine;
using TMPro;

public class GachaSystem : MonoBehaviour
{
    public string[] rewards = { "threestar", "fourstar", "fivestar" };
    public int luck = 0;
    public TextMeshProUGUI RewardText;

    public string RollGacha()
    {
        int threestarChance = 70 - luck;
        int fourstarChance = 25 + (luck / 2);
        int fivestarChance = 5 + (luck / 2);

        int roll = Random.Range(1, 101);

        if (roll <= fivestarChance)
            return rewards[2];
        else if (roll <= fivestarChance + fourstarChance)
            return rewards[1];
        else
            return rewards[0];
    }

    private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Gatcha"))
    {
        string result = RollGacha();
        Debug.Log("You got: " + result);
        if (RewardText != null)
        RewardText.text = "You got: " + result;
    }
}