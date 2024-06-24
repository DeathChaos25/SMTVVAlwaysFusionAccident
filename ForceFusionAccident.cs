using Reloaded.Hooks.Definitions;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using SMTVVAlwaysFusionAccident.Template;
using System.Diagnostics;
using System.Drawing;

namespace SMTVVAlwaysFusionAccident
{
    public class ForceFusionAccident
    {
        private delegate bool WillFusionResultInAccidentDelegate(int InSourceDevil1, int InSourceDevil2);

        private IHook<WillFusionResultInAccidentDelegate> _willFusionResultInAccidentHook;
        private string WillFusionResultInAccident_SIG = "48 89 5C 24 ?? 48 89 6C 24 ?? 41 56 48 83 EC 20 48 8B 1D ?? ?? ?? ??";

        private static IReloadedHooks _hooks = null!;
        private IStartupScanner _scanner;
        private ILogger _logger;

        internal unsafe ForceFusionAccident(ModContext _context)
        {
            var _hooks = _context.Hooks;
            _logger = _context.Logger;

            var startupScannerController = _context.ModLoader.GetController<IStartupScanner>();
            if (startupScannerController == null || !startupScannerController.TryGetTarget(out _scanner))
            {
                _logger.WriteLineAsync($"[smtvv.patch.alwaysfusionaccident]: Unable to get controller for Reloaded SigScan Library", Color.Red);
                return;
            }

            var BaseAddress = Process.GetCurrentProcess().MainModule.BaseAddress.ToInt64();

            _scanner.AddMainModuleScan(WillFusionResultInAccident_SIG, result =>
            {
                if (!result.Found)
                {
                    _logger.WriteLineAsync("[smtvv.patch.alwaysfusionaccident]: Couldn't find location for WillFusionResultInAccident, mod will not work", Color.Red);
                    return;
                }

                var addr = BaseAddress + result.Offset;

                _willFusionResultInAccidentHook = _hooks.CreateHook<WillFusionResultInAccidentDelegate>((a1, a2) =>
                {
                    bool result = _willFusionResultInAccidentHook.OriginalFunction(a1, a2);

                    return true;
                }, addr).Activate();
            });
        }
    }
}
