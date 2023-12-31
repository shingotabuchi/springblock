using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
public struct ComputeNode
{
    public bool fix;
    public bool breakbond0;
    public int bond0;
    public bool breakbond1;
    public int bond1;
    public bool breakbond2;
    public int bond2;
    public bool breakbond3;
    public int bond3;
    public Vector2 position;
    public Vector2 positionTmp;
    public Vector2 velocity;
    public Vector2 velocityTmp;
}
public class SpringModelMain : MonoBehaviour
{
    public Image plotImage;
    public int DIM_X;
    public int DIM_Y;
    public int rowCount;
    public int columnCount;
    public int nodeInterval;
    public int loopCount;
    public float D, L, K;
    public float bondCrit;
    public float slipCrit;
    Texture2D plotTexture;
    RenderTexture renderTexture;
    ComputeBuffer nodes;
    int Init, Plot, RandomMovement, Erase, StepTmp, Step, BreakBonds;
    public ComputeShader compute;
    int[] threadsPlot;
    int[] threadsNodes;
    ComputeNode[] debugNodes;
    void OnDestroy()
    {
        nodes.Dispose();
    }
    private void OnValidate()
    {
        compute.SetFloat("D", D);
        compute.SetFloat("L", L);
        compute.SetFloat("K", K);
        compute.SetFloat("bondCrit", bondCrit);
        compute.SetFloat("slipCrit", slipCrit);
    }
    void Start()
    {
        if (nodeInterval * (rowCount - 1) >= DIM_Y || nodeInterval * (columnCount - 1) >= DIM_X)
        {
            print("bruh");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        threadsPlot = new int[3] { (DIM_X + 15) / 16, (DIM_Y + 15) / 16, 1 };
        threadsNodes = new int[3] { (columnCount + 15) / 16, (rowCount + 15) / 16, 1 };

        // threadsPlot = new int[3] { (DIM_X + 7) / 8, (DIM_Y + 7) / 8, 1 };
        // threadsNodes = new int[3] { (columnCount + 7) / 8, (rowCount + 7) / 8, 1 };


        plotTexture = new Texture2D(DIM_X, DIM_Y);
        plotTexture.filterMode = FilterMode.Point;
        plotImage.sprite = Sprite.Create(plotTexture, new Rect(0, 0, DIM_X, DIM_Y), UnityEngine.Vector2.zero);
        ((RectTransform)plotImage.transform).sizeDelta = new Vector2(DIM_X * 1080 / DIM_Y, 1080);
        renderTexture = new RenderTexture(DIM_X, DIM_Y, 24);
        renderTexture.enableRandomWrite = true;

        compute.SetInt("DIM_X", DIM_X);
        compute.SetInt("DIM_Y", DIM_Y);
        compute.SetInt("nodeInterval", nodeInterval);
        compute.SetInt("rowCount", rowCount);
        compute.SetInt("columnCount", columnCount);
        OnValidate();

        nodes = new ComputeBuffer(columnCount * rowCount, Marshal.SizeOf(typeof(ComputeNode)));
        debugNodes = new ComputeNode[columnCount * rowCount];


        Init = compute.FindKernel("Init");
        compute.SetBuffer(Init, "nodes", nodes);

        Plot = compute.FindKernel("Plot");
        compute.SetTexture(Plot, "renderTexture", renderTexture);
        compute.SetBuffer(Plot, "nodes", nodes);

        Erase = compute.FindKernel("Erase");
        compute.SetTexture(Erase, "renderTexture", renderTexture);

        RandomMovement = compute.FindKernel("RandomMovement");
        compute.SetBuffer(RandomMovement, "nodes", nodes);

        StepTmp = compute.FindKernel("StepTmp");
        compute.SetBuffer(StepTmp, "nodes", nodes);

        Step = compute.FindKernel("Step");
        compute.SetBuffer(Step, "nodes", nodes);

        BreakBonds = compute.FindKernel("BreakBonds");
        compute.SetBuffer(BreakBonds, "nodes", nodes);

        compute.Dispatch(Init, threadsNodes[0], threadsNodes[1], threadsNodes[2]);
        compute.Dispatch(Plot, threadsNodes[0], threadsNodes[1], threadsNodes[2]);
        RenderTexture.active = renderTexture;
        plotTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        plotTexture.Apply();

        // nodes.GetData(debugNodes);
        // foreach (var item in debugNodes)
        // {
        //     print(item.position);
        // }
    }
    private void Update()
    {
        for (int i = 0; i < loopCount; i++)
        {
            // compute.SetInt("offset", (int)Random.Range(0, int.MaxValue));
            // compute.Dispatch(RandomMovement, threadsNodes[0], threadsNodes[1], threadsNodes[2]);
            compute.Dispatch(StepTmp, threadsNodes[0], threadsNodes[1], threadsNodes[2]);
            compute.Dispatch(Step, threadsNodes[0], threadsNodes[1], threadsNodes[2]);
            compute.Dispatch(BreakBonds, threadsNodes[0], threadsNodes[1], threadsNodes[2]);
        }
        compute.Dispatch(Erase, threadsPlot[0], threadsPlot[1], threadsPlot[2]);
        compute.Dispatch(Plot, threadsNodes[0], threadsNodes[1], threadsNodes[2]);
        RenderTexture.active = renderTexture;
        plotTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        plotTexture.Apply();
    }
}