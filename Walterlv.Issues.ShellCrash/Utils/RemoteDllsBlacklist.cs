using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Walterlv.Issues.ShellCrash.Utils
{
    internal static class RemoteDllsBlacklist
    {
        private static readonly HashSet<string> _blacklist = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "gamebox_shell.dll",
            "dopuslib32.dll",
            "ListaryHook.dll",
        };

        private static readonly HashSet<string> _trustDirectorylist = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            @"C:\WINDOWS",
            AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
        };

        /// <summary>
        /// 找到所有模块中可能不受信任的模块（非系统目录，非 EN 目录）。
        /// </summary>
        /// <param name="modules">模块列表。</param>
        /// <returns>可能不受信任的模块（非系统目录，非 EN 目录）。</returns>
        internal static IEnumerable<ProcessModule> FindOutNotTrustfulModules(this ProcessModuleCollection modules)
        {
            return modules.OfType<ProcessModule>().Where(x => _trustDirectorylist.All(
                t => !x.FileName.StartsWith(t, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// 找到所有模块中已明确在黑名单中的模块。
        /// </summary>
        /// <param name="modules">模块列表。</param>
        /// <returns>已明确在黑名单中的模块。</returns>
        internal static IEnumerable<ProcessModule> FindOutBlacklistModules(this ProcessModuleCollection modules)
        {
            return modules.OfType<ProcessModule>().FindOutBlacklistModules();
        }

        /// <summary>
        /// 找到指定的模块序列中已明确在黑名单中的模块。
        /// </summary>
        /// <param name="modules">模块列表。</param>
        /// <returns>已明确在黑名单中的模块。</returns>
        internal static IEnumerable<ProcessModule> FindOutBlacklistModules(this IEnumerable<ProcessModule> modules)
        {
            return modules.OfType<ProcessModule>().Where(x =>
                _blacklist.Contains(Path.GetFileName(x.FileName), StringComparer.OrdinalIgnoreCase));
        }
    }
}
