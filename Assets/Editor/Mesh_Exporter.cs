using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class MeshExporter
{
    [MenuItem("Tools/Export Selected Mesh to OBJ")]

    //menu attribute to add the function to the Unity Editor menu

    static void Export()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null)
        {
            Debug.LogError("No object selected!");
            return;
        }

        //Find whats selected

        MeshFilter mf = go.GetComponent<MeshFilter>();
        if (mf == null)
        {
            Debug.LogError("Selected object has no MeshFilter!");
            return;
        }

        //Get the MeshFilter component

        Mesh mesh = mf.sharedMesh;
        if (mesh == null)
        {
            Debug.LogError("No mesh found!");
            return;
        }

        // Gives the actual mesh data , references the asset and not runtime instance

        string path = EditorUtility.SaveFilePanel("Export OBJ", "", go.name + ".obj", "obj");
        if (string.IsNullOrEmpty(path))
            return;


        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.Write(MeshToString(mesh, go.transform));
        }

        Debug.Log("Exported " + go.name + " to " + path);
    }

    // Opens save file dialog and writes to the specified path    

    static string MeshToString(Mesh mesh, Transform transform)
    {
        StringBuilder sb = new StringBuilder();
        // makes a changeable string 

        sb.Append("g ").Append(mesh.name).Append("\n");
        foreach (Vector3 v in mesh.vertices)
        {
            Vector3 wv = transform.TransformPoint(v);
            sb.Append(string.Format("v {0} {1} {2}\n", -wv.x, wv.y, wv.z));
        }

        // write g to the buffer, g in the file format groups the vertices
        /// get vertex positions, turns local space to world space, builds the formatted obj string
        /// -x because of course every software has to use different axis conventions for some godforsaken reason

        sb.Append("\n");
        foreach (Vector3 n in mesh.normals)
        {
            Vector3 wn = transform.TransformDirection(n);
            sb.Append(string.Format("vn {0} {1} {2}\n", -wn.x, wn.y, wn.z));
        }

        //does the same for normals /n for new line

        sb.Append("\n");
        foreach (Vector2 uv in mesh.uv)
        {
            sb.Append(string.Format("vt {0} {1}\n", uv.x, uv.y));
        }

        // does the same for UVs

        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            int[] tris = mesh.GetTriangles(i);
            for (int t = 0; t < tris.Length; t += 3)
            {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                    tris[t] + 1, tris[t + 1] + 1, tris[t + 2] + 1));
            }
        }

        // Goes through sub meshes, gets triangle indexes, writes the faces

        return sb.ToString();
    }
}

//// I couldve just made a matching cylinder in maya or found a package why did i do this
