using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andraste.Shared.Lifecycle
{
    #nullable enable
    public class ManagerContainer : IManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected readonly Dictionary<Type, IManager> Managers;

        public ManagerContainer()
        {
            Managers = new Dictionary<Type, IManager>();
        }

        public bool Enabled
        {
            get => true;
            set => throw new InvalidOperationException("Cannot disable ManagerContainer");
        }

        public bool Loaded { get; private set; }

        public void Load()
        {
            foreach (var manager in Managers.Values.Where(manager => !manager.Loaded))
            {
                try
                {
                    manager.Load();
                    manager.Enabled = true;
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex, "Exception when Loading or Enabling a Manager");
                }
            }

            Loaded = true;
        }

        public void Unload()
        {
            foreach (var manager in Managers.Values.Where(manager => manager.Loaded))
            {
                try
                {
                    manager.Enabled = false;
                    manager.Unload();
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex, "Exception when Unloading or Disabling a Manager");
                }
            }

            Managers.Clear();
            Loaded = false;
        }

        public T RegisterManager<T>(T instance) where T: IManager
        {
            Managers.Add(instance.GetType(), instance);
            return instance;
        }

        public T RegisterManagerAndLoad<T>(T instance) where T: IManager
        {
            RegisterManager(instance);
            if (instance.Loaded)
            {
                return instance;
            }

            try
            {
                instance.Load();
                instance.Enabled = true;
            }
            catch (Exception ex)
            {
                Logger.Warn(ex, "Exception when Loading or Enabling a Manager");
            }

            return instance;
        }

        public IManager? GetManager(Type type)
        {
            return Managers.ContainsKey(type) ? Managers[type] : null;
        }

        // Unfortunately, we cannot bind T to extend IManager here.
        public T? GetManager<T>() where T : class
        {
            return GetManager(typeof(T)) as T;
        }

        public T GetManagerOrThrow<T>() where T : class
        {
            var result = GetManager<T>();
            if (result != null)
            {
                return result;
            }
            
            throw new KeyNotFoundException("This ManagerContainer has no registered manager of type " + typeof(T));
        }
    }
    #nullable restore
}
