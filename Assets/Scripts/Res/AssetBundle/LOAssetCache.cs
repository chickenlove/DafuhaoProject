// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;
using com.QH.QPGame.GameUtils;
using Object = UnityEngine.Object;

namespace LOAssetFramework
{
    internal sealed class LOAssetCache
    {
        #region 包裹缓存机制

        //创建缓存字典
        private Dictionary<string, LOAssetBundle> assetBundleCache;
        //缓存字典属性
        internal Dictionary<string, LOAssetBundle> BundleCache
        {
            get
            {
                if (assetBundleCache == null)
                {
                    assetBundleCache = new Dictionary<string, LOAssetBundle>();
                }

                return assetBundleCache;
            }
        }

        //创建缓存WWW对象
        private Dictionary<string, WWW> wwwCache;
        //创建缓存WWW对象属性
        internal Dictionary<string, WWW> WwwCache
        {
            get
            {
                if (wwwCache == null)
                {
                    wwwCache = new Dictionary<string, WWW>();
                }

                return wwwCache;
            }
        }

        //创建依赖缓存对象
        private Dictionary<string, string[]> dependCache;
        //创建依赖缓存属性
        internal Dictionary<string, string[]> DependCache
        {
            get
            {
                if (dependCache == null)
                {
                    dependCache = new Dictionary<string, string[]>();
                }
                return dependCache;
            }
        }

        private Dictionary<string, string> errorCache;

        internal Dictionary<string, string> ErrorCache
        {
            get
            {
                if (errorCache == null)
                {
                    errorCache = new Dictionary<string, string>();
                }
                return errorCache;
            }
        }


        /// <summary>
        /// Ins the cache.
        /// </summary>
        /// <returns><c>true</c>, if cache was ined, <c>false</c> otherwise.</returns>
        /// <param name="assetbundlename">Assetbundlename.</param>
        internal bool InCache(string assetbundlename)
        {
            return BundleCache.ContainsKey(assetbundlename);
        }

        internal bool InWWWCache(string assetbundlename)
        {
            return WwwCache.ContainsKey(assetbundlename);
        }

        #endregion


        #region 卸载系列函数

        /// <summary>
        /// 卸载资源包和依赖包
        /// </summary>
        /// <param name="assetBundleName">Asset bundle name.</param>
        public void UnloadAssetBundle(string assetBundleName)
        {
            UnloadAssetBundleInternal(assetBundleName);
            UnloadDependencies(assetBundleName);
        }

        internal void UnloadDependencies(string assetBundleName)
        {
            string[] dependencies = null;
            //获取所有的依赖包名称
            if (!DependCache.TryGetValue(assetBundleName, out dependencies))
                return;

            //卸载依赖包
            foreach (var dependency in dependencies)
            {
                UnloadAssetBundleInternal(dependency);
            }
            //删除依赖缓存策略
            DependCache.Remove(assetBundleName);
        }

        internal void UnloadAssetBundleInternal(string assetBundleName)
        {
            LOAssetBundle bundle;
            BundleCache.TryGetValue(assetBundleName, out bundle);

            if (bundle == null)
            {
                return;
            }

            if (bundle.Release(true))
            {
                FreeBundle(assetBundleName);
            }
        }

        #endregion

        #region Setter`Getter

        internal WWW GetWWWCache(string key)
        {
            WWW www;
            WwwCache.TryGetValue(key, out www);
            return www;
        }

        internal void SetWWWCache(string key, WWW value, bool save = false)
        {
            WwwCache.Add(key, value);
        }

        internal LOAssetBundle GetBundleCache(string key)
        {
            LOAssetBundle ab;

            BundleCache.TryGetValue(key, out ab);

            return ab;
        }

        internal void SetBundleCache(string key, LOAssetBundle value)
        {
            BundleCache.Add(key, value);
        }

        internal string[] GetDependCache(string key)
        {
            string[] depends;

            DependCache.TryGetValue(key, out depends);

            return depends;
        }

        internal void SetDependCache(string key, string[] value)
        {
            DependCache.Add(key, value);
        }

        internal string GetErrorCache(string key)
        {
            string error;

            ErrorCache.TryGetValue(key, out error);

            return error;
        }

        internal void SetErrorCache(string key, string value)
        {
            ErrorCache.Add(key, value);
        }

        #endregion

        internal void FreeBundle(string key)
        {
            Logger.Res.Log("FreeBundle~~~~~~~~~~~   key:" + key);
            BundleCache.Remove(key);
        }

        internal void FreeAll()
        {
            Logger.Res.Log("FreeAll~~~~~~~~~~~");
            foreach (var loAssetBundle in BundleCache)
            {
                loAssetBundle.Value.Bundle.Unload(true);
            }
            BundleCache.Clear();
            ErrorCache.Clear();
            DependCache.Clear();
            WwwCache.Clear();
        }
    }
}

