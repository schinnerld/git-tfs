using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitTfs.Commands;

namespace GitTfs
{
    internal static class Workarounds
    {
        private static bool UseWorkarounds = true;

        public static bool StopOnFailMergeCommit(bool originalCode)
        {
            // originalCode: _fetch.BranchStrategy == BranchStrategy.All
            // problem: if not all branches are needed, e.g. by providing the -ignore-branches-regex
            // or like we do with another workaround by providing a whitelist of brances
            // an error caused by any other branch would result in an error here and stop the whole migration
            // workaround: stopOnFailMergeCommit = false
			
            if (!UseWorkarounds)
                return originalCode;

            return false;   
        }


        public static bool IsWhitelisted(string tfsPath)
        {
            // problem: cBTI contain all subbranches
            // the initialization needs a lot of time even if just a couple of branches should be used for migration
            // -ignore-branches-regex is unfortunately not used at all here
            // workaround: use whitelisted branches only

            if (!UseWorkarounds)
                return false;

            return IsIncludedBranch(tfsPath);

        }

        //(tfsBranch.Path.Contains("4.12.x") || 
        //tfsBranch.Path.Contains("6.0.1.x") || tfsBranch.Path.Contains("RDP-750") || 
        //tfsBranch.Path.Contains("AS60_ES") || tfsBranch.Path.Contains("RDP-667"))
        public static bool IsBlacklisted(bool originalCode, string tfsPath)
        {
            // problem: blacklisting just is supported with the -ignore-branches-regex
            // with a lot o branches (speeking of several hundreds) this can get complex
            // workaround: use whitelisted branches only

            if (!UseWorkarounds)
                return originalCode;

            return !IsIncludedBranch(tfsPath);
        }

        private static bool IsIncludedBranch(string tfsPath)
        {
            return tfsPath.Contains("4.12.x") || tfsPath.Contains("6.0.1.x") || tfsPath.Contains("RDP-750") ||
                   tfsPath.Contains("AS60_ES") || tfsPath.Contains("RDP-667");
        }
    }
}
