// CreatePlane
// Credit: http://wiki.unity3d.com/index.php?title=CreatePlane

//  1. Using the inspector, create a new C# script.
//  2. Name the script "CreatePlane" and place it in a folder titled "Editor".
//  3. A new menu option titled "Custom Plane" will appear in "GameObject > Create Other". 

using UnityEditor;
using UnityEngine;

namespace EcsDamageBubbles.Editor
{
    public class CreatePlane : ScriptableWizard
    {
        public enum AnchorPoint
        {
            TopLeft,
            TopHalf,
            TopRight,
            RightHalf,
            BottomRight,
            BottomHalf,
            BottomLeft,
            LeftHalf,
            Center
        }

        public enum Orientation
        {
            Horizontal,
            Vertical
        }

        private static Camera _cam;
        private static Camera _lastUsedCam;

        public int widthSegments = 1;
        public int lengthSegments = 1;
        public float width = 1.0f;
        public float length = 1.0f;
        public Orientation orientation = Orientation.Horizontal;
        public AnchorPoint anchor = AnchorPoint.Center;
        public bool addCollider;
        public bool createAtOrigin = true;
        public string optionalName;


        private void OnWizardCreate()
        {
            var plane = new GameObject();

            if (!string.IsNullOrEmpty(optionalName))
                plane.name = optionalName;
            else
                plane.name = "Plane";

            if (!createAtOrigin && _cam)
                plane.transform.position = _cam.transform.position + _cam.transform.forward * 5.0f;
            else
                plane.transform.position = Vector3.zero;

            Vector2 anchorOffset;
            string anchorId;
            switch (anchor)
            {
                case AnchorPoint.TopLeft:
                    anchorOffset = new Vector2(-width / 2.0f, length / 2.0f);
                    anchorId = "TL";
                    break;
                case AnchorPoint.TopHalf:
                    anchorOffset = new Vector2(0.0f, length / 2.0f);
                    anchorId = "TH";
                    break;
                case AnchorPoint.TopRight:
                    anchorOffset = new Vector2(width / 2.0f, length / 2.0f);
                    anchorId = "TR";
                    break;
                case AnchorPoint.RightHalf:
                    anchorOffset = new Vector2(width / 2.0f, 0.0f);
                    anchorId = "RH";
                    break;
                case AnchorPoint.BottomRight:
                    anchorOffset = new Vector2(width / 2.0f, -length / 2.0f);
                    anchorId = "BR";
                    break;
                case AnchorPoint.BottomHalf:
                    anchorOffset = new Vector2(0.0f, -length / 2.0f);
                    anchorId = "BH";
                    break;
                case AnchorPoint.BottomLeft:
                    anchorOffset = new Vector2(-width / 2.0f, -length / 2.0f);
                    anchorId = "BL";
                    break;
                case AnchorPoint.LeftHalf:
                    anchorOffset = new Vector2(-width / 2.0f, 0.0f);
                    anchorId = "LH";
                    break;
                case AnchorPoint.Center:
                default:
                    anchorOffset = Vector2.zero;
                    anchorId = "C";
                    break;
            }

            var meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
            plane.AddComponent(typeof(MeshRenderer));

            var planeAssetName = plane.name + widthSegments + "x" + lengthSegments + "W" + width + "L" + length +
                                 (orientation == Orientation.Horizontal ? "H" : "V") + anchorId + ".asset";
            var m = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/Editor/" + planeAssetName, typeof(Mesh));

            if (m == null)
            {
                m = new Mesh();
                m.name = plane.name;

                var hCount2 = widthSegments + 1;
                var vCount2 = lengthSegments + 1;
                var numTriangles = widthSegments * lengthSegments * 6;
                var numVertices = hCount2 * vCount2;

                var vertices = new Vector3[numVertices];
                var uvs = new Vector2[numVertices];
                var triangles = new int[numTriangles];

                var index = 0;
                var uvFactorX = 1.0f / widthSegments;
                var uvFactorY = 1.0f / lengthSegments;
                var scaleX = width / widthSegments;
                var scaleY = length / lengthSegments;
                for (var y = 0.0f; y < vCount2; y++)
                for (var x = 0.0f; x < hCount2; x++)
                {
                    if (orientation == Orientation.Horizontal)
                        vertices[index] = new Vector3(x * scaleX - width / 2f - anchorOffset.x, 0.0f,
                            y * scaleY - length / 2f - anchorOffset.y);
                    else
                        vertices[index] = new Vector3(x * scaleX - width / 2f - anchorOffset.x,
                            y * scaleY - length / 2f - anchorOffset.y, 0.0f);
                    uvs[index++] = new Vector2(x * uvFactorX, y * uvFactorY);
                }

                index = 0;
                for (var y = 0; y < lengthSegments; y++)
                for (var x = 0; x < widthSegments; x++)
                {
                    triangles[index] = y * hCount2 + x;
                    triangles[index + 1] = (y + 1) * hCount2 + x;
                    triangles[index + 2] = y * hCount2 + x + 1;

                    triangles[index + 3] = (y + 1) * hCount2 + x;
                    triangles[index + 4] = (y + 1) * hCount2 + x + 1;
                    triangles[index + 5] = y * hCount2 + x + 1;
                    index += 6;
                }

                m.vertices = vertices;
                m.uv = uvs;
                m.triangles = triangles;
                m.RecalculateNormals();

                AssetDatabase.CreateAsset(m, "Assets/Editor/" + planeAssetName);
                AssetDatabase.SaveAssets();
            }

            meshFilter.sharedMesh = m;
            m.RecalculateBounds();

            if (addCollider)
                plane.AddComponent(typeof(BoxCollider));

            Selection.activeObject = plane;
        }


        private void OnWizardUpdate()
        {
            widthSegments = Mathf.Clamp(widthSegments, 1, 254);
            lengthSegments = Mathf.Clamp(lengthSegments, 1, 254);
        }


        [MenuItem("GameObject/Create Other/Custom Plane...")]
        private static void CreateWizard()
        {
            _cam = Camera.current;
            // Hack because camera.current doesn't return editor camera if scene view doesn't have focus
            if (!_cam)
                _cam = _lastUsedCam;
            else
                _lastUsedCam = _cam;
            DisplayWizard("Create Plane", typeof(CreatePlane));
        }
    }
}