namespace CodeBase.Sound
{
    public enum SelectionState
    {
        /// <summary>
        /// The UI object can be selected.
        /// </summary>
        Normal,

        /// <summary>
        /// The UI object is highlighted.
        /// </summary>
        Highlighted,

        /// <summary>
        /// The UI object is pressed.
        /// </summary>
        Pressed,

        /// <summary>
        /// The UI object is selected
        /// </summary>
        Selected,

        /// <summary>
        /// The UI object cannot be selected.
        /// </summary>
        Disabled,

        /// <summary>
        /// For Dropdown only
        /// </summary>
        Shown,

        /// <summary>
        /// For Dropdown only
        /// </summary>
        Hidden
    }
}