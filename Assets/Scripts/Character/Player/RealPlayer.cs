using UnityEngine;

public class RealPlayer : Player
{
    protected override void UpdateStuff()
    {
        if (waitingToFinishAttackPart) return;

        blocking = Input.GetMouseButton(0) && Input.GetMouseButton(1);

        if (blocking) return;

        if (Input.GetMouseButtonUp(0))
        {
            HeavyHit();
        }
        else if (Input.GetMouseButtonUp(1))
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
