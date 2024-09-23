using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BRDQ
{
    public class BRDQLevelBtn : Button
    {
        [field: SerializeField] public TMP_Text _BRDQLevel { get; private set; }
    }
}