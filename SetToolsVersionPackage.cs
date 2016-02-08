﻿using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;

namespace SetToolsVersion
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [Guid(SetToolsVersionPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class SetToolsVersionPackage: Package, IVsUpdateSolutionEvents2
    {
        private uint _updateSolutionEventsCookie;
        private IVsSolutionBuildManager2 _solutionBuildManager = null;

        /// <summary>
        /// SetToolsVersionPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "d310fc35-db47-4b95-8ea0-7f370585afff";

        /// <summary>
        /// Initializes a new instance of the <see cref="SetToolsVersionPackage"/> class.
        /// </summary>
        public SetToolsVersionPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            _solutionBuildManager =
                ServiceProvider.GlobalProvider.GetService(
                    typeof (SVsSolutionBuildManager)) as
                    IVsSolutionBuildManager2;
            Throw.IfNull(_solutionBuildManager, "_solutionBuildManager");
            ErrorHandler.ThrowOnFailure(
                _solutionBuildManager.AdviseUpdateSolutionEvents(this,
                    out _updateSolutionEventsCookie));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_solutionBuildManager != null &&
                _updateSolutionEventsCookie != 0)
            {
                _solutionBuildManager.UnadviseUpdateSolutionEvents(
                    _updateSolutionEventsCookie);
            }
        }

        public int UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        public int UpdateSolution_Done(int fSucceeded,
            int fModified, int fCancelCommand)
        {
            return VSConstants.S_OK;
        }

        public int UpdateSolution_StartUpdate(
            ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        public int UpdateSolution_Cancel()
        {
            return VSConstants.S_OK;
        }

        public int OnActiveProjectCfgChange(
            IVsHierarchy pIVsHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int UpdateProjectCfg_Begin(
            IVsHierarchy pHierProj, IVsCfg pCfgProj,
            IVsCfg pCfgSln, uint dwAction, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int UpdateProjectCfg_Done(
            IVsHierarchy pHierProj, IVsCfg pCfgProj,
            IVsCfg pCfgSln, uint dwAction, int fSuccess, int fCancel)
        {
            return VSConstants.S_OK;
        }
    }
}