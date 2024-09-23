using UnityEngine;

namespace BRDQ
{
    public static class BRDQStaticUtil
    {
        public static bool BRDQIsGameOver;
        public static bool BRDQIsPause;

        public static bool BRDQStopUpdate => BRDQIsGameOver || BRDQIsPause;

        public static string BRDQToTimeString(this float val)
        {
            var minutes = Mathf.FloorToInt(val / 60);
            var seconds = Mathf.FloorToInt(val % 60);
        
            return $"{minutes:00}:{seconds:00}";
        }
        
        public static void BRDQEnableCanvas(this CanvasGroup cg, bool val)
        {
            cg.alpha = val ? 1f : 0f;
            cg.interactable = val;
            cg.blocksRaycasts = val;
        }
    }
}