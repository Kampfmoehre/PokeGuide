namespace PokeGuide.Core.Enum
{
    /// <summary>
    /// Indicates a state of Exp. Point Pass Power (Gen 5) or Exp Point O-Power (Gen 6)
    /// </summary>
    public enum ExpPower
    {
        /// <summary>
        /// No Pass Power or O-Power is active
        /// </summary>
        None,
        /// <summary>
        /// Exp. Point Power ↓↓↓ (Pass Power) is active
        /// </summary>
        NegativeStageThree,
        /// <summary>
        /// Exp. Point Power ↓↓ (Pass Power) is active
        /// </summary>
        NegativeStageTwo,
        /// <summary>
        /// Exp. Point Power ↓ (Pass Power) is active
        /// </summary>
        NegativeStageOne,
        /// <summary>
        /// Exp. Point Power ↑ (Pass Power) or Stage 1 (O-Power) is active
        /// </summary>
        PositiveStageOne,
        /// <summary>
        /// Exp. Point Power ↑↑ (Pass Power) or Stage 2 (O-Power) is active
        /// </summary>
        PositiveStageTwo,
        /// <summary>
        /// Exp. Point Power ↑↑↑ (Pass Power), Stage 3, S or MAX (O-Power) is active
        /// </summary>
        PositiveStageThree
    }
}
