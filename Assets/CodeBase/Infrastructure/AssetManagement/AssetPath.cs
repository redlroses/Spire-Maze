namespace CodeBase.Infrastructure.AssetManagement
{
    public static class AssetPath
    {
        public const char DirectorySeparator = '/';

        public const string UIRoot = "Prefabs/UI/UIRoot";
        public const string Hud = "Prefabs/UI/Hud";
        public const string ExtraLiveView = "Prefabs/UI/ExtraLiveView";
        public const string TopRankView = "Prefabs/UI/Ranking/List_Medal";
        public const string RankView = "Prefabs/UI/Ranking/List_Variant";
        public const string CellView = "Prefabs/UI/InventorySlot";
        public const string CompassArrowPanel = "Prefabs/UI/CompassArrowPanel";
        public const string OverviewInterface = "Prefabs/UI/OverviewInterface";
        public const string EnterLevelPanel = "Prefabs/UI/EnterLevelPanel";
        public const string LevelNamePanel = "Prefabs/UI/LevelNamePanel";

        public const string EditorRewardADPanel = "Prefabs/UI/DebugPanels/EditorRewardADPanel";
        public const string EditorInterstitialADPanel = "Prefabs/UI/DebugPanels/EditorInterstitialADPanel";

        public const string Hero = "Prefabs/Hero";
        public const string VirtualMover = "Prefabs/VirtualMover";
        public const string Cells = "Prefabs/Cells";
        public const string Lobby = "Prefabs/Lobby";

        public const string Materials = "Materials";
        public const string PhysicMaterials = "Materials/Physics";
        public const string SpireMaterial = "Spire";
        public const string GroundMaterial = "Ground";

        public const string Avatar = "Sprite/Avatar";
        public const string Flag = "Sprite/Flag";

        public const string AudioMixer = "Audio/MainMixer";

        public const string SpireSegment = "Prefabs/SpireSegment";

        public const string HorizontalRail = "Prefabs/HorizontalRail";
        public const string VerticalRail = "Prefabs/VerticalRail";
        public const string HorizontalRailLock = "Prefabs/HorizontalRailLock";
        public const string VerticalRailLock = "Prefabs/VerticalRailLock";

        public const string TutorialTrigger = "Prefabs/TutorialTrigger";
        public const string TutorialPanel = "Prefabs/UI/Tutorial/TutorialPanel";

        public const string MusicPlayer = "Prefabs/MusicPlayer";

        public const string BuildInfo = "BuildInfo";
        public const string Camera = "Prefabs/Camera";

        public static string Combine(string folder, string name) =>
            $"{folder}{DirectorySeparator}{name}";
    }
}