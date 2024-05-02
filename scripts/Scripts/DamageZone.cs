using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D other)
    {
        // sækir rubycontroller script
        RubyController controller = other.GetComponent<RubyController>();

        // tekur heilsuna í burtu þegar Ruby rekst á game object
        if (controller != null )
        {
            controller.ChangeHealth(-1);
        }


    }

}
