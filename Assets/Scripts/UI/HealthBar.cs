using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class HealthBar : UIBar
    {
        public override ResourceType GetResource()
        {
            return ResourceType.HEALTH;
        }
    }
}
