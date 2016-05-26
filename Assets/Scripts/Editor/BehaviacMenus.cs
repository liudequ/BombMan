/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Tencent is pleased to support the open source community by making behaviac available.
//
// Copyright (C) 2015 THL A29 Limited, a Tencent company. All rights reserved.
//
// Licensed under the BSD 3-Clause License (the "License"); you may not use this file except in compliance with
// the License. You may obtain a copy of the License at http://opensource.org/licenses/BSD-3-Clause
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is
// distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class BehaviacMenus
{

    private static string WorkspaceExportPath
    {
        get
        {
            string relativePath = "/Resources/behaviac/exported";
            string path = "";
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                path = Application.dataPath + relativePath;
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                path = Application.dataPath + relativePath;
            }
            else
            {
                path = "Assets" + relativePath;
            }

            return path;
        }
    }


    [MenuItem("Behaviac/Export Meta")]
    static void CreateBTMetaFile()
    {

        //< write log file
        behaviac.Config.IsLogging = true;
        //behaviac.Config.IsSocketing = false;

        behaviac.Workspace.Instance.FilePath = WorkspaceExportPath;
        //behaviac.Workspace.Instance.FileFormat = behaviac.Workspace.EFileFormat.EFF_cs;
        behaviac.Workspace.Instance.FileFormat = behaviac.Workspace.EFileFormat.EFF_xml;


		behaviac.Agent.RegisterInstanceName<TestNodeAgent> ("TestNode");

		behaviac.Workspace.Instance.ExportMetas("behaviac/workspace/xmlmeta/BombMan.xml");
		behaviac.Workspace.Instance.Dispose();
    }

	[MenuItem("Behaviac/Export Behaviac Package")]
	static void ExportBehaviac()
	{
		//string[] assets = new string[1] {"Assets/Scripts/behaviac/"};
		//AssetDatabase.ExportPackage (assets, "..\\behaviac22.unitypackage", ExportPackageOptions.Recurse | ExportPackageOptions.Interactive);
		AssetDatabase.ExportPackage ("Assets/Scripts/behaviac", "..\\" +
		                                                        "behaviac.unitypackage", ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
	}


    [MenuItem("Behaviac/Launch Designer")]
    static void LaunchDesigner()
    {
        string btName = "Test";
        string workspacePath = System.IO.Path.Combine(Application.dataPath, "behaviac/workspace/BombMan.workspace.xml");
        string workspacePath2 = workspacePath.Replace("/", "\\");
        string stdWorkspacePath = string.Format("\"{0}\" /bt={1}", workspacePath2, btName);

        Debug.Log(stdWorkspacePath);

        //string[] outputs = new string[2];

        {

            string appPath = "C:\\GitLocation\\behaviac\\tools\\";

            string behaviacDesignerPath = System.IO.Path.Combine(appPath,"designer\\out\\BehaviacDesigner.exe");

            //outputs[0] = behaviacDesignerPath;

        
#if !UNITY_WEBPLAYER
            System.Diagnostics.Process.Start(behaviacDesignerPath, stdWorkspacePath);
#endif
        }
    }
}
