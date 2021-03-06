using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTemplate : MonoBehaviour
{
    private const float debugLineHeight = 10.0f;
    public int damagerecharge;

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position + Vector3.up * debugLineHeight / 2, transform.position + Vector3.down * debugLineHeight / 2, Color.green);
    }

    void start()
    {
    }

    void OnTriggerEnter2D(Collider2D colider)
    {
        if (colider.gameObject.tag == "Player")
        {
            if (CharacterMoveController.Instance.energy < 100)
            {
                //value -4 kalo bomb dan +6 jika energi boost
                CharacterMoveController.Instance.energy += damagerecharge;
            }
            Destroy(this.gameObject);
        }

    }
}
