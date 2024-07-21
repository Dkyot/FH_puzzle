using FH.Cards;
using FH.SO;
using UnityEngine;
using UnityEngine.Serialization;

namespace FH.Level
{
    public class LevelTrainingController : MonoBehaviour
    {
        [SerializeField] private GameContext gameContext;
        
        [SerializeField, Space] private CardManager cardManager;
        [FormerlySerializedAs("tipTrainingController")] [SerializeField] private TipsTrainingController tipsTrainingController;
        [SerializeField] private FlipCardTrainingController flipCardTrainingController;

        public async Awaitable StartTraining()
        {
            switch (gameContext.CurrentLevel.number)
            {
                case 1:
                    cardManager.FindPair();
                    break;
                case 2:
                    tipsTrainingController.ShowTipsPointer();
                    break;
                case 3:
                    await flipCardTrainingController.EnableTipAsync();
                    break;
            }
            
            if (gameContext.CurrentLevel.Params.LevelType is LevelType.AllEquals or LevelType.ThreePairs)
            {
                await cardManager.WaveTip();
            }
        }

        public void StopTraining()
        {
            flipCardTrainingController.DisableTip();
        }
    }
}