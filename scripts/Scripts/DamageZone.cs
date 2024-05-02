using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D other)
    {
        // s�kir rubycontroller script
        RubyController controller = other.GetComponent<RubyController>();

        // tekur heilsuna � burtu �egar Ruby rekst � game object
        if (controller != null )
        {
            controller.ChangeHealth(-1);
        }


    }

}
