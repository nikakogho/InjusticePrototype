using UnityEngine;

public class TrainingPlayer : Player
{
    protected override void UpdateStuff()
    {
        if (waitingToFinishAttackPart) return;

        bool currentlyBlocking = Input.GetMouseButton(0) && Input.GetMouseButton(1);

        if(blocking != currentlyBlocking)
        {
            ToggleBlocking();
            return;
        }

        if (Input.GetMouseButtonUp(0) && !Input.GetMouseButton(1))
        {
            HeavyHit();
        }
        else if (Input.GetMouseButtonUp(1) && !Input.GetMouseButton(0))
        {
            NormalHit();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (power >= 1)
            {
                SpecialAttack(1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (power >= 2)
            {
                SpecialAttack(2);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (power >= 3)
            {
                SpecialAttack(3);
            }
        }
    }
}
