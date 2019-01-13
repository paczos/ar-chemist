namespace GoogleARCoreInternal
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UnityEditor;
    using UnityEditor.Build;
    using UnityEngine;

    internal class ExamplePreprocessBuild : ExampleBuildHelper
    {
        public ExamplePreprocessBuild()
        {
            _AddExampleScene(new ExampleScene()
                {
                    ProductName = "HelloAR U3D",
                    PackageName = "com.google.ar.core.examples.unity.helloar",
                    SceneGuid = "e6a6fa04348cb45c9b0221eb19c946da",
                    IconGuid = "36b7440e71f344bef8fca770c2d365f8"
                });
            _AddExampleScene(new ExampleScene()
                {
                    ProductName = "CV U3D",
                    PackageName = "com.google.ar.core.archemist",
                    SceneGuid = "5ef0f7f7f2c7b4285b707265348bbffd",
                    IconGuid = "7c556c651080f499d9eaeea95d392d80"
                });
            _AddExampleScene(new ExampleScene()
                {
                    ProductName = "AR Chemist",
                    PackageName = "com.google.ar.core.examples.unity.augmentedimage",
                    SceneGuid = "be567d47d3ab94b3badc5b211f535a24",
                });
            _AddExampleScene(new ExampleScene()
                {
                    ProductName = "CloudAnchors U3D",
                    PackageName = "com.google.ar.core.examples.unity.cloudanchors",
                    SceneGuid = "83fb41cc294e74bdea57537befa00ffc",
                    IconGuid = "dcfb8b44c93d547e2bdf8a638c1415af"
                });
        }

        public override void OnPreprocessBuild(BuildTarget target, string path)
        {
            _DoPreprocessBuild(target, path);
        }
    }
}
