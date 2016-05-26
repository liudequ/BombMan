using System.Collections;
using behaviac;
using UnityEngine;
using Debug = UnityEngine.Debug;

[behaviac.TypeMetaInfo()]
public class TestNodeAgent : behaviac.Agent
{
    [behaviac.MemberMetaInfo()]
    public int testVar_0 = -1;

    [behaviac.MemberMetaInfo("testVar_1","testVar_1 property",100)]
    public int testVar_1 = -1;

    [behaviac.MemberMetaInfo()]
    public float testVar_2 = -1.0f;

    public bool eventVar = false;

    public bool isInstance = false;

    [behaviac.MethodMetaInfo()]
    public void setEventVarInt(int var)
    {
        testVar_0 = var;
        Debug.Log(testVar_0);
    }

    [behaviac.MethodMetaInfo()]
    public void setEventVarBool(bool var)
    {
        this.eventVar = var;
        Debug.Log(this.eventVar);
    }

    public void Awake()
    {
        behaviac.Config.IsLogging = true;

        behaviac.Workspace.Instance.FilePath = Application.dataPath+"/Resources/behaviac/export";
        behaviac.Workspace.Instance.FileFormat = Workspace.EFileFormat.EFF_xml;

        if (this.isInstance)
        {
            behaviac.Agent.RegisterInstanceName<TestNodeAgent>("TestNode");
            behaviac.Agent.BindInstance(this, "TestNode");
        }

        bool bo = this.btload("Test");
        if (bo)
        {
            this.btsetcurrent("Test");
        }
    }

    public void Update()
    {
        base.btexec();
    }



    public void OnDestroy()
    {
        behaviac.Workspace.Instance.Cleanup();
    }


}
