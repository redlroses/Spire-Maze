using CodeBase.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements.Buttons.TransitionButtons
{
    [RequireComponent(typeof(Button))]
    public class MenuButton : TransitionButton
    {
        protected override LoadPayload CreateTransitionPayload() =>
            new LoadPayload(LevelNames.Lobby, false, LevelNames.LobbyId, true, true);
    }
}