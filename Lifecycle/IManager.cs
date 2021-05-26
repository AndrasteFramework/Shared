namespace Andraste.Shared.Lifecycle
{
    /// <summary>
    /// A manager is a lifecycle unit within Andraste.
    /// Every service should be a manager, so it can properly be loaded,
    /// unloaded, enabled and disabled.
    /// In addition to that, every manager will be registered centrally, so
    /// every Manager can be queried from a container.
    /// 
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// Setting this property should be relatively lightweight, as
        /// the majority of the (de-) initialization should happen in
        /// <see cref="Load"/> and <see cref="Unload"/>.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// This property should be used to prevent falsely calling
        /// <see cref="Load"/> or <see cref="Unload"/> multiple times
        /// introducing bugs or leaks
        /// </summary>
        bool Loaded { get; }

        /// <summary>
        /// Load this manager, performing all needed heavy tasks.
        /// Faster tasks to enable and disable the functionality,
        /// such as some Hooks, should be done in <see cref="Enabled"/>
        /// </summary>
        void Load();

        /// <summary>
        /// Unloads this manager, performing all needed heavy tasks.
        /// Faster tasks to enable and disable the functionality,
        /// such as some Hooks, should be done in <see cref="Enabled"/>
        /// </summary>
        void Unload();
    }
}
