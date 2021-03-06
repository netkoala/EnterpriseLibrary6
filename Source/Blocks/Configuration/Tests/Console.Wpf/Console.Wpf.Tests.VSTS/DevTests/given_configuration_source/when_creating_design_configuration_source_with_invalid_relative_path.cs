﻿//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_source
{
    [TestClass]
    public class when_creating_design_configuration_source_with_invalid_relative_path : ArrangeActAssert
    {
        private FileConfigurationSourceElement configurationSourceElement;
        private DesignConfigurationSource mainConfigurationSource;
        private string expectedFilePath;
        private string mainFilePath;
            
        protected override void Arrange()
        {
            base.Arrange();

            var resourceHelper = new ResourceHelper<ConfigFiles.ConfigFileLocator>();
            mainFilePath = resourceHelper.DumpResourceFileToDisk("empty.config", "ds_inv_rel_path");
            mainConfigurationSource = new DesignConfigurationSource(mainFilePath);

            configurationSourceElement = new FileConfigurationSourceElement("relativefile", "doesnt-exist\\relative.config");

            var mainFileDirectory = Path.GetDirectoryName(mainFilePath);
            expectedFilePath = Path.Combine(mainFileDirectory, configurationSourceElement.FilePath);
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void then_creating_design_source_throws()
        {
            configurationSourceElement.CreateDesignSource(mainConfigurationSource);
        }

        protected override void Teardown()
        {
            RemoveFiles();
        }

        private void RemoveFiles()
        {
            if (File.Exists(expectedFilePath)) File.Delete(expectedFilePath);
            if (File.Exists(mainFilePath)) File.Delete(mainFilePath);
        }

    }
}
